using System.Data;
using Dapper;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Requests;
using GTT.Application;
using GTT.Domain.Enums;

namespace GTT.Infrastructure.Repositories
{
    public class ExGroupRepository : IExGroupRepository
    {
        #region Private Members
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _tran;
        #endregion

        #region Constructors
        public ExGroupRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.CreateConnection();
            _tran = _connection.BeginTransaction();
        }
        
        public ExGroupRepository(IDbConnection connection, IDbTransaction tran)
        {
            _connection = connection;
            _tran = tran;
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

                var result = await _connection.ExecuteAsync(query, queryParameters, transaction: _tran);

                return result;
            }
            catch (Exception ex)
            {
                var error = $"ExGroupRepository - {Helper.BuildErrorMessage(ex)}";
                throw new Exception(error);
            }
        }
    }
}
