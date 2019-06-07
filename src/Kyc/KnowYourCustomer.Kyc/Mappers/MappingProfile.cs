using AutoMapper;
using KnowYourCustomer.Kyc.Contracts.Models;
using KnowYourCustomer.Kyc.MrzProcessor.Contracts.Models;

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
            CreateMap<MrzStatusResponse, CheckMrzStatusResponseModel>();
        }
    }
}