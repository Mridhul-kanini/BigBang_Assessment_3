using System.ComponentModel.DataAnnotations;

namespace Travel.Model
{
    public class Bookingdetails
    {
        [Key]
        public int BookingId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        
    }
}
