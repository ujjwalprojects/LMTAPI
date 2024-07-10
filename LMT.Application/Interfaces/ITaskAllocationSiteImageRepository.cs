using LMT.Domain.Entities;

namespace LMT.Application.Interfaces
{
    public interface ITaskAllocationSiteImageRepository
    {
        Task<List<T_TaskAllocationSiteImages>> GetAllTaskAllocationSiteImagesAsync();
        Task<T_TaskAllocationSiteImages> GetTaskAllocationSiteImageByIdAsync(int taskAllocationSiteImageId);
        Task CreateTaskAllocationSiteImageAsync(T_TaskAllocationSiteImages taskAllocationSiteImage);
        Task UpdateTaskAllocationSiteImageAsync(T_TaskAllocationSiteImages taskAllocationSiteImage);
        Task DeleteTaskAllocationSiteImageAsync(int taskAllocationSiteImageId);
    }

}
