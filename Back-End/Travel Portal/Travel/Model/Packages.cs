using System.ComponentModel.DataAnnotations;

namespace Travel.Model
{
    public class Packages
    {
        [Key]
        public int PackageId { get; set; }
        public int TravelId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Type { get; set; }
        public string? ImageUrl { get; set; } // Azurite bob
        public string? Price { get; set; }   

    }

    public class PackageCreateViewModel
    {
        public int TravelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public IFormFile Image { get; set; } // This property is used for file upload in the controller, not in the entity model
        public string? Date { get; set; }
        public string Price { get; set; }
        public string? TotalPeople { get; set; }
    }


}
