using Dapper;
using GTT.Application;
using GTT.Application.Repositories;
using GTT.Application.ViewModels;
using GTT.Domain.Entities;
using System.Data;

namespace GTT.Infrastructure.Repositories
{
    public class ChallengeRepository : IChallengeRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _tran;

        public ChallengeRepository(IDbConnectionFactory dbConnectionFactory) {
            _connection = dbConnectionFactory.CreateConnection();
            _tran = _connection.BeginTransaction();
        }

        public async Task<ChallengeVM> AddAsync(ChallengeVM challenge)
        {
            
            try{
                var insertChallengeSql = @"INSERT INTO Challenge(Calories, SplatPoints, AvgHR, MaxHR, Miles, Steps, MemberID, CreatedDate, UpdatedDate)
                                        VALUES(@Calories, @SplatPoints, @AvgHR, @MaxHR, @Miles, @Steps, @MemberID, @CreatedDate, @UpdatedDate)
                                        DECLARE @challengeID int
                                        SET @challengeID = SCOPE_IDENTITY()
                                        SELECT* FROM Challenge WHERE challengeID = @ChallengeId
                                        ";

                var param = new
                {
                    Calories = challenge.Calories,
                    SplatPoints = challenge.SplatPoints,
                    AvgHR = challenge.AvgHr,
                    MaxHR = challenge.MaxHr,
                    Miles = challenge.Miles,
                    Steps = challenge.Steps,
                    MemberID = challenge.memberID,
                    CreatedDate = challenge.CreatedDate,
                    UpdatedDate = challenge.UpdatedDate,            
                };

                var result = await _connection.QuerySingleOrDefaultAsync<ChallengeVM>(insertChallengeSql, param, _tran);
                _tran.Commit();
                return result;
            }
            catch (Exception ex)
            {
                _tran.Rollback();
                throw new Exception();
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var insertChallengeSql = @"DELETE FROM Products WHERE Id = @Id and Age = @Age";
            var dataToInsert = new { 
                Id = id,
                Name = "Huy",
                Age = 18
            };
            var result = await _connection.ExecuteAsync(insertChallengeSql, dataToInsert);

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
