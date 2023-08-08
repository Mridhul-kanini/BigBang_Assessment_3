using System.ComponentModel.DataAnnotations;

namespace Travel.Model
{
    public class TravelAgent
    {
        [Key]
        public int TravelId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; } //Azurite bob
        public string? Password { get; set; }
        public string? Status { get; set; }

    }

    public class AgentCreateViewModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Description { get; set; }
        public IFormFile Image { get; set; } // This property is used for file upload in the controller, not in the entity model
        public string? Password { get; set; }
        public string? Status { get; set; }
    }
}
