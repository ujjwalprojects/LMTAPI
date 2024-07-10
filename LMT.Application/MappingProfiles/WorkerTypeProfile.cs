using AutoMapper;
using LMT.Application.DTOs;
using LMT.Domain.Entities;

namespace LMT.Application.MappingProfiles
{
    public class WorkerTypeProfile : Profile
    {
        public WorkerTypeProfile()
        {
            CreateMap<M_WorkerTypes, M_WorkerTypesDTO>().ReverseMap();
        }
    }
}
