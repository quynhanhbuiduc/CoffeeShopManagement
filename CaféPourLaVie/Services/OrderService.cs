using CaféPourLaVie.Data;
using CaféPourLaVie.Models;
using CaféPourLaVie.Models.Enums;
using CaféPourLaVie.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace CaféPourLaVie.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        private readonly CartService _cartService;

        public OrderService(ApplicationDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public async Task<int> CheckoutAsync(ClaimsPrincipal user, int paymentMethodId)
        {
            var cart = _cartService.GetCart();

            if (cart.Count == 0)
            {
                throw new InvalidOperationException("Giỏ hàng trống.");
            }

            // Take the current user's account ID from claims
            var accountIdClaim = user.Claims
                .FirstOrDefault(c => c.Type == "AccountId");

            if (accountIdClaim == null)
            {
                throw new Exception("Không tìm thấy thông tin tài khoản.");
            }

            if (!int.TryParse(accountIdClaim.Value, out int accountId))
            {
                throw new Exception("Thông tin tài khoản không hợp lệ.");
            }

            // Create a new order
            var order = new Order
            {
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(x => x.SubTotal),
                AccountId = accountId,
                PaymentMethodId = paymentMethodId,
                Status = OrderStatus.Completed,
                OrderDetails = new List<OrderDetail>()
            };

            foreach (var item in cart)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(
                        p => p.ProductId == item.ProductId);

                if (product == null)
                {
                    throw new Exception(
                        "Có sản phẩm không còn tồn tại.");
                }

                if (product.Quantity < item.Quantity)
                {
                    throw new InvalidOperationException(
                        $"Sản phẩm {product.ProductName} không đủ hàng.");
                }

                order.OrderDetails.Add(
                    new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        SubTotal = item.SubTotal
                    });

                // Update the product quantity in stock
                product.Quantity -= item.Quantity;

                // Save the inventory transaction for the sale
                _context.InventoryTransactions.Add(
                    new InventoryTransaction
                    {
                        ProductId = product.ProductId,
                        TransactionDate = DateTime.Now,
                        Quantity = item.Quantity,
                        Type = InventoryTransactionType.Sale,
                        Note = "Bán hàng"
                    });
            }


            // Save the order and order details to the database within a transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Tell EF Core to track the new order and its details
                _context.Orders.Add(order);

                await _context.SaveChangesAsync(); // This will save the order and generate the OrderId for the order

                await transaction.CommitAsync();

                _cartService.ClearCart();

                return order.OrderId; // At this point, OrderId will return the actual ID from SQL Server (e.g., 1, 2, 3...)
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task CancelOrderAsync(int orderId)
        {
            // Retrieve the order and its details from the database
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);


            if (order == null)
                throw new Exception("Không tìm thấy đơn hàng");


            if (order.Status == OrderStatus.Cancelled)
                throw new Exception("Đơn đã hủy");

            // Restore the product quantities in stock
            foreach (var detail in order.OrderDetails)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(
                        p => p.ProductId == detail.ProductId);

                if (product != null)
                {
                    // Restore the quantity of the product in stock
                    product.Quantity += detail.Quantity;

                    // Save the inventory transaction for the cancellation
                    _context.InventoryTransactions.Add(
                        new InventoryTransaction
                        {
                            ProductId = product.ProductId,
                            TransactionDate = DateTime.Now,
                            Quantity = detail.Quantity,
                            Type = InventoryTransactionType.CancelOrder,
                            Note = $"Hoàn kho do hủy đơn #{order.OrderId}"
                        });
                }
            }

            // Update the order status to Cancelled
            order.Status = OrderStatus.Cancelled;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Account)
                .Include(o => o.PaymentMethod)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders

                .Include(o => o.Account)

                .Include(o => o.PaymentMethod)

                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)

                .FirstOrDefaultAsync(
                    o => o.OrderId == id);
        }
    }
}