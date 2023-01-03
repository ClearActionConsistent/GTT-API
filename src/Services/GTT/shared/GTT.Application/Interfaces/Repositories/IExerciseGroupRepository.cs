using GTT.Application.Requests;
using GTT.Domain.Entities;

namespace GTT.Application.Interfaces.Repositories
{
    public interface IExerciseGroupRepository
    {
        Task<int> CreateExGroup(ExGroupRequestModel request);
        Task<ListExerciseGroupResponse> GetAllExGroup(int pageSize, int pageIndex, string filter);
    }
}
