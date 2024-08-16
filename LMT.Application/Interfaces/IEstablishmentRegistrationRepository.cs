using LMT.Domain.Entities;

namespace LMT.Application.Interfaces
{
    public interface IEstablishmentRegistrationRepository
    {
        Task<List<T_EstablishmentRegistrations>> GetAllEstablishmentRegistrationsAsync();
        Task<List<T_EstablishmentRegistrations>> GetAllEstablishmentRegistrationsAsync(string searchText);
        Task<T_EstablishmentRegistrations> GetEstablishmentRegistrationByIdAsync(int establishmentRegistrationId);
        Task CreateEstablishmentRegistrationAsync(T_EstablishmentRegistrations establishmentRegistration);
        Task UpdateEstablishmentRegistrationAsync(T_EstablishmentRegistrations establishmentRegistration);
        Task DeleteEstablishmentRegistrationAsync(int establishmentRegistrationId);
    }

}
