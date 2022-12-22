
using GTT.Application.ViewModels;
using GTT.Domain.Entities;

namespace GTT.Application.Repositories
{
    public interface IChallengeRepository
    {
        Task<ChallengeVM> AddAsync(ChallengeVM challengeVM);
    }
}
