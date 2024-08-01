using AutoMapper;
using LMT.Application.DTOs;
using LMT.Domain.Entities;

namespace LMT.Infrastructure.MappingProfiles
{
    public class RegistrationActProfile : Profile
    {
        public RegistrationActProfile()
        {
            CreateMap<M_RegistrationActs, M_RegistrationActsDTO>().ReverseMap();
        }
    }
}
