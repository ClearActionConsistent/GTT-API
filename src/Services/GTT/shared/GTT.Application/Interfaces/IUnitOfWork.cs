namespace GTT.Application.Repositories
{
    public interface IUnitOfWork
    {
        IChallengeRepository Challenges { get; }
        IClassRepository Classes { get; }
        void Complete();
        void Rollback();
    }
}
