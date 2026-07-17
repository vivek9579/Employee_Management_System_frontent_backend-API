using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Employee_View.Models
{
    public class DepartmentDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateOnly? CreateDate { get; set; }
        [JsonIgnore]
        public List<EmployeeDTO>? Employees { get; set; }
    }
}
