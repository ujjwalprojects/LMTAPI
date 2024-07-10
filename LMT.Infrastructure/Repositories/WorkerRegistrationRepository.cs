using LMT.Application.Interfaces;
using LMT.Domain.Entities;
using LMT.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMT.Infrastructure.Repositories
{
    public class WorkerRegistrationRepository : IWorkerRegistrationRepository
    {
        private readonly EFDBContext _dbContext;

        public WorkerRegistrationRepository(EFDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateWorkerRegistrationAsync(T_WorkerRegistrations workerRegistration)
        {
            _dbContext.T_WorkerRegistrations.Add(workerRegistration);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteWorkerRegistrationAsync(int workerRegistrationId)
        {
            var workerRegistration = await _dbContext.T_WorkerRegistrations.FindAsync(workerRegistrationId);
            if (workerRegistration != null)
            {
                _dbContext.T_WorkerRegistrations.Remove(workerRegistration);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<T_WorkerRegistrations>> GetAllWorkerRegistrationsAsync()
        {
            return await _dbContext.T_WorkerRegistrations.ToListAsync();
        }

        public async Task<T_WorkerRegistrations> GetWorkerRegistrationByIdAsync(int workerRegistrationId)
        {
            return await _dbContext.T_WorkerRegistrations.FindAsync(workerRegistrationId);
        }

        public async Task UpdateWorkerRegistrationAsync(T_WorkerRegistrations workerRegistration)
        {
            _dbContext.Entry(workerRegistration).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
