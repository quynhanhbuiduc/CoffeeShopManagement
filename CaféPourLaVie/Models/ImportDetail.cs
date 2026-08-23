namespace CaféPourLaVie.Models
{
    public class ImportDetail
    {
        public int ImportDetailId { get; set; }


        public int ImportReceiptId { get; set; }
        public ImportReceipt ImportReceipt { get; set; }


        public int ProductId { get; set; }
        public Product Product { get; set; }


        public int Quantity { get; set; }

        public decimal ImportPrice { get; set; }

        public decimal SubTotal { get; set; }
    }
}