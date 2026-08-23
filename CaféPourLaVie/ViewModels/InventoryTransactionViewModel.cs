namespace CaféPourLaVie.ViewModels
{
    public class InventoryTransactionViewModel
    {
        public DateTime TransactionDate { get; set; }

        public string ProductName { get; set; }

        public string Type { get; set; }

        public int Quantity { get; set; }

        public string? Note { get; set; }
    }
}