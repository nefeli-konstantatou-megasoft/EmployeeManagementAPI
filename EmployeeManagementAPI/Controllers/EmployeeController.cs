using EmployeeManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return employees.Values;
        }

        [HttpGet("{id}")]
        public ActionResult<Employee> GetById(int id)
        {
            return employees.TryGetValue(id, out var employee) ? Ok(employee) : NotFound();
        }

        [HttpPost]
        public ActionResult<Employee> Add([FromBody] Employee value)
        {
            if((value.Salary <= 0) || value.Name.Equals("") || value.Position.Equals("") || value.Department.Equals(""))
                return BadRequest();

            value.Id = employees.Count() + 1;
            return employees.TryAdd(value.Id, value) ? Created() : BadRequest();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return employees.Remove(id) ? Ok() : NotFound();
        }


        [HttpPut]
        public ActionResult Update([FromBody] Employee value)
        {
            if (employees.ContainsKey(value.Id))
            {
                employees[value.Id] = value;
                return Ok(employees[value.Id]);
            }
            else return NotFound();
        }

        static private Dictionary<int, Models.Employee> employees = new();
    }
}
