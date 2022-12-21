using GTT.Application.Requests;

namespace GTT.Application.Interfaces.Repositories
{
    public interface IExGroupRepository
    {
        Task<int> CreateExGroup(ExGroupRequestModel request);
    }
}
