using AutoMapper;
using LMT.Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace LMT.Infrastructure.MappingProfiles
{
    public class UserManagementProfile:Profile
    {
        public UserManagementProfile() {
            CreateMap<IdentityUser, UserListDTO>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
