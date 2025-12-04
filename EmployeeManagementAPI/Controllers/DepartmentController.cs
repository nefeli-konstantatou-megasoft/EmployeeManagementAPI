using EmployeeManagementModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentController : BaseController
    {
        public DepartmentController(EmployeeManagementContext context) { this.dbContext = context; }

        #region GetAll
        /// <summary>
        /// [GET] /api/departments: get all departments
        /// </summary>
        /// <returns>Response containing a list of all employees in the dictionary</returns>
        [HttpGet]
        public ActionResult<ResponseModel<IEnumerable<DepartmentModel>>> GetAll()
        {
            return MakeResponseSuccess<IEnumerable<DepartmentModel>>(StatusCodes.Status200OK, dbContext.Departments);
        }
        #endregion

        #region GetById
        /// <summary>
        /// [GET] /api/departments/{id}: get department by id
        /// </summary>
        /// <param name="id">The id to use for the retrieval</param>
        /// <returns>Response containing the department if successful, or code 404 if not found</returns>
        [HttpGet("{id}")]
        public ActionResult<ResponseModel<DepartmentModel>> GetById(int id)
        {
            var result = dbContext.Departments.FirstOrDefault(department => department.Id == id);
            if (result == null)
                return MakeResponseFailure<DepartmentModel>(StatusCodes.Status404NotFound, $"Department with invalid id = {id} was not found.");
            else
                return MakeResponseSuccess<DepartmentModel>(StatusCodes.Status200OK, result!);
        }
        #endregion

        #region Add
        /// <summary>
        /// [POST] /api/departments: add new department entry
        /// </summary>
        /// <param name="department">The entry to add to the database</param>
        /// <returns>Response containing the department if successful, or code 404 if not found</returns>
        [HttpPost]
        public async Task<ActionResult<ResponseModel<DepartmentModel>>> Add(DepartmentModel department)
        {
            dbContext.Departments.Add(department);
            var result = await dbContext.SaveChangesAsync();
            if (result >= 0)
                return MakeResponseSuccess<DepartmentModel>(StatusCodes.Status201Created, department);
            else
                return MakeResponseFailure<DepartmentModel>(StatusCodes.Status404NotFound, $"Department with id = {department.Id} already exists.");
        }
        #endregion

        #region Update
        /// <summary>
        /// [PUT]: /api/departments: update a department entry
        /// </summary>
        /// <param name="department">The department to update</param>
        /// <returns>Response containing true if successful, code 404 if not found, or code 500 if a different error occured</returns>
        [HttpPut]
        public async Task<ActionResult<ResponseModel<bool>>> Update(DepartmentModel department)
        {
            var dbDepartment = await dbContext.Departments.FirstOrDefaultAsync(_department => _department.Id == department.Id);
            if (dbDepartment is null)
                return MakeResponseFailure<bool>(StatusCodes.Status404NotFound, $"Cannot update department with invalid id = {department.Id}");

            dbDepartment.Name = department.Name;
            var result = await dbContext.SaveChangesAsync();

            if (result >= 0)
                return MakeResponseSuccess<bool>(StatusCodes.Status200OK, true);
            else
                return MakeResponseFailure<bool>(StatusCodes.Status400BadRequest, $"Could not update department with id = {department.Id}.");
        }
        #endregion

        #region Delete
        /// <summary>
        /// [DELETE]: /api/departments: remove a department entry
        /// </summary>
        /// <param name="id">The department to remove</param>
        /// <returns>Response code containing true if successful, code 404 if not found, or code 400 if a different error occured</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel<bool>>> Delete(int id)
        {
            var dbDepartment = await dbContext.Departments.FirstOrDefaultAsync(department => department.Id == id);
            if (dbDepartment is null)
                return MakeResponseFailure<bool>(StatusCodes.Status404NotFound, $"Department with id = {id} does not exist.");

            dbContext.Departments.Remove(dbDepartment);
            var result = dbContext.SaveChanges();

            if (result >= 0)
                return MakeResponseSuccess<bool>(StatusCodes.Status200OK, true);
            else
                return MakeResponseFailure<bool>(StatusCodes.Status400BadRequest, $"Could not delete department with id = {id}");
        }
        #endregion

        private readonly EmployeeManagementContext dbContext;
    }
}
