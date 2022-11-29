using GTT.Application.ViewModels;

namespace GTT.Application.Services
{
    public interface IChallengeService
    {
        public Task<ChallengeVM> CreateChallengeAsync(CreateChallengeData data);
    }
}
