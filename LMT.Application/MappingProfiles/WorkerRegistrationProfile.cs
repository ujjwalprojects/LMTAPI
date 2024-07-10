using AutoMapper;
using LMT.Application.DTOs;
using LMT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMT.Application.MappingProfiles
{
    public class WorkerRegistrationProfile : Profile
    {
        public WorkerRegistrationProfile()
        {
            CreateMap<T_WorkerRegistrations, T_WorkerRegistrationsDTO>().ReverseMap();
        }
    }
}
