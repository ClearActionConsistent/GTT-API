using Dapper;
using GTT.Application;
using GTT.Application.Repositories;
using GTT.Application.ViewModels;
using System.Data;

namespace GTT.Infrastructure.Repositories
{
    public class ChallengeRepository : IChallengeRepository
    {
        private readonly IDbConnection _connection;

        public ChallengeRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.CreateConnection();
        }
        public ChallengeRepository(IDbConnection dbConnection) 
        {
            _connection = dbConnection;
        }

        public async Task<ChallengeVM> AddAsync(CreateChallengeData challenge)
        {
            try
            {
                var insertChallengeSql = @"INSERT INTO Challenge(Calories, SplatPoints, AvgHR, MaxHR, Miles, Steps, MemberID, ClassID, CreatedDate, UpdatedDate, CreatedBy, UpdatedBy)
                                    VALUES(@Calories, @SplatPoints, @AvgHR, @MaxHR, @Miles, @Steps, @MemberID, @ClassID, @CreatedDate, @UpdatedDate, @CreatedBy, @UpdatedBy)
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
                    CreatedBy = challenge.CreatedBy,
                    UpdatedBy = challenge.UpdatedBy,
                };

                var result = await _connection.QueryFirstAsync<ChallengeVM>(insertChallengeSql, param);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }     
        }
    }
}
