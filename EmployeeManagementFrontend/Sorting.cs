namespace EmployeeManagementFrontend
{
    public enum SortingOrder
    {
        Ascending, Descending
    }
    public class Sorting
    {
        public string? SortField { get; set; } = null;
        public SortingOrder SortOrder { get; set; } = SortingOrder.Ascending;
    }
}
