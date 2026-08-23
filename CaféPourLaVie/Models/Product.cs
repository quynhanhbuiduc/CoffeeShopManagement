namespace CaféPourLaVie.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Image { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedDate { get; set; }


        // Foreign Key
        public int CategoryId { get; set; }  


        // Navigation property
        public Category Category { get; set; }
    }
}
