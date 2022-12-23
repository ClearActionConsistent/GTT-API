using GTT.Application.Requests;
using GTT.Domain.Entities;

namespace GTT.Application.Interfaces.Repositories
{
    public interface IExGroupRepository
    {
        Task<int> CreateExGroup(ExGroupRequestModel request);
        Task<List<ExcerciseGroup>> GetAllExGroup();
    }
}
