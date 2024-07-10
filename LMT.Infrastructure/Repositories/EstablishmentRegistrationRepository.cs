using LMT.Application.Interfaces;
using LMT.Domain.Entities;
using LMT.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMT.Infrastructure.Repositories
{
    public class EstablishmentRegistrationRepository : IEstablishmentRegistrationRepository
    {
        private readonly EFDBContext _dbContext;

        public EstablishmentRegistrationRepository(EFDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateEstablishmentRegistrationAsync(T_EstablishmentRegistrations establishmentRegistration)
        {
            _dbContext.T_EstablishmentRegistrations.Add(establishmentRegistration);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteEstablishmentRegistrationAsync(int establishmentRegistrationId)
        {
            var establishmentRegistration = await _dbContext.T_EstablishmentRegistrations.FindAsync(establishmentRegistrationId);
            if (establishmentRegistration != null)
            {
                _dbContext.T_EstablishmentRegistrations.Remove(establishmentRegistration);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<T_EstablishmentRegistrations>> GetAllEstablishmentRegistrationsAsync()
        {
            return await _dbContext.T_EstablishmentRegistrations.ToListAsync();
        }

        public async Task<T_EstablishmentRegistrations> GetEstablishmentRegistrationByIdAsync(int establishmentRegistrationId)
        {
            return await _dbContext.T_EstablishmentRegistrations.FindAsync(establishmentRegistrationId);
        }

        public async Task UpdateEstablishmentRegistrationAsync(T_EstablishmentRegistrations establishmentRegistration)
        {
            _dbContext.Entry(establishmentRegistration).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
