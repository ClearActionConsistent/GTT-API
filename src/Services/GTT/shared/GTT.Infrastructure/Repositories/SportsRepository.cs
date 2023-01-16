using System.Data;
using Dapper;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Requests;
using GTT.Application;
using GTT.Domain.Entities;
using GTT.Application.Extensions;

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

        public async Task<int> CreateSports(CreateSportRequestModel request)
        {
            try
            {
                var query = @"INSERT INTO Sports(SportImage, SportName, SportType, IsActive)
                              VALUES (@sportImage, @sportName, @sportType, @isActive)";

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
    }
}
