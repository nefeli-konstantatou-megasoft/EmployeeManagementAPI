using EmployeeManagementModels;
using Microsoft.AspNetCore.Mvc;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        #region GetAll
        /// <summary>
        /// [GET] /api/employees: get all employees.
        /// </summary>
        /// <returns>Response containing a list of all employees in the dictionary.</returns>
        [HttpGet]
        public ActionResult<ResponseModel<IEnumerable<EmployeeModel>>> GetAll()
        {
            return MakeActionResultSuccess<IEnumerable<EmployeeModel>>(StatusCodes.Status200OK, employees.Values);
        }
        #endregion

        #region GetById
        /// <summary>
        /// [GET] /api/employees/{id}: get employee with specified id
        /// </summary>
        /// <param name="id">The id of the employee to retrieve</param>
        /// <returns>Response containing the target employee, or code 404 if the employee doesn't exist</returns>
        [HttpGet("{id}")]
        public ActionResult<ResponseModel<EmployeeModel>> GetById(int id)
        {
            bool success = employees.TryGetValue(id, out var employee);
            if (success)
                return MakeActionResultSuccess<EmployeeModel>(StatusCodes.Status200OK, employee!);
            else
                return MakeActionResultFailure<EmployeeModel>(StatusCodes.Status404NotFound, $"Employee with specified id = {id} does not exist");
        }
        #endregion

        #region Add
        /// <summary>
        /// [POST] /api/employees: add employe
        /// </summary>
        /// <param name="value">Employee to add</param>
        /// <returns>Response containing the added employee if successful or code 400 if the request was invalid</returns>
        [HttpPost]
        public ActionResult<ResponseModel<EmployeeModel>> Add([FromBody] EmployeeModel value)
        {
            if (value.Salary <= 0)
                return MakeActionResultFailure<EmployeeModel>(StatusCodes.Status400BadRequest, "Specified salary has to be positive.");
            else if (value.Name.Equals(string.Empty))
                return MakeActionResultFailure<EmployeeModel>(StatusCodes.Status400BadRequest, "Specified name cannot be empty.");
            else if (value.Position.Equals(string.Empty))
                return MakeActionResultFailure<EmployeeModel>(StatusCodes.Status400BadRequest, "Specified position cannot be empty.");
            else if (value.Department.Equals(string.Empty))
                return MakeActionResultFailure<EmployeeModel>(StatusCodes.Status400BadRequest, "Specified department cannot be empty.");

            value.Id = employees.Count() + 1;
            employees.Add(value.Id, value);

            return MakeActionResultSuccess(StatusCodes.Status200OK, value);
        }
        #endregion

        #region Delete
        /// <summary>
        /// [DELETE]: /api/employees/{id}: remove an employee entry
        /// </summary>
        /// <param name="id">The id of the employee to remove</param>
        /// <returns>Response containing true if successful, or code 404 if employee with specified id was not found</returns>
        [HttpDelete("{id}")]
        public ActionResult<ResponseModel<bool>> Delete(int id)
        {
            bool success = employees.Remove(id);
            if (success)
                return MakeActionResultSuccess(StatusCodes.Status200OK, true);
            else
                return MakeActionResultFailure<bool>(StatusCodes.Status404NotFound, $"Cannot remove employee with invalid id = {id}");
        }
        #endregion

        #region Update
        /// <summary>
        /// [PUT]: /api/employees: update the fields of an employee entry
        /// </summary>
        /// <param name="value">The updated employee entry, replacing the entry with the same id as the given entry</param>
        /// <returns>Response containing true if successful, or code 404 if employee with specified id was not found</returns>
        [HttpPut]
        public ActionResult<ResponseModel<bool>> Update([FromBody] EmployeeModel value)
        {
            if (value.Salary <= 0)
                return MakeActionResultFailure<bool>(StatusCodes.Status400BadRequest, "Specified salary has to be positive.");
            else if (value.Name.Equals(string.Empty))
                return MakeActionResultFailure<bool>(StatusCodes.Status400BadRequest, "Specified name cannot be empty.");
            else if (value.Position.Equals(string.Empty))
                return MakeActionResultFailure<bool>(StatusCodes.Status400BadRequest, "Specified position cannot be empty.");
            else if (value.Department.Equals(string.Empty))
                return MakeActionResultFailure<bool>(StatusCodes.Status400BadRequest, "Specified department cannot be empty.");
            if (!employees.ContainsKey(value.Id))
                return MakeActionResultFailure<bool>(StatusCodes.Status404NotFound, $"Cannot update employee with invalid id = {value.Id}");
            
            employees[value.Id] = value;
            return MakeActionResultSuccess(StatusCodes.Status200OK, true);
        }
        #endregion

        static private Dictionary<int, EmployeeModel> employees = new();
    }
}
