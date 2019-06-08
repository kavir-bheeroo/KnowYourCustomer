using Microsoft.AspNetCore.Identity;
using System;

namespace KnowYourCustomer.Identity.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Mrz1 { get; set; }
        public string Mrz2 { get; set; }
        public string Number { get; set; }
        public DateTime DateOfExpiry { get; set; }
    }
}