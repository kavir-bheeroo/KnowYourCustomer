using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KnowYourCustomer.Common;
using KnowYourCustomer.Kyc.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Contracts.Models;
using KnowYourCustomer.Kyc.Contracts.Public.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        //// GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [Authorize]
        [HttpPost("{userId}")]
        public async Task Post([FromRoute] string userId, [FromForm] IFormFile file)
        {
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
        }
    }
}