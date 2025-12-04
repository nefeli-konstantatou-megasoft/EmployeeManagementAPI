using EmployeeManagementModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        public EmployeeController(EmployeeManagementContext context) { this.dbContext = context; }
        #region GetAll
        /// <summary>
        /// [GET] /api/employees: get all employees, optionally filtering them by position or department,
        /// and optionally sorting them by name or salary (ascending/descending).Uses paging.
        /// </summary>
        /// <returns>Response containing a list of all employees in the database.</returns>
        [HttpGet]
        public ActionResult<ResponseModel<IEnumerable<EmployeeModel>>> GetAll(Filter filter, Sorting sorting, Paging paging)
        {
            var employees = dbContext.Employees.ToList();
            filter.FilterField ??= string.Empty;
            filter.FilterBody ??= string.Empty;
            int ascendingSign = sorting.SortOrder == SortingOrder.Ascending? 1 : -1;

            switch(sorting.SortField)
            {
            case "Salary":
                employees.Sort((first, second) => ascendingSign * (first.Salary > second.Salary ? -1 : 1));
                break;
            case "Name":
                employees.Sort((first, second) => ascendingSign * string.Compare(first.Name, second.Name));
                break;
            default:
                if(!sorting.SortField.IsNullOrEmpty())
                    return MakeActionResultFailure<IEnumerable<EmployeeModel>>(StatusCodes.Status400BadRequest, "Query parameter 'sortBy' has to be either 'Salary' or 'Name' if specified");
                break;
            }

            if (filter.FilterField.IsNullOrEmpty() != filter.FilterBody.IsNullOrEmpty())
                return MakeActionResultFailure<IEnumerable<EmployeeModel>>(StatusCodes.Status400BadRequest, "Both 'filter' and 'filterBy' query parameters have to be specified, or neither of them do");
            else switch (filter.FilterField)
            {
            case "Department":
                {
                    var filteredEmployees = employees.Where(employee => employee.Department.Name.Trim().ToLower().Equals(filter.FilterBody.Trim().ToLower()));
                    return MakeActionResultSuccess<IEnumerable<EmployeeModel>>(StatusCodes.Status200OK, filteredEmployees);
                }
            case "Position":
                {
                    var filteredEmployees = employees.Where(employee => employee.Position.Trim().ToLower().Equals(filter.FilterBody.Trim().ToLower()));
                    return MakeActionResultSuccess<IEnumerable<EmployeeModel>>(StatusCodes.Status200OK, filteredEmployees);
                }
            default:
                if (!filter.FilterField.IsNullOrEmpty())
                    return MakeActionResultFailure<IEnumerable<EmployeeModel>>(StatusCodes.Status400BadRequest, "Query parameter 'filterBy' has to be either 'Department' or 'Position' if specified");
                break;
            }

            int startIndex = paging.PageIndex * paging.PageSize;
            int count = Math.Min(paging.PageSize, employees.Count - startIndex);
            try
            {
                employees = employees.GetRange(startIndex, count);
                return MakeActionResultSuccess<IEnumerable<EmployeeModel>>(StatusCodes.Status200OK, employees);
            }
            catch (ArgumentOutOfRangeException)
            {
                return MakeActionResultSuccess<IEnumerable<EmployeeModel>>(StatusCodes.Status200OK, []);
            }
        }
        #endregion

        #region GetById
        /// <summary>
        /// [GET] /api/employees/{id}: get employee with specified id
        /// </summary>
        /// <param name="id">The id of the employee to retrieve</param>
        /// <returns>Response containing the target employee, or code 404 if the employee doesn't exist</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseModel<EmployeeModel>>> GetById(int id)
        {
            var employee = await dbContext.Employees.FirstOrDefaultAsync(employee => employee.Id == id);
            if (employee is not null)
                return MakeActionResultSuccess<EmployeeModel>(StatusCodes.Status200OK, employee);
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
        public async Task<ActionResult<ResponseModel<EmployeeModel>>> Add([FromBody] EmployeeModel value)
        {
            value.Id = 0;
            value.Department = null;
            if (value.Salary <= 0)
                return MakeActionResultFailure<EmployeeModel>(StatusCodes.Status400BadRequest, "Specified salary has to be positive.");
            else if (value.Name.Equals(string.Empty))
                return MakeActionResultFailure<EmployeeModel>(StatusCodes.Status400BadRequest, "Specified name cannot be empty.");
            else if (value.Position.Equals(string.Empty))
                return MakeActionResultFailure<EmployeeModel>(StatusCodes.Status400BadRequest, "Specified position cannot be empty.");

            var department = await dbContext.Departments.FirstOrDefaultAsync(department => department.Id == value.DepartmentId);
            
            if(department is null)
                return MakeActionResultFailure<EmployeeModel>(StatusCodes.Status400BadRequest, $"Specified department id = {value.DepartmentId} is invalid for add.");

            dbContext.Employees.Add(value);
            var result = await dbContext.SaveChangesAsync();
            
            return result >= 0? MakeActionResultSuccess(StatusCodes.Status200OK, value):
                MakeActionResultFailure<EmployeeModel>(StatusCodes.Status400BadRequest, "Could not add employee.");
        }
        #endregion

        #region Delete
        /// <summary>
        /// [DELETE]: /api/employees/{id}: remove an employee entry
        /// </summary>
        /// <param name="id">The id of the employee to remove</param>
        /// <returns>Response containing true if successful, or code 404 if employee with specified id was not found</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel<bool>>> Delete(int id)
        {
            var employee = dbContext.Employees.FirstOrDefault(employee => employee.Id == id);
            if(employee is null)
                return MakeActionResultFailure<bool>(StatusCodes.Status404NotFound, $"Cannot remove employee with invalid id = {id}");

            dbContext.Employees.Remove(employee);
            var result = await dbContext.SaveChangesAsync();

            if (result >= 0)
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
        public async Task<ActionResult<ResponseModel<bool>>> Update([FromBody] EmployeeModel value)
        {
            if (value.Salary <= 0)
                return MakeActionResultFailure<bool>(StatusCodes.Status400BadRequest, "Specified salary has to be positive.");
            else if (value.Name.Equals(string.Empty))
                return MakeActionResultFailure<bool>(StatusCodes.Status400BadRequest, "Specified name cannot be empty.");
            else if (value.Position.Equals(string.Empty))
                return MakeActionResultFailure<bool>(StatusCodes.Status400BadRequest, "Specified position cannot be empty.");

            var department = await dbContext.Departments.FirstOrDefaultAsync(department => department.Id == value.DepartmentId);

            if (department is null)
                return MakeActionResultFailure<bool>(StatusCodes.Status400BadRequest, $"Specified department id = {value.DepartmentId} is invalid for update.");

            var employee = await dbContext.Employees.FirstOrDefaultAsync(employee => employee.Equals(value));

            if (employee is null)
                return MakeActionResultFailure<bool>(StatusCodes.Status404NotFound, $"Cannot update employee with invalid id = {value.Id}");

            employee.DepartmentId = value.DepartmentId;
            employee.Name = value.Name;
            employee.Position = value.Position;
            employee.Department = department;
            employee.Salary = value.Salary;
            var result = await dbContext.SaveChangesAsync();
            
            if (result >= 0)
                return MakeActionResultSuccess(StatusCodes.Status200OK, true);
            else
                return MakeActionResultFailure<bool>(StatusCodes.Status404NotFound, $"Updating employee with id = {value.Id} failed");
            
        }
        #endregion

        private readonly EmployeeManagementContext dbContext;
    }
}
