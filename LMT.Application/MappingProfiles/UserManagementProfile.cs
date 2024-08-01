using AutoMapper;
using LMT.Application.DTOs;

namespace LMT.Application.MappingProfiles
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
