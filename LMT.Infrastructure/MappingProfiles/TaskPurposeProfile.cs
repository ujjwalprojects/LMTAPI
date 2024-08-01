using AutoMapper;
using LMT.Application.DTOs;
using LMT.Domain.Entities;

namespace LMT.Infrastructure.MappingProfiles
{
    public class TaskPurposeProfile : Profile
    {
        public TaskPurposeProfile()
        {
            CreateMap<M_TaskPurposes, M_TaskPurposesDTO>().ReverseMap();
        }
    }
}
