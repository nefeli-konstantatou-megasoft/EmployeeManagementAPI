namespace EmployeeManagementAPI.Models
{
    public class ResponseModel<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Value { get; set; }
    }
}
