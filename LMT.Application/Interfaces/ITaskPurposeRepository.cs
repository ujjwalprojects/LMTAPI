﻿using LMT.Domain.Entities;

namespace LMT.Application.Interfaces
{
    public interface ITaskPurposeRepository
    {
        Task<List<M_TaskPurposes>> GetAllTaskPurposesAsync();
        Task<M_TaskPurposes> GetTaskPurposeByIdAsync(int taskPurposeId);
        Task CreateTaskPurposeAsync(M_TaskPurposes taskPurpose);
        Task UpdateTaskPurposeAsync(M_TaskPurposes taskPurpose);
        Task DeleteTaskPurposeAsync(int taskPurposeId);
    }

}
