using System.ComponentModel.DataAnnotations;

namespace Travel.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Image { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }

    }

    public class UserCreateViewModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public IFormFile? Image { get; set; } // This property is used for file upload in the controller, not in the entity model
        public string? Password { get; set; }
        public string? Address { get; set; }
    }
}
