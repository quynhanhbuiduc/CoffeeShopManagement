using CaféPourLaVie.Models;
using System.Text.Json;

namespace CaféPourLaVie.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }



        private ISession? Session => _httpContextAccessor.HttpContext?
                                                         .Session;


        /// Get the current cart from the session. If the cart does not exist, return an empty list.
        public List<CartItem> GetCart()
        {
            var json = Session?.GetString("Cart");

            if (string.IsNullOrEmpty(json))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
        }


        // Save the current cart to the session.
        public void SaveCart(List<CartItem> cart)
        {

            Session?.SetString("Cart",JsonSerializer.Serialize(cart));

        }


        // Add a product to the cart.
        // If the product is already in the cart, increase the quantity by 1.
        // If the product is not in the cart, add it with a quantity of 1.
        public void AddToCart(Product product)
        {

            var cart = GetCart();


            var item = cart
                .FirstOrDefault(x =>
                    x.ProductId == product.ProductId);


            if (item == null)
            {
                if (!product.Status)
                    return;
              
                cart.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = 1
                });
            }

            else
                if (item.Quantity < product.Quantity)
                {
                    item.Quantity++;
                }
            
            SaveCart(cart);
        }


        // Remove a product from the cart.
        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();


            var item = cart
                .FirstOrDefault(x => x.ProductId == productId);


            if (item != null)
                cart.Remove(item);
           

            SaveCart(cart);
        }


        // Get the total price of the cart.
        public decimal GetTotal()
        {
            return GetCart().Sum(x => x.SubTotal);
        }


        // Update the quantity of a product in the cart.
        public void UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
                return;

            if (quantity <= 0)
            {
                cart.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            SaveCart(cart);
        }


        // Clear the cart.
        public void ClearCart()
        {
            Session?.Remove("Cart");
        }

    }
}