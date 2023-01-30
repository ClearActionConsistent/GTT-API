using GTT.Application.Requests;
using GTT.Application.Response;

namespace GTT.Application.Interfaces.Repositories
{
    public interface ISportsRepository
    {
        Task<BaseResponseModel> CreateSport(SportRequestModel request);
        Task<ListSportsResponse> GetSports(int pageIndex, int pageSize);
        Task<BaseResponseModel> UpdateSport(int sportId, SportRequestModel request);
    }
}
