using System.ComponentModel.DataAnnotations;

namespace Travel.Model
{
    public class Images
    {
        [Key]
        public int ImgId { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? ImageUrl { get; set; } //Azurite bob

    }

    public class ImagesCreateViewModel
    {
        public string? ImgId { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public IFormFile? Image { get; set; }
    }
}
