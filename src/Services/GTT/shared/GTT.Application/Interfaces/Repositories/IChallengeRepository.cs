using GTT.Application.Requests;
using GTT.Application.Response;
using GTT.Domain.Entities;

namespace GTT.Application.Repositories
{
    public interface IChallengeRepository
    {
        Task<BaseResponseModel> AddAsync(CreateChallengeData challengeVM);

        Task<ListChallengeResponse> GetAllAsync(int pageSize, int pageIndex);
    }
}
