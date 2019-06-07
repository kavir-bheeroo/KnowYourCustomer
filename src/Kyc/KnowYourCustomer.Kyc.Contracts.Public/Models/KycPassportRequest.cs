using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KnowYourCustomer.Kyc.Contracts.Public.Models
{
    public class KycPassportRequest
    {
        [FromForm]
        public IFormFile File { get; set; }
    }
}