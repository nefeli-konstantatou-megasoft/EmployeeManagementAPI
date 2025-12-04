using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementAPI
{
    public class Filter
    {
        [FromQuery] public string? FilterField { get; set; } = null;
        [FromQuery] public string? FilterBody { get; set; } = null;
    }
}
