using AutoMapper;
using KnowYourCustomer.Kyc.Contracts.Models;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models;
using KnowYourCustomer.Kyc.Verifier.Contracts.Models;

namespace KnowYourCustomer.Kyc.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InitiateKycRequestModel, InitiateKycResponseModel>()
                .ForMember(d => d.KycId, o => o.Ignore())
                .ForMember(d => d.MrzTaskId, o => o.Ignore());

            CreateMap<InitiateKycRequestModel, MrzSubmitRequest>();
            CreateMap<CheckMrzStatusRequestModel, MrzStatusRequest>();

            CreateMap<MrzProcessor.Contracts.Models.UserInfo, UserInfoModel>();
            CreateMap<MrzProcessor.Contracts.Models.PassportInfo, PassportInfoModel>();
            CreateMap<MrzStatusResponse, CheckMrzStatusResponseModel>();

            CreateMap<VerificationRequestModel, IdentityVerificationRequest>();
            CreateMap<UserInfoModel, Verifier.Contracts.Models.UserInfo>().ReverseMap();
            CreateMap<PassportInfoModel, Verifier.Contracts.Models.PassportInfo>().ReverseMap();
            CreateMap<IdentityVerificationRequest, VerificationResponseModel>();
        }
    }
}