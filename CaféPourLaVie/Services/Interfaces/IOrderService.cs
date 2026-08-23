using CaféPourLaVie.Models;
using System.Security.Claims;

namespace CaféPourLaVie.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> CheckoutAsync(ClaimsPrincipal user, int paymentMethodId);

        Task<List<Order>> GetAllAsync();


        Task<Order?> GetByIdAsync(int id);

        Task CancelOrderAsync(int orderId);
    }
}