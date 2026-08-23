namespace CaféPourLaVie.ViewModels
{
    public class ReportViewModel
    {
        // =========================
        // FILTER
        // =========================
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }


        // =========================
        // SALES
        // =========================
        public decimal TotalRevenue { get; set; }

        public int TotalOrders { get; set; }

        public decimal AverageOrderValue { get; set; }


        // =========================
        // PRODUCTS
        // =========================
        public List<ReportProductViewModel> ProductSales { get; set; } = new();


        // =========================
        // INVENTORY
        // =========================
        public int TotalProducts { get; set; }

        public int LowStockProducts { get; set; }

        public decimal InventoryValue { get; set; }


        // =========================
        // IMPORT
        // =========================
        public int TotalImportReceipts { get; set; }

        public decimal TotalImportValue { get; set; }


        // =========================
        // REVENUE BY DATE
        // =========================
        public List<ReportRevenueViewModel> RevenueByDate { get; set; } = new();
    }
}