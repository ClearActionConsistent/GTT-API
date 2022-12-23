using System.Data;
using Dapper;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Requests;
using GTT.Application;
using GTT.Domain.Entities;

namespace GTT.Infrastructure.Repositories
{
    public class ExGroupRepository : IExGroupRepository
    {
        #region Private Members
        private readonly IDbConnection _connection;
        #endregion

        #region Constructors
        public ExGroupRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.CreateConnection();
        }
        
        public ExGroupRepository(IDbConnection connection)
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

        public async Task<List<ExcerciseGroup>> GetAllExGroup()
        {
            try
            {
                var query = @"SELECT Id ,GroupNumber, GroupName, Community, Address, City, Quotation, Phone, IsActive
                              FROM ExcerciseGroup";

                var result = (await _connection.QueryAsync<ExcerciseGroup>(query)).ToList();

                return result;
            }
            catch (Exception ex)
            {
                var error = $"ExGroupRepository - {Helpers.BuildErrorMessage(ex)}";
                throw new Exception(error);
            }
        }
    }
}
