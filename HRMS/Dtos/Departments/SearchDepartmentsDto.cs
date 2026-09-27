namespace HRMS.Dtos.Departments
{
    public class SearchDepartmentsDto
    {
        public string? Name { get; set; }
        public int? FloorNumber { get; set; }
    }

    public class DepartmentDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? FloorNumber { get; set; }
    }
}