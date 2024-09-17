using LMT.Application.DTOs;
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
            try
            {
                _dbContext.T_WorkerRegistrations.Add(workerRegistration);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task DeleteWorkerRegistrationAsync(long workerRegistrationId)
        {
            var workerRegistration = await _dbContext.T_WorkerRegistrations.FindAsync(workerRegistrationId);
            if (workerRegistration != null)
            {
                _dbContext.T_WorkerRegistrations.Remove(workerRegistration);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<GetT_WorkerRegistrationDTO>> GetAllWorkerRegistrationsAsync()
        {
            var result = await _dbContext.GetT_WorkerRegistrationDTO
             .FromSqlRaw("EXEC [dbo].[SP_GetWorkerRegistrations]")
             .ToListAsync();

            return result;
            //return await _dbContext.T_WorkerRegistrations.ToListAsync();
        }

        public async Task<List<GetT_WorkerRegistrationDTO>> GetAllWorkerRegistrationsAsync(string? searchText)
        {
            //if (string.IsNullOrEmpty(searchText))
            //{
            //    return await _dbContext.T_WorkerRegistrations.ToListAsync();
            //}
            //return await _dbContext.T_WorkerRegistrations
            //    .Where(c => c.Worker_Name.Contains(searchText)
            //    || c.Worker_Aadhaar_No.Contains(searchText)
            //    || c.Worker_eShram_No.Contains(searchText)
            //    || c.Worker_Contact_No.Contains(searchText))
            //    .ToListAsync();
            var result = await _dbContext.GetT_WorkerRegistrationDTO
           .FromSqlRaw("EXEC [dbo].[SP_GetWorkerRegistrationsWithSearch] @SearchTerm = {0}", searchText ?? (object)DBNull.Value)
           .ToListAsync();

            return result;
        }

        public async Task<T_WorkerRegistrations> GetWorkerRegistrationByIdAsync(long workerRegistrationId)
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
