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
        public ActionResult<ResponseModel<IEnumerable<EmployeeModel>>> GetAll()
        {
            return StatusCode(StatusCodes.Status200OK, new ResponseModel<IEnumerable<EmployeeModel>>() { 
                Success = true,
                Value = employees.Values
            });
        }

        [HttpGet("{id}")]
        public ActionResult<ResponseModel<EmployeeModel>> GetById(int id)
        {
            bool success = employees.TryGetValue(id, out var employee);
            if (success)
                return StatusCode(StatusCodes.Status200OK, new ResponseModel<EmployeeModel>()
                {
                    Success = true,
                    Value = employee
                });
            else
                return StatusCode(StatusCodes.Status404NotFound, new ResponseModel<EmployeeModel>()
                {
                    Success = false,
                    Message = $"Employee with specified id = {id} does not exist."
                });
        }

        [HttpPost]
        public ActionResult<ResponseModel<EmployeeModel>> Add([FromBody] EmployeeModel value)
        {
            if (value.Salary <= 0)
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel<EmployeeModel>()
                {
                    Success = false,
                    Message = "Specified salary has to be positive."
                });
            else if (value.Name.Equals(string.Empty))
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel<EmployeeModel>()
                {
                    Success = false,
                    Message = "Specified name cannot be empty."
                });
            else if (value.Position.Equals(string.Empty))
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel<EmployeeModel>()
                {
                    Success = false,
                    Message = "Specified position cannot be empty."
                });
            else if (value.Department.Equals(string.Empty))
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel<EmployeeModel>()
                {
                    Success = false,
                    Message = "Specified department cannot be empty."
                });

            value.Id = employees.Count() + 1;
            employees.Add(value.Id, value);

            return StatusCode(StatusCodes.Status201Created, new ResponseModel<EmployeeModel>()
            {
                Success = true,
                Value = value
            });
        }

        [HttpDelete("{id}")]
        public ActionResult<ResponseModel<bool>> Delete(int id)
        {
            bool success = employees.Remove(id);
            if (success)
                return StatusCode(StatusCodes.Status200OK, new ResponseModel<bool>()
                {
                    Success = true,
                    Value = true
                });
            else
                return StatusCode(StatusCodes.Status404NotFound, new ResponseModel<bool>()
                {
                    Success = false,
                    Message = $"Cannot remove employee with invalid id = {id}"
                });
        }


        [HttpPut]
        public ActionResult<ResponseModel<bool>> Update([FromBody] EmployeeModel value)
        {
            if (!employees.ContainsKey(value.Id))
                return StatusCode(StatusCodes.Status404NotFound, new ResponseModel<bool>()
                {
                    Success = false,
                    Message = $"Cannot update employee with invalid id = {value.Id}"
                });
            
            employees[value.Id] = value;
            return StatusCode(StatusCodes.Status200OK, new ResponseModel<bool>()
            {
                Success = true,
                Value = true
            });
        }

        static private Dictionary<int, Models.EmployeeModel> employees = new();
    }
}
