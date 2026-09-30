using HRMS.DbContexts;
using HRMS.Dtos.Employees;
using HRMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Controllers
{
    //Data Annotations : Extra Informations

    [Route("api/[controller]")] //api/Employees
    [ApiController]
    public class EmployeesController : ControllerBase


    {

        //HRMSContext _dbContext = new HRMSContext();

        // Dependency Injection
        private readonly HMRSContext _dbContext;

        public EmployeesController(HMRSContext dbContext)
        {
            _dbContext = dbContext;
        }


        public static List<Employee> employees = new List<Employee>()
        {
         new Employee(){Id=1,FirstNmae="Ahmad",LastNmae="Alnajjar",Email="ahmadnajar@gmail.com",BirthDate=new DateTime(1995,1,25),PhoneNumber="0792062909",IsActive=true,StartDate=new DateTime(),Salary=1000},
         new Employee(){Id=2,FirstNmae="Omar",LastNmae="Khatib",Email="omar.khatib@gmail.com",BirthDate=new DateTime(1998,5,14),PhoneNumber="0798765432",IsActive=true,StartDate=new DateTime(2022,3,1),Salary=1200},
         new Employee(){Id=3,FirstNmae="Sara",LastNmae="Mansour",Email="sara.mansour@gmail.com",BirthDate=new DateTime(2000,11,30),PhoneNumber="0781234567",IsActive=false,StartDate=new DateTime(2023,1,15),Salary=950},
         new Employee(){Id=4,FirstNmae="Khaled",LastNmae="Othman",Email="khaled.othman@gmail.com",BirthDate=new DateTime(1992,8,9),PhoneNumber="0775554321",IsActive=true,StartDate=new DateTime(2020,6,10),Salary=1500}
        };


        //[HttpGet("GetByCriteria")]// بترجع كل معلومات الموظفين الموجودين عندي
        //public IActionResult GetByCriteria()

        //{
        //    //
        //    var data = from emp in employees
        //                orderby emp.Id descending
        //               select new
        //               {
        //                   Id = emp.Id,
        //                   Name = emp.FirstNmae + " " + emp.LastNmae,
        //                   Position = emp.Position,
        //                   BirthDate = emp.BirthDate,
        //                   StartDate = emp.StartDate,
        //                   EndDate = emp.EndDate
        //               };
        //    return Ok(data); 
        //}

        //**********************************************************************************************

        //[HttpGet("GetByCriteria")]// بترجع الموظفين بناءا على position
        //public IActionResult GetByCriteria(string? position , string? name)

        //{
        //    //
        //    var data = from emp in employees
        //               where (position == null || emp.Position.ToUpper().Contains( position.ToUpper()))&&
        //               (name ==  null || emp.FirstNmae.ToUpper().Contains( name.ToUpper()))
        //               orderby emp.Id descending
        //               select new EmployeeDto
        //               {
        //                   Id = emp.Id,
        //                   Name = emp.FirstNmae + " " + emp.LastNmae,
        //                   Position = emp.Position,
        //                   BirthDate = emp.BirthDate,
        //                   StartDate = emp.StartDate,
        //                   EndDate = emp.EndDate
        //                   //بحدد فقط المعلومات الي بدي ارجعها لان بعض المعلومات حساسة صعب ارجعها
        //               };

        //    return Ok(data);
        //}


        [HttpGet]// بترجع الموظفين بناءا على position
        public IActionResult GetByCriteria([FromQuery]SearchEmployeeDTO searchemployeeDTO)

        {
            //
            var data = from emp in _dbContext.Employees
                       from dep in _dbContext.Departments.Where(x => x.Id==emp.DepartmentId).DefaultIfEmpty()// join // inner join / left join(.DefaultIfEmpty())
                       from Manger in _dbContext.Employees.Where(x=>x.Id==emp.MangerId).DefaultIfEmpty()
                       where (searchemployeeDTO.Position == null || emp.Position.ToUpper().Contains(searchemployeeDTO.Position.ToUpper())) &&
                       (searchemployeeDTO.Name == null || emp.FirstNmae.ToUpper().Contains(searchemployeeDTO.Name.ToUpper()))
                       orderby emp.Id descending
                       select new EmployeeDto
                       {
                           Id = emp.Id,
                           Name = emp.FirstNmae + " " + emp.LastNmae,
                           Position = emp.Position,
                           BirthDate = emp.BirthDate,
                           StartDate = emp.StartDate,
                           EndDate = emp.EndDate,
                           PhoneNumber = emp.PhoneNumber,
                           IsActive = emp.IsActive,
                           Salary = emp.Salary,
                           Email = emp.Email,
                           DepartmentId = emp.DepartmentId,
                           MangerId = emp.MangerId,
                           DepartmentName = dep.Name,
                           MangerName= Manger.FirstNmae + " " + Manger.LastNmae

                           //بحدد فقط المعلومات الي بدي ارجعها لان بعض المعلومات حساسة صعب ارجعها
                       };


            return Ok(data);
        }




        [HttpGet("{id:long}")]//Route parameter
        public IActionResult GetById(long id)
        {
            var data = employees.Select(emp => new EmployeeDto
            {
                Id = emp.Id,
                Name = emp.FirstNmae + " " + emp.LastNmae,
                Position = emp.Position,
                BirthDate = emp.BirthDate,
                StartDate = emp.StartDate,
                EndDate = emp.EndDate

            }).FirstOrDefault(emp => emp.Id == id);

            // var data = employees.SingleOrDefault(emp=>emp.Id == id);
            if (data == null)
            {
                return NotFound("Employee Not Found");
            }
            return Ok(data);

        }

        [HttpPost("{id:long}")]
        public IActionResult Add(SaveEmployeeDto employeeDto)
        {
            var employee = new Employee()
            {
                 Id = (employees.LastOrDefault()?.Id ?? 0) + 1,
                FirstNmae = employeeDto.FirstNmae,
                LastNmae = employeeDto.LastNmae,
                Position = employeeDto.Position,
                BirthDate = employeeDto.BirthDate,
                StartDate = employeeDto.StartDate,
                EndDate = employeeDto.EndDate,
                Email = employeeDto.Email,
                IsActive = employeeDto.IsActive,
                PhoneNumber = employeeDto.PhoneNumber,
                Salary = employeeDto.Salary,
            };

            employees.Add(employee);

            return Ok(employee.Id);



        }
        [HttpPut("{id:long}")] //Resors Update
        public IActionResult UpDate([FromQuery]long id,[FromBody]SaveEmployeeDto employeeDto)
        {
            if (id != employeeDto.Id)
            {
                return BadRequest("Id Mismatch"); //404
            }
            var employee = employees.FirstOrDefault(x => x.Id == employeeDto.Id);
            if (employee == null)
            {
                return NotFound("Employee Dose Not Exist");
            }
            employee.FirstNmae = employeeDto.FirstNmae;
            employee.LastNmae = employeeDto.LastNmae;
            employee.PhoneNumber = employeeDto.PhoneNumber;
            employee.BirthDate = employeeDto.BirthDate;
            employee.StartDate = employeeDto.StartDate;
            employee.EndDate = employeeDto.EndDate;
            employee.Email = employeeDto.Email;
            employee.IsActive = employeeDto.IsActive;
            employee.Position = employeeDto.Position;

            return Ok();
        }
        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            var employee = employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
                return NotFound("Employee Dose Not Exist");
            }

            employees.Remove(employee);
            return Ok();
        }

        // Query Parameter => [FromQuery]
        // Request Body => [FromBody]

        // Simple Data type => string, int, long... --> (By Default) Query Parameters
        // Complix Data type => Model, Dto, Object.. --> (By Default) Request Body

        // Method Can Use Multiple Parameters Of Type [fromQuery]
        // Method Can Not Use Multiple Parameters Of Type [FromBody]




        //**********************************************************************************************
        //[HttpGet]
        //public IActionResult GetEmployees()
        //{
        //    return Ok(new {Name="ali",Age=22});//200 ok
        //   // return BadRequest("Data Not Loaded"); //400 Bad Request
        //    //return NotFound("Employee Not Found");//404 Not Found
        //    ////هون فش ميثود جاهزة انت صنعها (number,massege)
        //    //return StatusCode(500, "Something went Wrong");  //500 Internal Error
        //}


        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    return Ok(new { Name = "ali", Age = 22 });//200 ok
        //                                              // return BadRequest("Data Not Loaded"); //400 Bad Request
        //                                              //return NotFound("Employee Not Found");//404 Not Found
        //                                              ////هون فش ميثود جاهزة انت صنعها (number,massege)
        //                                              //return StatusCode(500, "Something went Wrong");  //500 Internal Error
        //}


        //**********************************************************************************************

        //[HttpGet("GetEmployees")]
        //public IActionResult GetEmployees()
        //{
        //    return Ok(new { Name = "ali", Age = 22 });//200 ok
        //                                              // return BadRequest("Data Not Loaded"); //400 Bad Request
        //                                              //return NotFound("Employee Not Found");//404 Not Found
        //                                              ////هون فش ميثود جاهزة انت صنعها (number,massege)
        //                                              //return StatusCode(500, "Something went Wrong");  //500 Internal Error
        //}


        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    // return Ok(new { Name = "ali", Age = 22 });//200 ok
        //    return BadRequest("Data Not Loaded"); //400 Bad Request
        //                                          //return NotFound("Employee Not Found");//404 Not Found
        //                                          ////هون فش ميثود جاهزة انت صنعها (number,massege)
        //                                          //return StatusCode(500, "Something went Wrong");  //500 Internal Error
        //}





    }


}


