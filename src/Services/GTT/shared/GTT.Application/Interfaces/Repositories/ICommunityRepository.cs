using GTT.Application.Response;

namespace GTT.Application.Interfaces.Repositories
{
    public interface ICommunityRepository
    {
        Task<BaseResponseModel> GetAllCommunity();
    }
}
