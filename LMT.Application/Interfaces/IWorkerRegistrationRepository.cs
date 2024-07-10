using LMT.Domain.Entities;

namespace LMT.Application.Interfaces
{
    public interface IWorkerRegistrationRepository
    {
        Task<List<T_WorkerRegistrations>> GetAllWorkerRegistrationsAsync();
        Task<T_WorkerRegistrations> GetWorkerRegistrationByIdAsync(int workerRegistrationId);
        Task CreateWorkerRegistrationAsync(T_WorkerRegistrations workerRegistration);
        Task UpdateWorkerRegistrationAsync(T_WorkerRegistrations workerRegistration);
        Task DeleteWorkerRegistrationAsync(int workerRegistrationId);
    }

}
