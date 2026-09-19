using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class Employee
    {
        // معلومات اجبارية ومعلومات اختيارية
        public long Id { get; set; }
        public string FirstNmae { get; set; }

        public string LastNmae { get; set; }

        
        public string? Email { get; set; } // (?)==>  Optional / Nullable
        
        public string Position { get; set; }

        public DateTime BirthDate { get; set; }
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }// Required.

        public DateTime? EndDate { get; set; } //Optional

        public decimal? Salary { get; set; }



    }
}
