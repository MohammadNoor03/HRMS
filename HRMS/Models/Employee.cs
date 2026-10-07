using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models
{
    public class Employee
    {
        // معلومات اجبارية ومعلومات اختيارية
        [Key]
        public long Id { get; set; }

        [MaxLength(50)]

        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; } // (?)==>  Optional / Nullable

        //[MaxLength(50)]
        //public string Position { get; set; }

        public DateTime BirthDate { get; set; }
        [MaxLength(50)]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }// Required.

        public DateTime? EndDate { get; set; } //Optional

        public decimal? Salary { get; set; }
        [ForeignKey("Department")]//Navigation prop اسم ال
        public long? DepartmentId { get; set; }
        public Department? Department { get; set; } // Navigation prop

        [ForeignKey("Manager")]
        public long? ManagerId { get; set; }
        public Employee? Manager { get; set; }// Navigation prop

        [ForeignKey("Lookup")]
        public long? PositionId { get; set; }

        public Lookup? Lookup { get; set; }

    }
}
