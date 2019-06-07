using AutoMapper;
using KnowYourCustomer.Common;
using KnowYourCustomer.Kyc.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Contracts.Models;
using KnowYourCustomer.Kyc.Data.Contracts.Entities;
using KnowYourCustomer.Kyc.Data.Contracts.Enumerations;
using KnowYourCustomer.Kyc.Data.Contracts.Interfaces;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Interfaces;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models;
using KnowYourCustomer.Kyc.Verifier.Contracts.Interfaces;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Services
{
    public class KycService : IKycService
    {
        private readonly IKycRepository _kycRepository;
        private readonly IKycDocumentRepository _kycDocumentRepository;
        private readonly IKycOperationRepository _kycOperationRepository;

        private readonly IMrzProcessor _mrzProcessor;
        private readonly IVerifier _verifier;
        private readonly IMapper _mapper;

        public KycService(
            IKycRepository kycRepository,
            IKycDocumentRepository kycDocumentRepository,
            IKycOperationRepository kycOperationRepository,
            IMrzProcessor mrzProcessor,
            IVerifier verifier,
            IMapper mapper)
        {
            _kycRepository = Guard.IsNotNull(kycRepository, nameof(kycRepository));
            _kycDocumentRepository = Guard.IsNotNull(kycDocumentRepository, nameof(kycDocumentRepository));
            _kycOperationRepository = Guard.IsNotNull(kycOperationRepository, nameof(kycOperationRepository));

            _mrzProcessor = Guard.IsNotNull(mrzProcessor, nameof(mrzProcessor));
            _verifier = Guard.IsNotNull(verifier, nameof(verifier));
            _mapper = Guard.IsNotNull(mapper, nameof(mapper));
        }

        public async Task<InitiateKycResponseModel> InitiateKyc(InitiateKycRequestModel requestModel)
        {
            var kycEntity = await _kycRepository.AddAsync(new KycEntity(requestModel.UserId, KycStatus.Initiated));

            var mrzSubmitRequest = _mapper.Map<MrzSubmitRequest>(requestModel);
            var mrzSubmitResponse = _mrzProcessor.ProcessMrzFile(mrzSubmitRequest);

            kycEntity.Status = KycStatus.Queued;
            await _kycRepository.UpdateAsync(kycEntity);

            var responseModel = _mapper.Map<InitiateKycResponseModel>(requestModel);
            responseModel.KycId = kycEntity.Id;
            responseModel.MrzTaskId = mrzSubmitResponse.TaskId;

            return responseModel;
            //var identityVerificationRequest = new IdentityVerificationRequest
            //{
            //    UserInfo = _mapper.Map<UserInfo>(mrzSubmitResponse.UserInfo),
            //    PassportInfo = _mapper.Map<PassportInfo>(mrzSubmitResponse.PassportInfo)
            //};

            //var verified = await _verifier.VerifyAsync(identityVerificationRequest);
            //int x = 1;
            //
        }

        public async Task<CheckMrzStatusResponseModel> CheckMrzTaskStatus(CheckMrzStatusRequestModel requestModel)
        {
            var mrzStatusRequest = _mapper.Map<MrzStatusRequest>(requestModel);
            var mrzStatusResponse = _mrzProcessor.GetMrzTaskStatus(mrzStatusRequest);

            var entity = await _kycRepository.GetByKycIdAsync(requestModel.KycId);
            entity.Status = mrzStatusResponse.IsMrzCompleted ? KycStatus.MrzPassed : KycStatus.MrzFailed;
            await _kycRepository.UpdateAsync(entity);

            var responseModel = _mapper.Map<CheckMrzStatusResponseModel>(mrzStatusResponse);
            return responseModel;
        }
    }
}