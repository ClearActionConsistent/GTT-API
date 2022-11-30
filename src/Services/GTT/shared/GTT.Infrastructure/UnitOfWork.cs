using GTT.Application;
using GTT.Application.Repositories;

namespace GTT.Infrastructure
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public UnitOfWork(IDbConnectionFactory dbConnectionFactory,
            IChallengeRepository challenges,
            IClassRepository classes)
        {
            _dbConnectionFactory = dbConnectionFactory;
            Challenges = challenges;
            Classes = classes;
        }

        public IChallengeRepository Challenges { get; }
        public IClassRepository Classes { get; }
    }
}
