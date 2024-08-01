using AutoMapper;
using LMT.Application.DTOs;
using LMT.Domain.Entities;

namespace LMT.Infrastructure.MappingProfiles
{
    public class BlockMunicipalProfile:Profile
    {
        public BlockMunicipalProfile()
        {
            CreateMap<M_BlockMunicipals, M_BlockMunicipalsDTO>().ReverseMap();
        }
    }
}
