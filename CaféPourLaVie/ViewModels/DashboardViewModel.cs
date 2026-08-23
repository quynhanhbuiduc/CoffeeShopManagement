using CaféPourLaVie.Models;
using Microsoft.EntityFrameworkCore;

namespace CaféPourLaVie.ViewModels
{
    
    public class DashboardViewModel
    {
        // Constructor to initialize the lists
        public decimal TodayRevenue { get; set; }

        public int TodayOrders { get; set; }

        public int TotalProducts { get; set; }

        public int LowStockProducts { get; set; }


        // Top products list
        public List<TopProductViewModel> TopProducts { get; set; }

        // Low stock products list
        public List<Product> LowStockList { get; set; }

        // Recent orders list
        public List<Order> RecentOrders { get; set; }
    }
}
