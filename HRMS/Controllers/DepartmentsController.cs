using HRMS.Dtos.Departments;
using HRMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        public DepartmentsController()
        {

        }

        public static List<Department> departments = new List<Department>()
        {
            new Department(){ Id = 1, Name = "HR", Description = "Human Resources", FloorNumber = 1 },
            new Department(){ Id = 2, Name = "IT", Description = "Information Technology", FloorNumber = 2 },
            new Department(){ Id = 3, Name = "Finance", Description = "Financial Department", FloorNumber = 1 },
            new Department(){ Id = 4, Name = "Marketing", Description = "Marketing & Sales", FloorNumber = 3 }
        };

        [HttpGet]
        public IActionResult GetByCriteria([FromQuery] SearchDepartmentsDto searchDepartmentsDto)
        {
            var data = from dept in departments
                       where (searchDepartmentsDto.Name == null || dept.Name.ToUpper().Contains(searchDepartmentsDto.Name.ToUpper())) &&
                             (searchDepartmentsDto.FloorNumber == null || dept.FloorNumber == searchDepartmentsDto.FloorNumber)
                       orderby dept.Id descending
                       select new DepartmentDto
                       {
                           Id = dept.Id,
                           Name = dept.Name,
                           Description = dept.Description,
                           FloorNumber = dept.FloorNumber
                       };

            return Ok(data);
        }

        [HttpGet("{id:long}")]
        public IActionResult GetById(long id)
        {
            var data = departments.Select(dept => new DepartmentDto
            {
                Id = dept.Id,
                Name = dept.Name,
                Description = dept.Description,
                FloorNumber = dept.FloorNumber
            }).FirstOrDefault(dept => dept.Id == id);

            if (data == null)
            {
                return NotFound("Department Not Found");
            }
            return Ok(data);
        }

        [HttpPost]
        public IActionResult Add(DepartmentDto departmentDto)
        {
            var department = new Department()
            {
                Id = (departments.LastOrDefault()?.Id ?? 0) + 1,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                FloorNumber = departmentDto.FloorNumber
            };

            departments.Add(department);

            return Ok(department.Id);
        }

        [HttpPut("{id:long}")]
        public IActionResult UpDate([FromQuery] long id, [FromBody] DepartmentDto departmentDto)
        {
            if (id != departmentDto.Id)
            {
                return BadRequest("Id Mismatch");
            }
            var department = departments.FirstOrDefault(x => x.Id == departmentDto.Id);
            if (department == null)
            {
                return NotFound("Department Dose Not Exist");
            }

            department.Name = departmentDto.Name;
            department.Description = departmentDto.Description;
            department.FloorNumber = departmentDto.FloorNumber;

            return Ok();
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            var department = departments.FirstOrDefault(x => x.Id == id);
            if (department == null)
            {
                return NotFound("Department Dose Not Exist");
            }

            departments.Remove(department);
            return Ok();
        }
    }
}