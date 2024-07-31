using LMT.Application.Interfaces;
using LMT.Domain.Entities;
using LMT.Infrastructure.Data;
using LMT.Infrastructure.IDGenerators;
using Microsoft.EntityFrameworkCore;

namespace LMT.Infrastructure.Repositories
{
    public class TaskAllocationFormRepository : ITaskAllocationFormRepository
    {
        private readonly EFDBContext _dbContext;
        private readonly UniqueIdGenerator _uniqueIdGenerator;

        public TaskAllocationFormRepository(EFDBContext dbContext)
        {
            _dbContext = dbContext;
            _uniqueIdGenerator = new UniqueIdGenerator(_dbContext);
        }

        public async Task CreateTaskAllocationFormAsync(T_TaskAllocationForms taskAllocationForm)
        {
            //Generate TaskID
            taskAllocationForm.Task_Id = _uniqueIdGenerator.GenerateUniqueId();

            try
            {
                _dbContext.T_TaskAllocationForms.Add(taskAllocationForm);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        
        }

        public async Task DeleteTaskAllocationFormAsync(string taskAllocationFormId)
        {
            var taskAllocationForm = await _dbContext.T_TaskAllocationForms.FindAsync(taskAllocationFormId);
            if (taskAllocationForm != null)
            {
                _dbContext.T_TaskAllocationForms.Remove(taskAllocationForm);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<T_TaskAllocationForms>> GetAllTaskAllocationFormsAsync()
        {
            return await _dbContext.T_TaskAllocationForms.ToListAsync();
        }

        public async Task<T_TaskAllocationForms> GetTaskAllocationFormByIdAsync(string taskAllocationFormId)
        {
            return await _dbContext.T_TaskAllocationForms.FindAsync(taskAllocationFormId);
        }

        public async Task UpdateTaskAllocationFormAsync(T_TaskAllocationForms taskAllocationForm)
        {
            _dbContext.Entry(taskAllocationForm).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
