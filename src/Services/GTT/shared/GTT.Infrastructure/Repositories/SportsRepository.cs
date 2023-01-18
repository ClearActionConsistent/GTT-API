using System.Data;
using Dapper;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Requests;
using GTT.Application;
using GTT.Application.Extensions;
using GTT.Application.Response;

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

        public async Task<int> CreateSport(CreateSportRequestModel request)
        {
            try
            {
                var query = @"INSERT INTO Sports(SportImage, SportName, SportType, CreatedBy, IsActive)
                              VALUES (@sportImage, @sportName, @sportType, NULL, @isActive)";

                var queryParameters = new DynamicParameters();
                queryParameters.Add("@sportImage", request.SportImage);
                queryParameters.Add("@sportName", request.SportName);
                queryParameters.Add("@sportType", request.SportType);
                queryParameters.Add("@isActive", request.IsActive);

                var result = await _connection.ExecuteAsync(query, queryParameters);

                return result;
            }
            catch (Exception ex)
            {
                var error = $"SportsRepository - {Helpers.BuildErrorMessage(ex)}";
                return -1;
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

                var sports = (await query.ReadAsync<SportReponse>()).ToList();
                var totalRow = await query.ReadSingleOrDefaultAsync<long>();

                return new ListSportsResponse
                {
                    Sports = sports,
                    TotalRow = (int)totalRow
                };

            }
            catch(Exception ex)
            {
                var error = $"SportsRepository - {Helpers.BuildErrorMessage(ex)}";
                return null;
            }
        }
    }
}
