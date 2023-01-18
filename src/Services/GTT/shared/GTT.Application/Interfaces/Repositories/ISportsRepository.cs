using GTT.Application.Requests;
using GTT.Application.Response;

namespace GTT.Application.Interfaces.Repositories
{
    public interface ISportsRepository
    {
        Task<int> CreateSport(CreateSportRequestModel request);
        Task<ListSportsResponse> GetSports(int pageIndex, int pageSize);
    }
}
