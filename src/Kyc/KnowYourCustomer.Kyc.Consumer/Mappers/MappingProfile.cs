using AutoMapper;
using KnowYourCustomer.Identity.Contracts.Models;
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

            CreateMap<VerificationResponseModel, UpdateModel>()
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.UserInfo.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.UserInfo.LastName))
                .ForMember(d => d.DateOfBirth, o => o.MapFrom(s => s.UserInfo.DateOfBirth))
                .ForMember(d => d.Gender, o => o.MapFrom(s => s.UserInfo.Gender))
                .ForMember(d => d.Nationality, o => o.MapFrom(s => s.UserInfo.Nationality))
                .ForMember(d => d.Mrz1, o => o.MapFrom(s => s.PassportInfo.Mrz1))
                .ForMember(d => d.Mrz2, o => o.MapFrom(s => s.PassportInfo.Mrz2))
                .ForMember(d => d.Number, o => o.MapFrom(s => s.PassportInfo.Number))
                .ForMember(d => d.DateOfExpiry, o => o.MapFrom(s => s.PassportInfo.DateOfExpiry));
        }
    }
}