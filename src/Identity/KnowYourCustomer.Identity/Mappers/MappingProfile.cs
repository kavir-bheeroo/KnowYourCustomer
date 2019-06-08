using AutoMapper;
using KnowYourCustomer.Identity.Data.Entities;
using KnowYourCustomer.Identity.Models;

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