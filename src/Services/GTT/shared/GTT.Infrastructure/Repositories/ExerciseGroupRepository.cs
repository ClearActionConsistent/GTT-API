using System.Data;
using Dapper;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Requests;
using GTT.Application;
using GTT.Domain.Entities;
using GTT.Application.Extensions;

namespace GTT.Infrastructure.Repositories
{
    public class ExerciseGroupRepository : IExerciseGroupRepository
    {
        #region Private Members
        private readonly IDbConnection _connection;
        #endregion

        #region Constructors
        public ExerciseGroupRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.CreateConnection();
        }

        public ExerciseGroupRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        #endregion

        public async Task<int> CreateExGroup(ExGroupRequestModel request)
        {
            try
            {
                var query = @"INSERT INTO ExcerciseGroup (GroupNumber, GroupName, Community, Address, City, Quotation, Phone, IsActive)
                              VALUES (@groupNumber, @groupName, @community, @address, @city, @quotation, @phone, @isActive)";

                var queryParameters = new DynamicParameters();
                queryParameters.Add("@groupNumber", request.GroupNumber);
                queryParameters.Add("@groupName", request.GroupName);
                queryParameters.Add("@community", request.Community);
                queryParameters.Add("@address", request.Address);
                queryParameters.Add("@city", request.City);
                queryParameters.Add("@quotation", request.Quotation);
                queryParameters.Add("@phone", request.Phone);
                queryParameters.Add("@isActive", request.IsActive);

                var result = await _connection.ExecuteAsync(query, queryParameters);

                return result;
            }
            catch (Exception ex)
            {
                var error = $"ExGroupRepository - {Helpers.BuildErrorMessage(ex)}";
                throw new Exception(error);
            }
        }

        public async Task<ListExerciseGroupResponse> GetAllExGroup(int pageSize, int pageIndex, string filter)
        {
            try
            {
                var sql = @$"SELECT Id ,GroupNumber, GroupName, Community, Address, City, Quotation, Phone, IsActive
                            FROM ExcerciseGroup
                            {filter}
                            ORDER BY GroupName ASC, Community ASC
                            OFFSET @offset ROWS
                            FETCH NEXT @limit ROW ONLY;
                            SELECT COUNT(*) AS TotalRows FROM ExcerciseGroup;";

                var queryParameters = new DynamicParameters();
                queryParameters.Add("@limit", pageSize);
                queryParameters.Add("@offset", (pageIndex - 1) * pageSize);

                var query = await _connection.QueryMultipleAsync(sql, queryParameters, commandType: CommandType.Text);

                var excerciseGroups = (await query.ReadAsync<ExerciseGroupResponse>()).ToList();
                var totalRow = await query.ReadSingleOrDefaultAsync<long>();

                return new ListExerciseGroupResponse
                {
                    ExcerciseGroups = excerciseGroups,
                    TotalRow = (int)totalRow
                };
            }
            catch (Exception ex)
            {
                var error = $"ExGroupRepository - {Helpers.BuildErrorMessage(ex)}";
                throw new Exception(error);
            }
        }
    }
}
