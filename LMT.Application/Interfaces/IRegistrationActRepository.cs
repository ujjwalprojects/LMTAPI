﻿using LMT.Domain.Entities;

namespace LMT.Application.Interfaces
{
    public interface IRegistrationActRepository
    {
        Task<List<M_RegistrationActs>> GetAllRegistrationActsAsync();
        Task<M_RegistrationActs> GetRegistrationActByIdAsync(int registrationActId);
        Task CreateRegistrationActAsync(M_RegistrationActs registrationAct);
        Task UpdateRegistrationActAsync(M_RegistrationActs registrationAct);
        Task DeleteRegistrationActAsync(int registrationActId);
    }

}
