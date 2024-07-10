using LMT.Domain.Entities;

namespace LMT.Application.Interfaces
{
    public interface ITaskAllocationFormRepository
    {
        Task<List<T_TaskAllocationForms>> GetAllTaskAllocationFormsAsync();
        Task<T_TaskAllocationForms> GetTaskAllocationFormByIdAsync(string taskAllocationFormId);
        Task CreateTaskAllocationFormAsync(T_TaskAllocationForms taskAllocationForm);
        Task UpdateTaskAllocationFormAsync(T_TaskAllocationForms taskAllocationForm);
        Task DeleteTaskAllocationFormAsync(string taskAllocationFormId);
    }

}
