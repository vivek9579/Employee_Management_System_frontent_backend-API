using System.ComponentModel.DataAnnotations;

namespace Employee_View.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage ="Please Select Role")]
        public string Role { get; set; }
    }
}
