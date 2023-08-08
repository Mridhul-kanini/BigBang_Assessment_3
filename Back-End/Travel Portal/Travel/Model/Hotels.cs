using System.ComponentModel.DataAnnotations;

namespace Travel.Model
{
    public class Hotels
    {
        [Key]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Travel ID is required")]
        public int TravelId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid price format")]
        public string? Price { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string? ImageUrl { get; set; } // Azurite Blob
    }
}
