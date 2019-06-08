using System;

namespace KnowYourCustomer.Identity.Contracts.Models
{
    public class RegisterResponseModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
    }
}