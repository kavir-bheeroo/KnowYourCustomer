using AutoMapper;
using Bogus;
using FluentAssertions;
using KnowYourCustomer.Common.Messaging.Interfaces;
using KnowYourCustomer.Common.Messaging.Kafka;
using KnowYourCustomer.Kyc.Contracts.Models;
using KnowYourCustomer.Kyc.Data.Contracts.Entities;
using KnowYourCustomer.Kyc.Data.Contracts.Enumerations;
using KnowYourCustomer.Kyc.Data.Contracts.Interfaces;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Interfaces;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models;
using KnowYourCustomer.Kyc.Services;
using KnowYourCustomer.Kyc.Verifier.Contracts.Interfaces;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace KnowYourCustomer.Kyc.UnitTests
{
    public class PlayerServiceTests
    {
        private readonly Mock<IKycRepository> _kycRepositoryMock;
        private readonly Mock<IMrzProcessor> _mrzProcessorMock;
        private readonly Mock<IVerifier> _verifierMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IKafkaProducer<string, InitiateKycResponseModel>> _initiateKycProducerMock;
        private readonly Mock<IKafkaProducer<string, CheckMrzStatusResponseModel>> _checkMrzStatusProducerMock;
        private readonly Mock<IKafkaProducer<string, VerificationResponseModel>> _verificationMock;

        private readonly KycService _sut;

        public PlayerServiceTests()
        {
            _kycRepositoryMock = new Mock<IKycRepository>();
            _mrzProcessorMock = new Mock<IMrzProcessor>();
            _verifierMock = new Mock<IVerifier>();
            _mapperMock = new Mock<IMapper>();
            _initiateKycProducerMock = new Mock<IKafkaProducer<string, InitiateKycResponseModel>>();
            _checkMrzStatusProducerMock = new Mock<IKafkaProducer<string, CheckMrzStatusResponseModel>>();
            _verificationMock = new Mock<IKafkaProducer<string, VerificationResponseModel>>();

            _sut = new KycService(
                _kycRepositoryMock.Object,
                _mrzProcessorMock.Object,
                _verifierMock.Object,
                _mapperMock.Object,
                _initiateKycProducerMock.Object,
                _checkMrzStatusProducerMock.Object,
                _verificationMock.Object);
        }

        [Fact]
        public async Task InitiateKyc_ShouldReturnResponseModel()
        {
            // Arrange
            var requestModel = new Faker<InitiateKycRequestModel>()
                .RuleFor(x => x.UserId, f => Guid.NewGuid())
                .Generate();
            var responseModel = new Faker<InitiateKycResponseModel>().Generate();
            var submitRequest = new Faker<MrzSubmitRequest>().Generate();
            var submitResponse = new Faker<MrzSubmitResponse>().Generate();
            var entity = new Faker<KycEntity>()
                .RuleFor(x => x.Id, f => requestModel.UserId)
                .RuleFor(x => x.Status, f => KycStatus.Initiated)
                .Generate();

            _kycRepositoryMock.Setup(x => x.AddAsync(It.IsAny<KycEntity>())).ReturnsAsync(entity);
            _mapperMock.Setup(x => x.Map<MrzSubmitRequest>(requestModel)).Returns(submitRequest);
            _mrzProcessorMock.Setup(x => x.ProcessMrzFile(submitRequest)).Returns(submitResponse);
            _kycRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<KycEntity>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(x => x.Map<InitiateKycResponseModel>(requestModel)).Returns(responseModel);
            _initiateKycProducerMock.Setup(x => x.ProduceAsync(It.IsAny<KafkaMessage<string, InitiateKycResponseModel>>())).Returns(Task.CompletedTask);

            // Act
            var response = await _sut.InitiateKycAsync(requestModel);

            // Assert
            response.Should().NotBeNull();

            _kycRepositoryMock.Verify(x => x.AddAsync(It.IsAny<KycEntity>()), Times.Once);
            _mapperMock.Verify(x => x.Map<MrzSubmitRequest>(requestModel), Times.Once);
            _mrzProcessorMock.Verify(x => x.ProcessMrzFile(submitRequest), Times.Once);
            _kycRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<KycEntity>()), Times.Once);
            _mapperMock.Verify(x => x.Map<InitiateKycResponseModel>(It.IsAny<InitiateKycRequestModel>()), Times.Once);
            _initiateKycProducerMock.Verify(x => x.ProduceAsync(It.IsAny<KafkaMessage<string, InitiateKycResponseModel>>()), Times.Once);
        }
    }
}