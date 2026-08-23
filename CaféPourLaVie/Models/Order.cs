using CaféPourLaVie.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace CaféPourLaVie.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;


        public int AccountId { get; set; }
        public Account Account { get; set; }


        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }


        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
