using CaféPourLaVie.Data;
using CaféPourLaVie.Services;
using CaféPourLaVie.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CaféPourLaVie.Controllers
{

    [Authorize(Roles = "Admin,Employee")]
    public class SalesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly CartService _cartService;

        private readonly IOrderService _orderService;

        public SalesController(
            ApplicationDbContext context,
            CartService cartService,
            IOrderService orderService)
        {
            _context = context;
            _cartService = cartService;
            _orderService = orderService;
        }

        // GET: Sales
        public async Task<IActionResult> Index( int? categoryId, string? searchString)
        {
            ViewBag.Categories = await _context.Categories
                .ToListAsync();

            var products = _context.Products
                .Include(p => p.Category)
                .Where(p => p.Status);

            if (categoryId != null)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString));
            }

            ViewBag.SelectedCategory = categoryId;
            ViewBag.SearchString = searchString;

            return View(await products.ToListAsync());
        }

        // POST: Sales/AddToCart
        public async Task<IActionResult> AddToCart(int id, int? categoryId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);


            if (product == null)
            {
                return NotFound();
            }


            _cartService.AddToCart(product);


            return RedirectToAction(
                "Index",
                new { categoryId = categoryId }
            );
        }

        // GET: Sales/Cart
        public IActionResult Cart()
        {
            var cart = _cartService.GetCart();

            return View(cart);
        }

        // POST: Sales/RemoveFromCart
        public IActionResult RemoveFromCart(int id)
        {

            _cartService.RemoveFromCart(id);


            return RedirectToAction("Cart");

        }


        // GET: Sales/Success
        public IActionResult Success(int id)
        {
            ViewBag.OrderId = id;

            return View();
        }


        // GET: Sales/UpdateQuantity
        public IActionResult Increase(int id)
        {
            var cart = _cartService.GetCart();
            var item = cart.FirstOrDefault(x => x.ProductId == id);

            // 1. Take the product from the database to check its quantity
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            // 2. Check if the item exists in the cart and the product exists in the database
            if (item != null && product != null)
            {
                // 3. Compare quantities
                if (item.Quantity < product.Quantity)
                {
                    _cartService.UpdateQuantity(id, item.Quantity + 1);
                }
            }

            return RedirectToAction("Cart");
        }
        public IActionResult Decrease(int id)
        {
            var cart = _cartService.GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
            {
                _cartService.UpdateQuantity(id, item.Quantity - 1);
            }

            return RedirectToAction("Cart");
        }

        // GET: Sales/Checkout
        public async Task<IActionResult> Checkout()
        {
            var cart = _cartService.GetCart();

            if (cart.Count == 0)
            {
                return RedirectToAction("Cart");
            }


            ViewBag.PaymentMethods = await _context.PaymentMethods
                .ToListAsync();


            return View(cart);
        }
        // POST: Sales/Checkout
        [HttpPost]
        public async Task<IActionResult> Checkout(int paymentMethodId)
        {
            try
            {
                int orderId = await _orderService.CheckoutAsync(
                    User,
                    paymentMethodId
                );


                return RedirectToAction(
                    "Success",
                    new { id = orderId }
                );
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;

                return RedirectToAction("Cart");
            }
        }
    }

}