using KnowYourCustomer.Common;
using KnowYourCustomer.Common.Extensions;
using KnowYourCustomer.Kyc.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KycController : ControllerBase
    {
        private readonly IKycService _kycService;

        public KycController(IKycService kycService)
        {
            _kycService = Guard.IsNotNull(kycService, nameof(kycService));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile file)
        {
            var userId = HttpContext.User.GetUserId();

            if (userId == null)
            {
                throw new UnauthorizedAccessException("'sub' claim is missing in token.");
            }

            var kycFolderPath = Path.Combine(Environment.CurrentDirectory, "kyc-files");
            Directory.CreateDirectory(kycFolderPath);
            var path = Path.Combine(kycFolderPath, file.FileName);

            using (var fileStream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(fileStream);
            }

            var model = new KycFile
            {
                UserId = userId,
                FileName = file.FileName,
                FilePath = path
            };

            await _kycService.ProcessPassport(model);
            return Ok();
        }
    }
}