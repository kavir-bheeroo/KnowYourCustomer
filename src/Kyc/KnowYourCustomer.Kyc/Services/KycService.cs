using AutoMapper;
using KnowYourCustomer.Common;
using KnowYourCustomer.Kyc.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Contracts.Models;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Interfaces;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models;
using KnowYourCustomer.Kyc.Verifier.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Verifier.Contracts.Models;
using System.Threading.Tasks;
using PassportInfo = KnowYourCustomer.Kyc.Verifier.Contracts.Models.PassportInfo;
using UserInfo = KnowYourCustomer.Kyc.Verifier.Contracts.Models.UserInfo;

namespace KnowYourCustomer.Kyc.Services
{
    public class KycService : IKycService
    {
        private readonly IMrzProcessor _mrzProcessor;
        private readonly IVerifier _verifier;
        private readonly IMapper _mapper;

        public KycService(IMrzProcessor mrzProcessor, IVerifier verifier, IMapper mapper)
        {
            _mrzProcessor = Guard.IsNotNull(mrzProcessor, nameof(mrzProcessor));
            _verifier = Guard.IsNotNull(verifier, nameof(verifier));
            _mapper = Guard.IsNotNull(mapper, nameof(mapper));
        }

        public async Task ProcessPassport(KycFile model)
        {
            var mrzRequest = new MrzProcessRequest { FileName = model.FileName, FilePath = model.FilePath };
            var mrzResponse = _mrzProcessor.ProcessMrzFile(mrzRequest);

            var identityVerificationRequest = new IdentityVerificationRequest
            {
                UserInfo = _mapper.Map<UserInfo>(mrzResponse.UserInfo),
                PassportInfo = _mapper.Map<PassportInfo>(mrzResponse.PassportInfo)
            };

            var verified = await _verifier.VerifyAsync(identityVerificationRequest);
            int x = 1;
            //
        }
    }
}