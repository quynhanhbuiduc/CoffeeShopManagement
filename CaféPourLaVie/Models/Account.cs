namespace CaféPourLaVie.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public bool Status { get; set; }



        // Navigation property for the related Employee entity
        public Employee Employee { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
