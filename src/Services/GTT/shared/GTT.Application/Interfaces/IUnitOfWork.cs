using GTT.Application.Interfaces.Repositories;

namespace GTT.Application.Repositories
{
    public interface IUnitOfWork
    {
        IChallengeRepository Challenges { get; }
        IClassRepository Classes { get; }
        IExGroupRepository ExGroup { get; }
        void Complete();
        void Rollback();
    }
}
