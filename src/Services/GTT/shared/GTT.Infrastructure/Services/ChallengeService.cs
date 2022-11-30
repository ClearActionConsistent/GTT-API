using Dapper;
using GTT.Application;
using GTT.Application.Services;
using GTT.Application.ViewModels;
using GTT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Infrastructure.Services
{
    public class ChallengeService : IChallengeService
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public ChallengeService(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<ChallengeVM> CreateChallengeAsync(CreateChallengeData data, CancellationToken cancellationToken)
        {
            var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
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
                    field1 = "data.field1",
                    field2 = "data.field2",
                    field3 = "data.field3"
                };

                var insertedChallenges = await connection.QueryAsync<Challenge>(insertChallengeSql, param, transaction: tran);

                var insertedChallenge = insertedChallenges?.FirstOrDefault();

                tran.Commit();
                connection.Close();

                return new ChallengeVM { Name = insertedChallenge?.Name};
            }
            catch
            {
                tran.Rollback();
                connection?.Close();

                throw;
            }
            finally
            {
                connection?.Close();
            }
        }
    }
}
