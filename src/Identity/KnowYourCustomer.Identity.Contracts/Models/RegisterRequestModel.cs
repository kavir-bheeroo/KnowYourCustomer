using System.ComponentModel.DataAnnotations;

namespace KnowYourCustomer.Identity.Contracts.Models
{
    public class RegisterRequestModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}