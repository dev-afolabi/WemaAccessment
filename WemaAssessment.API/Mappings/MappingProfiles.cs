using AutoMapper;
using WemaAssessment.Application.DTOs;
using WemaAssessment.Domain.Models;

namespace WemaAssessment.API.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.IsCustomerConfirmed, src => src.MapFrom(p=>p.PhoneNumberConfirmed))
                .ForMember(dest => dest.State, src => src.MapFrom(p => p.State))
                .ForMember(dest => dest.LGA, src => src.MapFrom(p => p.LGA));

            CreateMap<AddCustomerDto, Customer>();
        }
    }
}
