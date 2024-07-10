using LMT.Domain.Entities;

namespace LMT.Application.Interfaces
{
    public interface IStateRepository
    {
        Task<List<M_States>> GetAllStatesAsync();
        Task<M_States> GetStateByIdAsync(int stateId);
        Task CreateStateAsync(M_States state);
        Task UpdateStateAsync(M_States state);
        Task DeleteStateAsync(int stateId);
    }

}
