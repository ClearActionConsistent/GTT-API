using Dapper;
using GTT.Application;
using GTT.Application.Extensions;
using GTT.Application.Repositories;
using GTT.Application.Requests;
using GTT.Application.Response;
using GTT.Application.ViewModels;
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

        public async Task<int> AddAsync(ClassVM entity)
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
            try
            {
                var sql = @"UPDATE c
                           SET c.IsDeleted = 1
                           FROM Class c
                           WHERE c.ClassId = @classId";

                var result = await _connection.ExecuteAsync(sql, new { classId = id }, _tran);
                _tran.Commit();
                return result;
            }
            catch (Exception ex)
            {
                var error = $"ClassRepository - {Helpers.BuildErrorMessage(ex)}";
                throw new Exception(error);
            }          
        }

        public async Task<IReadOnlyList<ClassVM>> GetAllAsync()
        {
            var insertChallengeSql = @"SELECT * FROM Challenge";
            var result = await _connection.QueryAsync<ClassVM>(insertChallengeSql, _tran);
            return result.ToList(); 
        }

        public async Task<ClassVM> GetByIdAsync(int id)
        {
            try
            {
                var query = @"SELECT c.ClassId,
                                 c.Title,
                                 c.CoachId, 
                                 c.CommunityId, 
                                 c.Duration,
                                 c.StartDate, 
                                 c.CreatedBy, 
                                 c.UpdatedBy, 
                                 c.CreatedDate,
                                 c.UpdatedDate,
                                 c.IsActive,
                                 c.IsDeleted
                           FROM Class c WHERE ClassId = @Id";

                var parameter = new DynamicParameters();
                parameter.Add("@ClassId", id);

                var result = await _connection.QuerySingleOrDefaultAsync<ClassVM>(query, new { Id = id }, _tran);
                return result;
            }
            catch (Exception ex)
            {
                var error = $"ClassRepository - {Helpers.BuildErrorMessage(ex)}";
                throw new Exception(error);
            }
        }

        public async Task<int> UpdateAsync(ClassVM entity)
        {
            var sql = @"UPDATE Challenge SET something equal something";
            var result = await _connection.ExecuteAsync(sql, entity, _tran);
            return result;
        }

        public async Task<BaseResponseModel> CreateClass(CreateClassRequestModel request)
        {
            try
            {
                #region  Check CommunityId and CoachId
                var sql = @"SELECT cm.CommunityId, c.CoachId  FROM Community cm, Coach c 
                            WHERE cm.CommunityId = @communityId and c.CoachId = @coachId";

                var parameter = new DynamicParameters();

                parameter.Add("@coachId", request.CoachId);
                parameter.Add("@communityId", request.CommunityId);

                var queryData = await _connection.QueryFirstOrDefaultAsync(sql, parameter, _tran);

                if (queryData == null)
                {
                    return new BaseResponseModel(HttpStatusCode.NotFound, "CommunityId or CoachId invalid");
                }

                sql = @"SELECT c.Title FROM Class c WHERE c.Title = @title";

                parameter.Add("@title", request.Title);

                var queryTitle = await _connection.QueryFirstOrDefaultAsync(sql, parameter, _tran);

                if (queryTitle != null)
                {
                    return new BaseResponseModel(HttpStatusCode.BadRequest, "Title must be unique");
                }

                #endregion

                sql = @"INSERT INTO Class
                             VALUES(@title, @coachId, @communityId, @duration, @startDate, @createdBy, @updatedBy, @createdDate, @updatedDate, @IsActive);
                             DECLARE @classId int                             
                             SET @classId = SCOPE_IDENTITY()
                             SELECT * FROM Class WHERE ClassId = @classId";

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

                var result = await _connection.QueryFirstAsync<ClassVM>(sql, parameter, _tran);
                _tran.Commit();
                return new BaseResponseModel(result);
            }
            catch (Exception ex)
            {
                var error = $"ClassRepository - {Helpers.BuildErrorMessage(ex)}";
                throw new Exception(error);
            }
        }
    }
}