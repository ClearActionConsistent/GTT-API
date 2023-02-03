using System.Data;
using Dapper;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Requests;
using GTT.Application;
using GTT.Application.Extensions;
using GTT.Application.Response;
using System.Net;

namespace GTT.Infrastructure.Repositories
{
    public class SportsRepository : ISportsRepository
    {
        #region Private Members
        private readonly IDbConnection _connection;
        #endregion

        #region Constructors
        public SportsRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.CreateConnection();
        }

        public SportsRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        #endregion

        public async Task<BaseResponseModel> CreateSport(SportRequestModel request)
        {
            try
            {
                var sportName = await CheckSportNameIsUnique(request.SportName);
                if (sportName != null)
                {
                    return new BaseResponseModel(HttpStatusCode.BadRequest, "Sport name has already existed!");
                }

                var query = @"INSERT INTO Sports(SportImage, SportName, SportType, CreatedBy,CreatedDate, IsActive)
                              VALUES (@sportImage, @sportName, @sportType, @createdBy,@createdDate, @isActive)";

                var queryParameters = new DynamicParameters();
                queryParameters.Add("@sportImage", request.SportImage);
                queryParameters.Add("@sportName", request.SportName);
                queryParameters.Add("@sportType", request.SportType);
                queryParameters.Add("@isActive", request.IsActive);
                queryParameters.Add("@createdBy", Guid.NewGuid().ToString());
                queryParameters.Add("@createdDate", DateTime.Now);

                var result = await _connection.ExecuteAsync(query, queryParameters);
                if (result < 0)
                {
                    return new BaseResponseModel(HttpStatusCode.BadRequest, "Failed to insert data into Sports table");
                }

                return new BaseResponseModel(HttpStatusCode.OK, "Success");
            }
            catch (Exception ex)
            {
                var error = $"SportsRepository - {Helpers.BuildErrorMessage(ex)}";
                return null;
            }
        }

        public async Task<ListSportsResponse> GetSports(int pageIndex, int pageSize)
        {
            try
            {
                var sql = @"SELECT 
                                    SportId, 
                                    SportImage, 
                                    SportName, 
                                    SportType, 
                                    CreatedBy, 
                                    UpdatedBy, 
                                    CreatedDate, 
                                    UpdatedDate, 
                                    IsActive, 
                                    IsDeleted
                              FROM Sports
                              ORDER BY SportName ASC
                              OFFSET @offset ROWS
                              FETCH NEXT @limit ROWS ONLY;
                              SELECT COUNT(*) AS TotalRows FROM Sports;";

                var queryParameters = new DynamicParameters();
                queryParameters.Add("@offset", (pageIndex - 1) * pageSize);
                queryParameters.Add("@limit", pageSize);

                var query = await _connection.QueryMultipleAsync(sql, queryParameters, commandType: CommandType.Text);

                var sports = (await query.ReadAsync<SportResponse>()).ToList();
                var totalRow = await query.ReadSingleOrDefaultAsync<long>();

                return new ListSportsResponse
                {
                    Sports = sports,
                    TotalRow = (int)totalRow
                };

            }
            catch (Exception ex)
            {
                var error = $"SportsRepository - {Helpers.BuildErrorMessage(ex)}";
                return null;
            }
        }

        public async Task<BaseResponseModel> UpdateSport(int sportId, SportRequestModel request)
        {
            try
            {
                var query = $@"SELECT SportName FROM Sports WHERE SportId = {sportId}";

                var sportNameById = await _connection.QueryFirstOrDefaultAsync<string>(query);

                var sportName = await CheckSportNameIsUnique(request.SportName);
                if (sportName != null && sportNameById == request.SportName)
                {
                    return new BaseResponseModel(HttpStatusCode.Created, "Sport name has already existed.");
                }

                var sql = @"UPDATE Sports
                            SET SportImage = @sportImage, 
                                SportName = @sportName, 
                                SportType = @sportType,
                                UpdatedBy = @updatedBy,
                                UpdatedDate = @updatedDate,
                                IsActive = @isActive
                            WHERE SportId = @sportId;";

                var queryParameters = new DynamicParameters();
                queryParameters.Add("@sportImage", request.SportImage);
                queryParameters.Add("@sportName", request.SportName);
                queryParameters.Add("@sportType", request.SportType);
                queryParameters.Add("@isActive", request.IsActive);
                queryParameters.Add("@updatedBy", Guid.NewGuid().ToString());
                queryParameters.Add("@updatedDate", DateTime.Now);
                queryParameters.Add("@sportId", sportId);

                var result = await _connection.ExecuteAsync(sql, queryParameters);
                if (result < 0)
                {
                    return new BaseResponseModel(HttpStatusCode.BadRequest, "Failed to update sport info.");
                }

                return new BaseResponseModel(null);
            }
            catch (Exception ex)
            {
                var error = $"SportsRepository - {Helpers.BuildErrorMessage(ex)}";
                return null;
            }
        }

        private async Task<string> CheckSportNameIsUnique(string sportName)
        {
            try
            {
                var sql = @$"SELECT SportName
                             FROM   Sports                             
                             WHERE  SportName LIKE '%{sportName}%'";

                var result = await _connection.QueryFirstOrDefaultAsync<string>(sql);

                return result;
            }
            catch (Exception ex)
            {
                var error = $"SportsRepository - {Helpers.BuildErrorMessage(ex)}";
                return null;
            }
        }
    }
}
