using GTT.Application.Requests;

namespace GTT.Application.Interfaces.Repositories
{
    public interface ISportsRepository
    {
        Task<int> CreateSports(CreateSportRequestModel request);
    }
}
