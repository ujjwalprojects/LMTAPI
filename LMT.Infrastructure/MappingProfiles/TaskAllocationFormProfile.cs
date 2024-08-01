using AutoMapper;
using LMT.Application.DTOs;
using LMT.Domain.Entities;

namespace LMT.Infrastructure.MappingProfiles
{
    public class TaskAllocationFormProfile : Profile
    {
        public TaskAllocationFormProfile()
        {
            CreateMap<T_TaskAllocationForms, T_TaskAllocationFormsDTO>().ReverseMap();
        }
    }
}
