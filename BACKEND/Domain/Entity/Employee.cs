using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int Salary { get; set; }
        public bool IsActive { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
