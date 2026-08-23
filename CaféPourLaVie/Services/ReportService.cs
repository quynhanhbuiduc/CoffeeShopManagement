using CaféPourLaVie.Data;
using CaféPourLaVie.Models.Enums;
using CaféPourLaVie.Services.Interfaces;
using CaféPourLaVie.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CaféPourLaVie.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReportViewModel> GetReportAsync(
            DateTime fromDate,
            DateTime toDate)
        {
            // =========================
            // DATE RANGE
            // =========================
            var startDate = fromDate.Date;
            var endDate = toDate.Date.AddDays(1);


            // =========================
            // SALES
            // =========================
            var orders = _context.Orders
                .Where(o =>
                    o.OrderDate >= startDate &&
                    o.OrderDate < endDate &&
                    o.Status == OrderStatus.Completed);


            var totalRevenue = await orders
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0;


            var totalOrders = await orders
                .CountAsync();


            var averageOrderValue = totalOrders > 0
                ? totalRevenue / totalOrders
                : 0;


            // =========================
            // PRODUCT SALES
            // =========================
            var productSales = await _context.OrderDetails
                .Where(od =>
                    od.Order.Status == OrderStatus.Completed &&
                    od.Order.OrderDate >= startDate &&
                    od.Order.OrderDate < endDate)
                .GroupBy(od => new
                {
                    od.ProductId,
                    od.Product.ProductName
                })
                .Select(g => new ReportProductViewModel
                {
                    ProductId = g.Key.ProductId,

                    ProductName = g.Key.ProductName,

                    QuantitySold = g.Sum(x => x.Quantity),

                    Revenue = g.Sum(x => x.SubTotal)
                })
                .OrderByDescending(x => x.QuantitySold)
                .ToListAsync();


            // =========================
            // REVENUE BY DATE
            // =========================
            var revenueByDate = await _context.Orders
                .Where(o =>
                    o.OrderDate >= startDate &&
                    o.OrderDate < endDate &&
                    o.Status == OrderStatus.Completed)
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new ReportRevenueViewModel
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.TotalAmount),
                    Orders = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();


            // =========================
            // INVENTORY
            // =========================
            var totalProducts = await _context.Products
                .CountAsync();


            var lowStockProducts = await _context.Products
                .CountAsync(p => p.Quantity < 10);


            // inventory value = sum of (quantity * price)
            var inventoryValue = await _context.Products
                .SumAsync(p => (decimal?)(
                    p.Quantity * p.Price
                )) ?? 0;


            // =========================
            // IMPORT
            // =========================
            var importReceipts = _context.ImportReceipts
                .Where(r =>
                    r.ImportDate >= startDate &&
                    r.ImportDate < endDate);


            var totalImportReceipts = await importReceipts
                .CountAsync();


            var totalImportValue = await importReceipts
                .SumAsync(r => (decimal?)r.TotalAmount) ?? 0;


            // =========================
            // RETURN
            // =========================
            return new ReportViewModel
            {
                FromDate = fromDate,

                ToDate = toDate,

                TotalRevenue = totalRevenue,

                TotalOrders = totalOrders,

                AverageOrderValue = averageOrderValue,

                ProductSales = productSales,

                RevenueByDate = revenueByDate,

                TotalProducts = totalProducts,

                LowStockProducts = lowStockProducts,

                InventoryValue = inventoryValue,

                TotalImportReceipts = totalImportReceipts,

                TotalImportValue = totalImportValue
            };
        }
    }
}