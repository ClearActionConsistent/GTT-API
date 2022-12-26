using Dapper;
using GTT.Application;
using GTT.Application.Repositories;
using GTT.Application.Requests;
using GTT.Application.Response;
using GTT.Application.ViewModels;
using System.Data;
using System.Net;

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

        public async Task<BaseResponseModel> AddAsync(CreateChallengeData challenge)
        {
            try
            {
                var checkclass = await checkClassExist(challenge.ClassID);
                if(!checkclass)
                {
                    return new BaseResponseModel(HttpStatusCode.NotFound, "Class ID invalid");
                }

                var insertChallengeSql = @"INSERT INTO Challenge(Calories, SplatPoints, AvgHR, MaxHR, Miles, Steps, ClassID, CreatedDate, UpdatedDate, CreatedBy, UpdatedBy)
                                    VALUES(@Calories, @SplatPoints, @AvgHR, @MaxHR, @Miles, @Steps, @ClassID, @CreatedDate, @UpdatedDate, @CreatedBy, @UpdatedBy)
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
                    ClassID = challenge.ClassID,
                    CreatedDate = challenge.CreatedDate,
                    UpdatedDate = challenge.UpdatedDate,
                    CreatedBy = challenge.CreatedBy,
                    UpdatedBy = challenge.UpdatedBy,
                };

                var result = await _connection.QueryFirstAsync<ChallengeVM>(insertChallengeSql, param);
                return new BaseResponseModel(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }     
        }

        private async Task<bool> checkClassExist(int classid)
        {
            var query = @"SELECT * FROM Class WHERE ClassId = @classid";

            var result = await _connection.QueryFirstOrDefaultAsync(query, new { classid });

            return result != null ? true : false;
        }
    }
}
