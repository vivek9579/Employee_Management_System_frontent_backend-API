using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Frontend.Models
{
    public class DepartmentDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateOnly CreatedDate { get; set; }

        [JsonIgnore]
        public List<EmployeeDTO>? employees { get; set; }
    }
}
