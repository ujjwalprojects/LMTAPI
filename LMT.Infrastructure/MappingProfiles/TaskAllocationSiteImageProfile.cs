﻿using AutoMapper;
using LMT.Application.DTOs;
using LMT.Domain.Entities;

namespace LMT.Infrastructure.MappingProfiles
{
    public class TaskAllocationSiteImageProfile : Profile
    {
        public TaskAllocationSiteImageProfile()
        {
            CreateMap<T_TaskAllocationSiteImages, T_TaskAllocationSiteImagesDTO>().ReverseMap();
        }
    }
}