using CaféPourLaVie.ViewModels;
using System.Security.Claims;

namespace CaféPourLaVie.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardAsync(ClaimsPrincipal user);
    }
}