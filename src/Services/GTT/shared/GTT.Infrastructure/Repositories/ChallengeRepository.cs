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

        public ChallengeRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.CreateConnection();
            _tran = _connection.BeginTransaction();
        }

        public async Task<ChallengeVM> AddAsync(ChallengeVM challenge)
        {
            try
            {
                var insertChallengeSql = @"INSERT INTO Challenge(Calories, SplatPoints, AvgHR, MaxHR, Miles, Steps, MemberID, ClassID, CreatedDate, UpdatedDate)
                                    VALUES(@Calories, @SplatPoints, @AvgHR, @MaxHR, @Miles, @Steps, @MemberID, @ClassID, @CreatedDate, @UpdatedDate)
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
                    ClassID = challenge.classID,
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
                throw new Exception(ex.Message);
            }
            
        }
    }
}
