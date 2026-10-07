using System.ComponentModel.DataAnnotations;

namespace HRMS.Dtos.Employees
{
    public class SaveEmployeeDto
    {
        public long? Id { get; set; }
        public string FirstNmae { get; set; }

        public string LastNmae { get; set; }

         
        public string? Email { get; set; } // (?)==>  Optional / Nullable
        
        public long? PositionId { get; set; }

        public DateTime BirthDate { get; set; }
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public DateTime StartDate { get; set; }// Required.

        public DateTime? EndDate { get; set; } //Optional

        public decimal? Salary { get; set; }

        public long? DepartmentId { get; set; }
        public long? MangerId { get; set; }
    }
}
