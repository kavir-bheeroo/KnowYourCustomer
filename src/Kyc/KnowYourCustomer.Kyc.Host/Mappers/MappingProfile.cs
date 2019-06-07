using AutoMapper;
using KnowYourCustomer.Kyc.Contracts.Models;
using KnowYourCustomer.Kyc.Contracts.Public.Models;

namespace KnowYourCustomer.Kyc.Host.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InitiateKycResponseModel, InitiateKycResponse>();
            CreateMap<CheckMrzStatusRequest, CheckMrzStatusRequestModel>();
            CreateMap<CheckMrzStatusResponseModel, CheckMrzStatusResponse>();

            CreateMap<UserInfoModel, UserInfo>().ReverseMap();
            CreateMap<PassportInfoModel, PassportInfo>().ReverseMap();

            CreateMap<VerificationRequest, VerificationRequestModel>();
            CreateMap<VerificationResponseModel, VerificationResponse>();
        }
    }
}