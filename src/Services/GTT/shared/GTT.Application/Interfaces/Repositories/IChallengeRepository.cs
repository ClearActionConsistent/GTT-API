using GTT.Application.Requests;
using GTT.Application.Response;

namespace GTT.Application.Repositories
{
    public interface IChallengeRepository
    {
        Task<BaseResponseModel> AddAsync(CreateChallengeData challengeVM);
    }
}
