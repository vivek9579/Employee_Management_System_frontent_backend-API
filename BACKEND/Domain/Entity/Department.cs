using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateOnly? CreateDate { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}
