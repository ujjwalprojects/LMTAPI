using AutoMapper;
using LMT.Application.DTOs;
using LMT.Domain.Entities;

namespace LMT.Infrastructure.MappingProfiles
{
    public class JobRoleProfile:Profile
    {
        public JobRoleProfile()
        {
            CreateMap<M_JobRoles, M_JobRolesDTO>().ReverseMap();
        }
    }
}
