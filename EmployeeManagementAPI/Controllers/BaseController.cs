using EmployeeManagementModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        public ActionResult<ResponseModel<T>> MakeResponseSuccess<T>(int statusCode, T value)
        {
            return StatusCode(statusCode, new ResponseModel<T>()
            {
                Success = true,
                Value = value
            });
        }

        public ActionResult<ResponseModel<T>> MakeResponseFailure<T>(int statusCode, string message)
        {
            return StatusCode(statusCode, new ResponseModel<T>()
            {
                Success = false,
                Message = message
            });
        }
    }
}
