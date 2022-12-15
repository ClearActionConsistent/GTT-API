using GTT.Application;
using GTT.Application.Repositories;
using GTT.Infrastructure.Repositories;
using System.Data;

namespace GTT.Infrastructure
{
    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _tran;
        public UnitOfWork(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.CreateConnection();
            _tran = _connection.BeginTransaction();
        }

        public IChallengeRepository Challenges { get; private set; }
        public IClassRepository Classes { get; private set; }

        public void Complete()
        {
            _tran?.Commit();
            _connection?.Close();
        }

        public void Dispose()
        {
            _tran?.Rollback();
            _connection?.Close();
        }

        public void Rollback()
        {
            _tran?.Rollback();
            _connection?.Close();
        }
    }
}
