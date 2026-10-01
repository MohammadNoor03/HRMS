using HRMS.DbContexts;
using HRMS.Dtos.Employees;
using HRMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.Arm;

namespace HRMS.Controllers
{
    //Data Annotations : Extra Informations

    [Route("api/[controller]")] //api/Employees
    [ApiController]
    public class EmployeesController : ControllerBase


    {

        //HRMSContext _dbContext = new HRMSContext();

        // Dependency Injection

        // read only (بعطيه قيمة عند الانشاء او في الكونستركتر)

        private readonly HMRSContext _dbContext;

        public EmployeesController(HMRSContext dbContext)
        {
            _dbContext = dbContext;
        }


       

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
                       from Manger in _dbContext.Employees.Where(x=>x.Id==emp.ManagerId).DefaultIfEmpty()
                       where (searchemployeeDTO.Position == null || emp.Position.ToUpper().Contains(searchemployeeDTO.Position.ToUpper())) &&
                       (searchemployeeDTO.Name == null || emp.FirstName.ToUpper().Contains(searchemployeeDTO.Name.ToUpper()))
                       orderby emp.Id descending
                       select new EmployeeDto
                       {
                           Id = emp.Id,
                           Name = emp.FirstName + " " + emp.LastName,
                           Position = emp.Position,
                           BirthDate = emp.BirthDate,
                           StartDate = emp.StartDate,
                           EndDate = emp.EndDate,
                           PhoneNumber = emp.PhoneNumber,
                           IsActive = emp.IsActive,
                           Salary = emp.Salary,
                           Email = emp.Email,
                           DepartmentId = emp.DepartmentId,
                           MangerId = emp.ManagerId,
                           DepartmentName = dep.Name,
                           MangerName= Manger.FirstName + " " + Manger.LastName

                           //بحدد فقط المعلومات الي بدي ارجعها لان بعض المعلومات حساسة صعب ارجعها
                       };


            return Ok(data);
        }




        [HttpGet("{id:long}")]//Route parameter
        public IActionResult GetById(long id)
        {
            var data = _dbContext.Employees.Select(emp => new EmployeeDto
            {
                Id = emp.Id,
                Name = emp.FirstName + " " + emp.LastName,
                Position = emp.Position,
                BirthDate = emp.BirthDate,
                StartDate = emp.StartDate,
                EndDate = emp.EndDate,
                PhoneNumber = emp.PhoneNumber,
                IsActive = emp.IsActive,
                Salary = emp.Salary,
                Email = emp.Email,
                DepartmentId = emp.DepartmentId,
                MangerId = emp.ManagerId,
                DepartmentName = "",
                MangerName = " " 

            }).FirstOrDefault(emp => emp.Id == id);

            // var data = employees.SingleOrDefault(emp=>emp.Id == id);
            if (data == null)
            {
                return NotFound("Employee Not Found");
            }
            return Ok(data);

        }

        [HttpPost]
        public IActionResult Add(SaveEmployeeDto employeeDto)
        {
            var employee = new Employee()
            {
                // لا تعطي قيمة لل id لما بدك تبعثوا على DB
                 Id =0, //(employees.LastOrDefault()?.Id ?? 0) + 1,
                FirstName = employeeDto.FirstNmae,
                LastName = employeeDto.LastNmae,
                Position = employeeDto.Position,
                BirthDate = employeeDto.BirthDate,
                StartDate = employeeDto.StartDate,
                EndDate = employeeDto.EndDate,
                Email = employeeDto.Email,
                IsActive = employeeDto.IsActive,
                PhoneNumber = employeeDto.PhoneNumber,
                Salary = employeeDto.Salary,
                DepartmentId = employeeDto.DepartmentId,
                ManagerId = employeeDto.MangerId,

            };

            _dbContext.Employees.Add(employee);
            _dbContext.SaveChanges();
            return Ok(employee.Id);



        }
        [HttpPut("{id:long}")] //Resors Update
        public IActionResult UpDate([FromQuery]long id,[FromBody]SaveEmployeeDto employeeDto)
        {
            if (id != employeeDto.Id)
            {
                return BadRequest("Id Mismatch"); //404
            }
            var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == employeeDto.Id);
            if (employee == null)
            {
                return NotFound("Employee Dose Not Exist");
            }
            employee.FirstName = employeeDto.FirstNmae;
            employee.LastName = employeeDto.LastNmae;
            employee.PhoneNumber = employeeDto.PhoneNumber;
            employee.BirthDate = employeeDto.BirthDate;
            employee.StartDate = employeeDto.StartDate;
            employee.EndDate = employeeDto.EndDate;
            employee.Email = employeeDto.Email;
            employee.IsActive = employeeDto.IsActive;
            employee.Position = employeeDto.Position;
            employee.DepartmentId = employeeDto.DepartmentId;
            employee.ManagerId = employeeDto.MangerId;

            _dbContext.SaveChanges();

            return Ok();
        }
        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            var employee = _dbContext.Employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
            {
                return NotFound("Employee Dose Not Exist");
            }

            _dbContext.Employees.Remove(employee);
            _dbContext.SaveChanges();
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


