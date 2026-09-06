using System.ComponentModel.DataAnnotations;

namespace CaféPourLaVie.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [StringLength(100)]
        public string EmployeeName { get; set; }
        
        
        [MaxLength(100)]                
        public string Email { get; set; }


        public string Phone { get; set; }


        [MaxLength(250)]
        public string Address { get; set; }


        public DateTime HireDate { get; set; }


        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
