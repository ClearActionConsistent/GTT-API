using Dapper;
using GTT.Application;
using GTT.Application.Extensions;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Requests.ExerciseLib;
using GTT.Application.Response;
using GTT.Application.ViewModels;
using System.Data;
using System.Net;

namespace GTT.Infrastructure.Repositories
{
    public class ExerciseLibRepository : IExerciseLibRepository
    {
        #region Private Members
        private readonly IDbConnection _connection;
        #endregion

        #region Constructors
        public ExerciseLibRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.CreateConnection();
        }

        public ExerciseLibRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        #endregion

        public async Task<BaseResponseModel> CreateExerciseLib(CreateExerciseLibRequestModel request)
        {
            try
            {
                var checkClassId = await CheckClassExist(request.ClassId);

                if (!checkClassId)
                    return new BaseResponseModel(HttpStatusCode.NotFound, "Class Id Invalid");

                var checkCommunityId = await CheckCommunityExist(request.CommunityId);

                if (!checkCommunityId)
                    return new BaseResponseModel(HttpStatusCode.NotFound, "Community Id Invalid");

                var queryCheckName = @"SELECT Name FROM Exercise e
                                            WHERE e.Name = @ExName";

                var query = @"INSERT INTO Exercise (ClassId, Name, Image, IsActive, Description, FocusArea, Equipment, Video, CommunityId, CreatedDate, CreatedBy, UpdatedBy, UpdatedDate)
                                VALUES (@ClassId, @ExName, @ExImage, @IsActive, @Description, @FocusArea, @Equipment, @Video, @CommunityId, @CreatedDate, @CreatedBy, @UpdateBy, @UpdatedDate)
                                DECLARE @exerciseId int                             
                                SET @exerciseId = SCOPE_IDENTITY()
                                SELECT ExerciseId, ClassId, CommunityId, Name, Image, Video, Description, FocusArea, Equipment, IsActive, IsDeleted, CreatedBy, UpdatedBy, CreatedDate, UpdatedDate 
                                FROM Exercise WHERE ExerciseId = @exerciseId";

                var parameters = new DynamicParameters();
                parameters.Add("@ClassId", request.ClassId);
                parameters.Add("@ExName", request.ExerciseName);
                parameters.Add("@ExImage", request.ExerciseImage);
                parameters.Add("@IsActive", request.IsActive);
                parameters.Add("@Description", request.Description);
                parameters.Add("@FocusArea", request.FocusArea);
                parameters.Add("@Equipment", request.Equipment);
                parameters.Add("@Video", request.ExerciseVideo);
                parameters.Add("@CommunityId", request.CommunityId);
                parameters.Add("@CreatedDate", request.CreatedDate);
                parameters.Add("@CreatedBy", request.CreatedBy);
                parameters.Add("@UpdateBy", request.UpdatedBy);
                parameters.Add("@UpdatedDate", request.UpdatedDate);

                var checkName = await _connection.QueryFirstOrDefaultAsync(queryCheckName, parameters);

                if (checkName == null)
                {
                    var result = await _connection.QueryFirstAsync<ExerciseLibVM>(query, parameters);

                    return new BaseResponseModel(result);
                }
                else
                {
                    return new BaseResponseModel
                    (
                        HttpStatusCode.BadRequest,
                        "Exercise Name must be unique"
                    );
                }
            }
            catch (Exception ex)
            {
                var error = $"ExerciseLibRepository - {Helpers.BuildErrorMessage(ex)}";
                throw new Exception(error);
            }
        }

        private async Task<bool> CheckClassExist(int Id)
        {
            try
            {
                var checkClassId = @"SELECT ClassId FROM CLASS
                                    WHERE ClassId = @Id";
                var resultCheck = await _connection.QueryFirstOrDefaultAsync(checkClassId, new { Id });

                return resultCheck != null ? true : false;
            }
            catch (Exception ex)
            {
                var error = $"ExerciseLibRepository - {Helpers.BuildErrorMessage(ex)}";
                throw new Exception(error);
            }
            
        }

        private async Task<bool> CheckCommunityExist(int Id)
        {
            try
            {
                var checkCommunityId = @"SELECT CommunityId FROM Community
                                    WHERE CommunityId = @Id";
                var resultCheck = await _connection.QueryFirstOrDefaultAsync(checkCommunityId, new { Id });

                return resultCheck != null ? true : false;
            }
            catch (Exception ex)
            {
                var error = $"ExerciseLibRepository - {Helpers.BuildErrorMessage(ex)}";
                throw new Exception(error);
            }

        }
    }
}
