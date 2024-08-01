﻿using AutoMapper;
using LMT.Application.DTOs;
using LMT.Domain.Entities;

namespace LMT.Infrastructure.MappingProfiles
{
    public class DistrictProfile : Profile
    {
        public DistrictProfile()
        {
            CreateMap<M_Districts, M_DistrictsDTO>().ReverseMap();
        }
    }
}
