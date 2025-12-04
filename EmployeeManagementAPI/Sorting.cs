using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementAPI
{
    public enum SortingOrder
    {
        Ascending, Descending
    }
    public class Sorting
    {
        [FromQuery] public string? SortField { get; set; } = null;
        [FromQuery] public SortingOrder SortOrder { get; set; } = SortingOrder.Ascending;
    }
}
