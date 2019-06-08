using AutoMapper;
using KnowYourCustomer.Identity.Contracts.Models;
using KnowYourCustomer.Identity.Data.Entities;

namespace KnowYourCustomer.Identity.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UpdateModel, ApplicationUser>();
        }
    }
}