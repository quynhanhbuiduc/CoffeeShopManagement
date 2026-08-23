using CaféPourLaVie.Models.Enums;

namespace CaféPourLaVie.Models
{
    public class InventoryTransaction
    {
        public int InventoryTransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public int Quantity { get; set; }

        public InventoryTransactionType Type { get; set; }

        public string? Note { get; set; }


        // Sản phẩm
        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}