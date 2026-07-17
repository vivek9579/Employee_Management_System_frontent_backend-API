using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Frontend.Models
{
    public class EmployeeDTO
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Salary { get; set; }
        public bool IsActive { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        [JsonIgnore]
       public List<DepartmentDTO>? Departments { get; set; }
    }
}
