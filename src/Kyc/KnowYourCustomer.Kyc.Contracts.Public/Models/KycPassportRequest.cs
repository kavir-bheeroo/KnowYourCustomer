using Microsoft.AspNetCore.Http;

namespace KnowYourCustomer.Kyc.Contracts.Public.Models
{
    public class KycPassportRequest
    {
        public string UserId { get; set; }
        public IFormFile File { get; set; }
    }
}