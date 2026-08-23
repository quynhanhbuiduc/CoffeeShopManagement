using CaféPourLaVie.Data;
using CaféPourLaVie.Models.Enums;
using CaféPourLaVie.Services.Interfaces;
using CaféPourLaVie.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaféPourLaVie.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> GetDashboardAsync(ClaimsPrincipal user)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);


            // =========================
            // TODAY ORDERS
            // =========================
            var todayOrders = _context.Orders
                .Where(o =>
                    o.OrderDate >= today &&
                    o.OrderDate < tomorrow &&
                    o.Status == OrderStatus.Completed);

            var todayRevenue = await todayOrders
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

            var todayOrderCount = await todayOrders
                .CountAsync();


            // =========================
            // TOTAL PRODUCTS
            // =========================
            var totalProducts = await _context.Products.CountAsync();


            // =========================
            // LOW STOCK
            // =========================
            var lowStockQuery = _context.Products.Where(p => p.Quantity < 10);

            var lowStockCount = await lowStockQuery.CountAsync();

            var lowStockList = await lowStockQuery
                .Include(p => p.Category)
                .OrderBy(p => p.Quantity)
                .ToListAsync();


            // =========================
            // TOP PRODUCTS
            // =========================
            var topProducts = await _context.OrderDetails
                .Where(od =>
                    od.Order.Status == OrderStatus.Completed &&
                    od.Product != null)
                .GroupBy(od => new
                {
                    od.ProductId,
                    od.Product.ProductName
                })
                .Select(g => new TopProductViewModel
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    QuantitySold = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.SubTotal)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(5)
                .ToListAsync();


            // =========================
            // RECENT ORDERS
            // =========================
            var recentOrders = await _context.Orders
                .Include(o => o.Account)
                .Include(o => o.PaymentMethod)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToListAsync();


            // =========================
            // RETURN DASHBOARD
            // =========================
            return new DashboardViewModel
            {
                TodayRevenue = todayRevenue,

                TodayOrders = todayOrderCount,

                TotalProducts = totalProducts,

                LowStockProducts = lowStockCount,

                TopProducts = topProducts,

                LowStockList = lowStockList,

                RecentOrders = recentOrders
            };
        }
    }
}