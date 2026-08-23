using System.ComponentModel.DataAnnotations;

namespace CaféPourLaVie.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên danh mục.")]
        [MaxLength(50, ErrorMessage = "Tên không được quá 50 ký tự.")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Mô tả tối đa 200 ký tự")]
        public string Description { get; set; } = string.Empty;


        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
