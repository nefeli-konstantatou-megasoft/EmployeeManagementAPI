namespace EmployeeManagementModels
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Position { get; set; }
        public required string Department { get; set; }
        public decimal Salary { get; set; }
    }
}
