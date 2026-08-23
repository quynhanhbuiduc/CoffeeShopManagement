using CaféPourLaVie.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaféPourLaVie.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            var dashboard = await _dashboardService.GetDashboardAsync(User);

            return View(dashboard);
        }
    }
}