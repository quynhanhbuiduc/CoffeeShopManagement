namespace CaféPourLaVie.Models
{
    public class PaymentMethod
    {
        public int PaymentMethodId { get; set; }


        public string MethodName { get; set; }


        public ICollection<Order> Orders { get; set; }
    }
}
