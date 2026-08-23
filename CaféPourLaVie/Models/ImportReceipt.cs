using CaféPourLaVie.Models.Enums;

namespace CaféPourLaVie.Models
{
    public class ImportReceipt
    {
        public int ImportReceiptId { get; set; }

        public DateTime ImportDate { get; set; }

        public decimal TotalAmount { get; set; }

        public ImportStatus Status { get; set; } = ImportStatus.Pending;


        // Relationships
        public int AccountId { get; set; }
        public Account Account { get; set; }


        public ICollection<ImportDetail> ImportDetails { get; set; }
            = new List<ImportDetail>();
    }
}