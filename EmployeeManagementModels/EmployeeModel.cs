using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementModels
{
    public class EmployeeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = 0;

        [RegularExpression("^.{2,50}$")]
        public required string Name { get; set; }

        [RegularExpression("^.{2,50}$")]
        public required string Position { get; set; }

        public required int DepartmentId { get; set; }

        [Range(0, 10000)]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [ForeignKey("DepartmentId")]

        public DepartmentModel? Department { get; set; } = null;
    }
}
