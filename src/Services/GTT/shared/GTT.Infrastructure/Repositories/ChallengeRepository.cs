using Dapper;
using GTT.Application;
using GTT.Application.Extensions;
using GTT.Application.Repositories;
using GTT.Application.Requests;
using GTT.Application.Response;
using GTT.Application.ViewModels;
using GTT.Domain.Entities;
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
                if (!checkclass)
                {
                    return new BaseResponseModel(HttpStatusCode.NotFound, "Class ID invalid");
                }

                var insertChallengeSql = @"INSERT INTO Challenge(Calories, SplatPoints, AvgHR, MaxHR, Miles, Steps, ClassID, CreatedDate, UpdatedDate, CreatedBy, UpdatedBy)
                                    VALUES(@Calories, @SplatPoints, @AvgHR, @MaxHR, @Miles, @Steps, @ClassID, @CreatedDate, @UpdatedDate, @CreatedBy, @UpdatedBy)
                                    DECLARE @challengeID int
                                    SET @challengeID = SCOPE_IDENTITY()
                                    SELECT
                                    ChallengeID, Calories, SplatPoints, AvgHR, MaxHR, Miles, Steps, ClassID,
                                    CreatedDate, UpdatedDate, CreatedBy, UpdatedBy
                                    FROM Challenge 
                                    WHERE ChallengeID = @ChallengeId
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

        public async Task<ListChallengeResponse> GetAllAsync(int pageSize, int pageIndex)
        {
            try
            {
                var sql = @"SELECT ChallengeID
                                  ,Calories
                                  ,SplatPoints
                                  ,AvgHR
                                  ,MaxHR
                                  ,Miles
                                  ,Steps
                                  ,ClassID
                            FROM Challenge
                            ORDER BY UpdatedDate DESC
                            OFFSET @offset ROWS
                            FETCH NEXT @limit ROW ONLY;
                            SELECT COUNT(*) AS TotalRows FROM Challenge;";

                var queryParameters = new DynamicParameters();
                queryParameters.Add("@limit", pageSize);
                queryParameters.Add("@offset", (pageIndex - 1) * pageSize);

                var query = await _connection.QueryMultipleAsync(sql, queryParameters, commandType: CommandType.Text);

                var challenges = (await query.ReadAsync<ChallengeResponse>()).ToList();
                var totalRow = await query.ReadSingleOrDefaultAsync<long>();

                return new ListChallengeResponse
                {
                    Challenges = challenges,
                    TotalRow = (int)totalRow
                };
            }
            catch (Exception ex)
            {
                var error = $"ChallengeRepository - {Helpers.BuildErrorMessage(ex)}";
                throw new Exception(error);
            }
        }

        private async Task<bool> checkClassExist(int classid)
        {
            try
            {
                var query = @"SELECT ClassId FROM Class WHERE ClassId = @classid";

                var result = await _connection.QueryFirstOrDefaultAsync(query, new { classid });

                return result != null ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
