using System.ComponentModel.DataAnnotations;

namespace CaféPourLaVie.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }


        [MaxLength(100)]
        public string MethodName { get; set; }


        public ICollection<Order> Orders { get; set; }
    }
}
