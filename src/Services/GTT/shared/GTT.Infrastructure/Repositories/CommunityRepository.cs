using Dapper;
using GTT.Application;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Response;
using System.Data;

namespace GTT.Infrastructure.Repositories
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly IDbConnection _connection;

        public CommunityRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.CreateConnection();
        }
        public CommunityRepository(IDbConnection dbConnection)
        {
            _connection = dbConnection;
        }

        public async Task<BaseResponseModel> GetAllCommunity()
        {
            try
            {
                var getAllSql = @"SELECT CommunityId
                              ,CommunityName
                              ,Image
                              ,IsActive
                              ,IsDeleted
                            FROM Community";

                var result = await _connection.QueryAsync(getAllSql);

                return new BaseResponseModel(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
