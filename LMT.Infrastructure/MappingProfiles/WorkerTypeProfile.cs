using AutoMapper;
using LMT.Application.DTOs;
using LMT.Domain.Entities;

namespace LMT.Infrastructure.MappingProfiles
{
    public class WorkerTypeProfile : Profile
    {
        public WorkerTypeProfile()
        {
            CreateMap<M_WorkerTypes, M_WorkerTypesDTO>().ReverseMap();
        }
    }
}
