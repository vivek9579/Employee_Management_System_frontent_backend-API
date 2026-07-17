using Domain.Entity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.DTOs
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
