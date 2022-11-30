using Dapper;
using GTT.Application;
using GTT.Application.Repositories;
using GTT.Domain.Entities;

namespace GTT.Infrastructure.Repositories
{
    public class ChallengeRepository : IChallengeRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public ChallengeRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<int> AddAsync(Challenge entity)
        {
            var connection = await _dbConnectionFactory.CreateConnectionAsync();
            var tran = connection.BeginTransaction();

            try
            {
                var insertChallengeSql = @"
                    INSERT INTO Challenge(field1, field2, field3)
                    VALUES(@field1, @field2, @field3)
                    SET @ChallengeId = SCOPE_IDENTITY()
                    SELECT * FROM Challenge WHERE ChallengeId = @ChallengeId
                ";

                var param = new
                {
                    field1 = "entity.field1",
                    field2 = "entity.field2",
                    field3 = "entity.field3"
                };

                var result = await connection.ExecuteAsync(insertChallengeSql, param, transaction: tran);

                tran.Commit();
                connection.Close();

                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                tran.Rollback();
                connection?.Close();
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var connection = await _dbConnectionFactory.CreateConnectionAsync();

            try
            {
                var insertChallengeSql = @"DELETE FROM Products WHERE Id = @Id";
                var result = await connection.ExecuteAsync(insertChallengeSql, new { Id = id});
                connection.Close();

                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                connection?.Close();
            }
        }

        public async Task<IReadOnlyList<Challenge>> GetAllAsync()
        {
            var connection = await _dbConnectionFactory.CreateConnectionAsync();

            try
            {
                var insertChallengeSql = @"SELECT * FROM Challenge";
                var result = await connection.QueryAsync<Challenge>(insertChallengeSql);
                connection.Close();
                
                return result.ToList();
            }
            catch
            {
                throw;
            }
            finally
            {
                connection?.Close();
            }
        }

        public async Task<Challenge> GetByIdAsync(int id)
        {
            var connection = await _dbConnectionFactory.CreateConnectionAsync();

            try
            {
                var insertChallengeSql = @"SELECT * FROM Challenge WHERE Id = @Id";
                var result = await connection.QuerySingleOrDefaultAsync<Challenge>(insertChallengeSql, new {Id = id});
                connection.Close();

                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                connection?.Close();
            }
        }

        public async Task<int> UpdateAsync(Challenge entity)
        {
            var connection = await _dbConnectionFactory.CreateConnectionAsync();

            try
            {
                var sql = @"UPDATE Challenge SET something equal something";
                var result = await connection.ExecuteAsync(sql, entity);
                connection.Close();

                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                connection?.Close();
            }
        }
    }
}
