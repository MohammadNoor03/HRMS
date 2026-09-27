namespace HRMS.Models
{
    public class Department
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? FloorNumber { get; set; } // Nullable integer for optional property
    }
}
