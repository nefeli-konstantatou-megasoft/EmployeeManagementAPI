using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementAPI.Models
{
    public class ResponseModel<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Value { get; set; }
        public static ActionResult<ResponseModel<T>> MakeActionResultSuccess(ControllerBase controller, int statusCode, T value)
        {
            return controller.StatusCode(statusCode, new ResponseModel<T>()
            {
                Success = true,
                Value = value
            });
        }

        public static ActionResult<ResponseModel<T>> MakeActionResultFailure(ControllerBase controller, int statusCode, string message)
        {
            return controller.StatusCode(statusCode, new ResponseModel<T>()
            {
                Success = false,
                Message = message
            });
        }
    }
}
