using System.ComponentModel.DataAnnotations;

namespace CaféPourLaVie.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm.")]
        public string ProductName { get; set; }

        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm không được âm.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không được âm.")]
        public int Quantity { get; set; }

        public string? Image { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedDate { get; set; }


        // Foreign Key
        public int CategoryId { get; set; }  


        // Navigation property
        public Category Category { get; set; }
    }
}
