using AutoMapper;
using KnowYourCustomer.Kyc.Contracts.Models;
using KnowYourCustomer.Kyc.Contracts.Public.Models;

namespace KnowYourCustomer.Kyc.Consumer.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserInfoModel, UserInfo>();
            CreateMap<PassportInfoModel, PassportInfo>();
            CreateMap<CheckMrzStatusResponseModel, VerificationRequest>();
        }
    }
}