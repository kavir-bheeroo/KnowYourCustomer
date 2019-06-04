using AutoMapper;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models;

namespace KnowYourCustomer.Kyc.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserInfo, Verifier.Contracts.Models.UserInfo>();
            CreateMap<PassportInfo, Verifier.Contracts.Models.PassportInfo>();
        }
    }
}