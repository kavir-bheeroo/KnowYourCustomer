using AutoMapper;
using KnowYourCustomer.Common;
using KnowYourCustomer.Common.Messaging.Interfaces;
using KnowYourCustomer.Common.Messaging.Kafka;
using KnowYourCustomer.Kyc.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Contracts.Models;
using KnowYourCustomer.Kyc.Data.Contracts.Entities;
using KnowYourCustomer.Kyc.Data.Contracts.Enumerations;
using KnowYourCustomer.Kyc.Data.Contracts.Interfaces;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Interfaces;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models;
using KnowYourCustomer.Kyc.Verifier.Contracts.Interfaces;
using KnowYourCustomer.Kyc.Verifier.Contracts.Models;
using System;
using System.Threading.Tasks;

namespace KnowYourCustomer.Kyc.Services
{
    public class KycService : IKycService
    {
        private readonly IKycRepository _kycRepository;

        private readonly IMrzProcessor _mrzProcessor;
        private readonly IVerifier _verifier;
        private readonly IMapper _mapper;

        private readonly IKafkaProducer<string, InitiateKycResponseModel> _initiateKycProducer;
        private readonly IKafkaProducer<string, CheckMrzStatusResponseModel> _checkMrzStatusProducer;
        private readonly IKafkaProducer<string, VerificationResponseModel> _verificationProducer;

        public KycService(
            IKycRepository kycRepository,
            IMrzProcessor mrzProcessor,
            IVerifier verifier,
            IMapper mapper,
            IKafkaProducer<string, InitiateKycResponseModel> initiateKycProducer,
            IKafkaProducer<string, CheckMrzStatusResponseModel> checkMrzStatusProducer,
            IKafkaProducer<string, VerificationResponseModel> verificationProducer)
        {
            _kycRepository = Guard.IsNotNull(kycRepository, nameof(kycRepository));

            _mrzProcessor = Guard.IsNotNull(mrzProcessor, nameof(mrzProcessor));
            _verifier = Guard.IsNotNull(verifier, nameof(verifier));
            _mapper = Guard.IsNotNull(mapper, nameof(mapper));

            _initiateKycProducer = Guard.IsNotNull(initiateKycProducer, nameof(initiateKycProducer));
            _checkMrzStatusProducer = Guard.IsNotNull(checkMrzStatusProducer, nameof(checkMrzStatusProducer));
            _verificationProducer = Guard.IsNotNull(verificationProducer, nameof(verificationProducer));
        }

        public async Task<InitiateKycResponseModel> InitiateKycAsync(InitiateKycRequestModel requestModel)
        {
            var kycEntity = await _kycRepository.AddAsync(new KycEntity(requestModel.UserId, KycStatus.Initiated));

            var mrzSubmitRequest = _mapper.Map<MrzSubmitRequest>(requestModel);
            var mrzSubmitResponse = _mrzProcessor.ProcessMrzFile(mrzSubmitRequest);

            kycEntity.Status = KycStatus.Queued;
            await _kycRepository.UpdateAsync(kycEntity);

            var responseModel = _mapper.Map<InitiateKycResponseModel>(requestModel);
            responseModel.KycId = kycEntity.Id;
            responseModel.MrzTaskId = mrzSubmitResponse.TaskId;

            // Send Kafka Message
            var kafkaMessage = new KafkaMessage<string, InitiateKycResponseModel>
            {
                Key = Guid.NewGuid().ToString(),
                Value = responseModel,
                MessageType = nameof(InitiateKycResponseModel)
            };

            await _initiateKycProducer.ProduceAsync(kafkaMessage);

            return responseModel;
        }

        public async Task<CheckMrzStatusResponseModel> CheckMrzTaskStatusAsync(CheckMrzStatusRequestModel requestModel)
        {
            var mrzStatusRequest = _mapper.Map<MrzStatusRequest>(requestModel);
            var mrzStatusResponse = _mrzProcessor.GetMrzTaskStatus(mrzStatusRequest);

            var entity = await _kycRepository.GetByKycIdAsync(requestModel.KycId);
            entity.Status = mrzStatusResponse.IsMrzCompleted ? KycStatus.MrzPassed : KycStatus.MrzFailed;
            await _kycRepository.UpdateAsync(entity);

            var responseModel = _mapper.Map<CheckMrzStatusResponseModel>(mrzStatusResponse);
            responseModel.UserId = requestModel.UserId;
            responseModel.KycId = requestModel.KycId;

            if (mrzStatusResponse.IsMrzCompleted)
            {
                // Send Kafka Message
                var kafkaMessage = new KafkaMessage<string, CheckMrzStatusResponseModel>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = responseModel,
                    MessageType = nameof(CheckMrzStatusResponseModel)
                };

                await _checkMrzStatusProducer.ProduceAsync(kafkaMessage);
            }

            return responseModel;
        }

        public async Task<VerificationResponseModel> VerifyIdentityAsync(VerificationRequestModel requestModel)
        {
            var identityVerificationRequest = _mapper.Map<IdentityVerificationRequest>(requestModel);
            var verified = await _verifier.VerifyAsync(identityVerificationRequest);

            var entity = await _kycRepository.GetByKycIdAsync(requestModel.KycId);
            entity.Status = verified ? KycStatus.VerificationPassed : KycStatus.VerificationFailed;
            await _kycRepository.UpdateAsync(entity);

            var responseModel = _mapper.Map<VerificationResponseModel>(identityVerificationRequest);
            responseModel.UserId = requestModel.UserId;
            responseModel.IsVerified = verified;

            // Send Kafka Message
            var kafkaMessage = new KafkaMessage<string, VerificationResponseModel>
            {
                Key = Guid.NewGuid().ToString(),
                Value = responseModel,
                MessageType = nameof(VerificationResponseModel)
            };

            await _verificationProducer.ProduceAsync(kafkaMessage);

            return responseModel;
        }
    }
}