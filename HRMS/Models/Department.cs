using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class Department
    {
        public long Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public int? FloorNumber { get; set; } // Nullable integer for optional property

        //Navigation prop
      //  public ICollection<Employee>? Employee { get; set; } // Many Employees

         
    }
}
