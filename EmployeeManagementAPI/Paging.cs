using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementAPI
{
    public class Paging
    {
        [FromQuery] public int PageSize { get; set; } = 100;
        [FromQuery] public int PageIndex { get; set; } = 0;
    }
}
