using GTT.Application.Requests;

namespace GTT.Application.Interfaces.Repositories
{
    public interface ISportsRepository
    {
        Task<int> CreateSport(CreateSportRequestModel request);
    }
}
