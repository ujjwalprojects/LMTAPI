using AutoMapper;
using LMT.Application.DTOs;
using LMT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMT.Infrastructure.MappingProfiles
{
    public class EstablishmentRegistrationProfile:Profile
    {
        public EstablishmentRegistrationProfile()
        {
            CreateMap<T_EstablishmentRegistrations, T_EstablishmentRegistrationsDTO>().ReverseMap();
        }
    }
}
