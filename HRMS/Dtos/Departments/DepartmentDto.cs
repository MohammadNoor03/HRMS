namespace HRMS.Dtos.Departments
{
    public class DepartmentDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? FloorNumber { get; set; }
    }
}