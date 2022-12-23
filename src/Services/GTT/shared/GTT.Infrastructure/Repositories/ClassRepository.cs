using Dapper;
using GTT.Application;
using GTT.Application.Repositories;
using GTT.Application.Requests;
using GTT.Application.Response;
using GTT.Domain.Entities;
using System.Data;
using System.Net;

namespace GTT.Infrastructure.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _tran = null;

        public ClassRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.CreateConnection();
            _tran = _connection.BeginTransaction();
        }
        public ClassRepository(IDbConnection dbConnection, IDbTransaction tran)
        {
            _connection = dbConnection;
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

        public async Task<BaseResponseModel> CreateClass(CreateClassRequestModel request)
        {
            try
            {
                var parameter = new DynamicParameters();

                #region  Check CommunityId and CoachId
                var sql = @"SELECT cm.CommunityId, c.CoachId  FROM Community cm, Coach c 
                            WHERE cm.CommunityId = @communityId and c.CoachId = @coachId";

                parameter.Add("@coachId", request.CoachId);
                parameter.Add("@communityId", request.CommunityId);

                var queryData = await _connection.QueryFirstOrDefaultAsync<CoachCommunityResponse>(sql, parameter, _tran);

                if (queryData == null)
                {
                    return new BaseResponseModel(HttpStatusCode.NotFound, "CommunityId or CoachId invalid");
                }
                
                #endregion

                sql = @"INSERT INTO Class
                             VALUES(@title, @coachId, @communityId, @duration, @startDate, @createdBy, @updatedBy, @createdDate, @updatedDate, @IsActive)";

                parameter.Add("@title", request.Title);
                parameter.Add("@duration", request.Duration);
                parameter.Add("@startDate", request.StartDate);
                parameter.Add("@isActive", request.IsActive);
                parameter.Add("@startDate", request.StartDate);
                parameter.Add("@createdBy", request.CreatedBy);
                parameter.Add("@updatedBy", request.UpdatedBy);
                parameter.Add("@createdDate", request.CreatedDate);
                parameter.Add("@updatedDate", request.UpdatedDate);
                parameter.Add("@IsActive", request.IsActive);

                var result = await _connection.ExecuteAsync(sql, parameter, _tran);

                _tran.Commit();

                return new BaseResponseModel(HttpStatusCode.OK, "Success");
            }
            catch(Exception ex)
            {
                var error = $"ClassRepository - {Helpers.BuildErrorMessage}";
                throw new Exception(error, ex);
            }
        }
    }
}
