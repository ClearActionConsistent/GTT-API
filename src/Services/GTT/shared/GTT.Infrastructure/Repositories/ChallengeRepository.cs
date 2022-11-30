using Dapper;
using GTT.Application;
using GTT.Application.Repositories;
using GTT.Domain.Entities;
using System.Data;

namespace GTT.Infrastructure.Repositories
{
    public class ChallengeRepository : IChallengeRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _tran;
        public ChallengeRepository(IDbConnection connection, IDbTransaction tran)
        {
            _connection = connection;
            _tran = tran;
        }

        public async Task<int> AddAsync(Challenge entity)
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

            var result = await _connection.ExecuteAsync(insertChallengeSql, param, transaction: _tran);

            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var insertChallengeSql = @"DELETE FROM Products WHERE Id = @Id";
            var result = await _connection.ExecuteAsync(insertChallengeSql, new { Id = id }, _tran);

            return result;
        }

        public async Task<IReadOnlyList<Challenge>> GetAllAsync()
        {
            var insertChallengeSql = @"SELECT * FROM Challenge";
            var result = await _connection.QueryAsync<Challenge>(insertChallengeSql, _tran);

            return result.ToList();
        }

        public async Task<Challenge> GetByIdAsync(int id)
        {
            var insertChallengeSql = @"SELECT * FROM Challenge WHERE Id = @Id";
            var result = await _connection.QuerySingleOrDefaultAsync<Challenge>(insertChallengeSql, new { Id = id }, _tran);

            return result;
        }

        public async Task<int> UpdateAsync(Challenge entity)
        {
            var sql = @"UPDATE Challenge SET something equal something";
            var result = await _connection.ExecuteAsync(sql, entity, _tran);

            return result;
        }
    }
}
