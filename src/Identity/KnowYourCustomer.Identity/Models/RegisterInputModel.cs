using System.ComponentModel.DataAnnotations;

namespace KnowYourCustomer.Identity.Models
{
    public class RegisterInputModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}