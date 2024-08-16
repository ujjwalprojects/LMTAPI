using LMT.Domain.Entities;

namespace LMT.Application.Interfaces
{
    public interface IWorkerRegistrationRepository
    {
        Task<List<T_WorkerRegistrations>> GetAllWorkerRegistrationsAsync();
        Task<List<T_WorkerRegistrations>> GetAllWorkerRegistrationsAsync(string searchText);
        Task<T_WorkerRegistrations> GetWorkerRegistrationByIdAsync(long workerRegistrationId);
        Task CreateWorkerRegistrationAsync(T_WorkerRegistrations workerRegistration);
        Task UpdateWorkerRegistrationAsync(T_WorkerRegistrations workerRegistration);
        Task DeleteWorkerRegistrationAsync(long workerRegistrationId);
    }

}
