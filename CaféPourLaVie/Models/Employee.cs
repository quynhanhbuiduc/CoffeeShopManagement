namespace CaféPourLaVie.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }
        
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public DateTime HireDate { get; set; }


        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
