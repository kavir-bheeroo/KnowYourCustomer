using AutoMapper;
using KnowYourCustomer.Common;
using KnowYourCustomer.Common.Extensions;
using KnowYourCustomer.Kyc.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Contracts.Models;
using KnowYourCustomer.Kyc.Contracts.Public.Models;
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
    [Authorize]
    public class KycController : ControllerBase
    {
        private readonly IKycService _kycService;
        private readonly IMapper _mapper;

        public KycController(IKycService kycService, IMapper mapper)
        {
            _kycService = Guard.IsNotNull(kycService, nameof(kycService));
            _mapper = Guard.IsNotNull(mapper, nameof(mapper));
        }

        [HttpPost("initiate")]
        public async Task<ActionResult<InitiateKycResponse>> Initiate([FromForm] IFormFile file)
        {
            var userId = HttpContext.User.GetUserId();

            if (userId == null)
            {
                throw new UnauthorizedAccessException("'sub' claim is missing in token.");
            }

            var kycFolderPath = Path.Combine(Environment.CurrentDirectory, "kyc-documents");
            Directory.CreateDirectory(kycFolderPath);
            var path = Path.Combine(kycFolderPath, file.FileName);

            using (var fileStream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(fileStream);
            }

            var model = new InitiateKycRequestModel
            {
                UserId = userId.Value,
                FileName = file.FileName,
                FilePath = path
            };

            var result = await _kycService.InitiateKycAsync(model);
            var response = _mapper.Map<InitiateKycResponse>(result);

            return Ok(response);
        }

        [HttpPost("checkmrzstatus")]
        public async Task<ActionResult<CheckMrzStatusResponse>> CheckMrzStatus(CheckMrzStatusRequest request)
        {
            var model = _mapper.Map<CheckMrzStatusRequestModel>(request);
            var result = await _kycService.CheckMrzTaskStatusAsync(model);
            var response = _mapper.Map<CheckMrzStatusResponse>(result);

            return Ok(response);
        }

        [HttpPost("verifyidentity")]
        public async Task<ActionResult<CheckMrzStatusResponse>> VerifyIdentity(VerificationRequest request)
        {
            var model = _mapper.Map<VerificationRequestModel>(request);
            var result = await _kycService.VerifyIdentityAsync(model);
            var response = _mapper.Map<VerificationResponse>(result);

            return Ok(response);
        }
    }
}