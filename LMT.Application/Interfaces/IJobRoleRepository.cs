using LMT.Domain.Entities;

namespace LMT.Application.Interfaces
{
    public interface IJobRoleRepository
    {
        Task<List<M_JobRoles>> GetAllJobRolesAsync();
        Task<M_JobRoles> GetJobRoleByIdAsync(int jobRoleId);
        Task CreateJobRoleAsync(M_JobRoles jobRole);
        Task UpdateJobRoleAsync(M_JobRoles jobRole);
        Task DeleteJobRoleAsync(int jobRoleId);
    }

}
