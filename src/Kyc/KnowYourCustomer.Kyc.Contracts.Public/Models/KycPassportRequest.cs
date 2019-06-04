using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KnowYourCustomer.Kyc.Contracts.Public.Models
{
    public class KycPassportRequest
    {
        public string UserId { get; set; }

        [FromForm]
        public IFormFile File { get; set; }
    }
}