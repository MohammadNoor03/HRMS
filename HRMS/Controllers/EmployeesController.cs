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
        public List<Employee> employees = new List<Employee>()
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

        [HttpGet("GetByCriteria")]// بترجع الموظفين بناءا على position
        public IActionResult GetByCriteria(string? position)

        {
            //
            var data = from emp in employees
                       where (position ==null|| emp.Position == position)
                       orderby emp.Id descending
                       select new EmployeeDto
                       {
                           Id = emp.Id,
                           Name = emp.FirstNmae + " " + emp.LastNmae,
                           Position = emp.Position,
                           BirthDate = emp.BirthDate,
                           StartDate = emp.StartDate,
                           EndDate = emp.EndDate
                           //بحدد فق المعلومات الي بدب ارجعها لان بعض المعلومات حساسة صعب ارجعها
                       };
            
              return Ok(data);
        }




        [HttpGet("GetById")]
        public IActionResult GetById(long id)
        {
            var data = employees.Select(emp=> new EmployeeDto
            {
                Id = emp.Id,
                Name = emp.FirstNmae + " " + emp.LastNmae,
                Position = emp.Position,
                BirthDate = emp.BirthDate,
                StartDate = emp.StartDate,
                EndDate = emp.EndDate

            } ) .FirstOrDefault(emp => emp.Id == id);


            if(data == null)
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
                Id = (employees.LastOrDefault()?.Id ?? 0)+1,
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


