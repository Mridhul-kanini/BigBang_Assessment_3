using System.ComponentModel.DataAnnotations;

namespace Travel.Model
{
    public class Carddetails
    {
        [Key]
        public int CardId { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Card number is required")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Invalid card number format")]
        public string? Cardnumber { get; set; }

        [Required(ErrorMessage = "Expiry date is required")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$", ErrorMessage = "Invalid expiry date format (MM/YY or MM/YYYY)")]
        public string? Expirydate { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [Range(100, 9999, ErrorMessage = "CVV must be between 100 and 9999")]
        public int? Cvv { get; set; }
    }
}
