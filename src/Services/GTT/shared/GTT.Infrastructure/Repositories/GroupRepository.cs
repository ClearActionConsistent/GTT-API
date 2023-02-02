using Dapper;
using GTT.Application;
using GTT.Application.Extensions;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Response;
using System.Data;

namespace GTT.Infrastructure.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        #region Private Members
        private readonly IDbConnection _connection;
        #endregion

        #region Constructors
        public GroupRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.CreateConnection();
        }

        public GroupRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        #endregion

        public async Task<ListGroupsResponse> GetGroups(int pageSize, int pageIndex)
        {
            try
            {
                var sql = @$"SELECT 
                                g.GroupId ,g.GroupName, 
                                g.Description, g.Location, 
                                g.GroupType, STRING_AGG(s.SportName,',') AS Sports, 
                                g.CreatedDate, g.TotalRunner, g.IsActive
                            FROM Groups g
							JOIN SportGroup sg 
                                ON g.GroupId = sg.GroupId
							JOIN Sports s 
                                ON sg.SportId = s.SportId
                            GROUP BY g.GroupId, g.GroupName, 
                                     g.Description, g.Location,
                                     g.GroupType, g.CreatedDate,
                                     g.TotalRunner, g.IsActive
                            ORDER BY GroupName ASC
                            OFFSET @offset ROWS
                            FETCH NEXT @limit ROW ONLY;
                            SELECT COUNT(*) AS TotalRows FROM Groups;";

                var queryParameters = new DynamicParameters();
                queryParameters.Add("@limit", pageSize);
                queryParameters.Add("@offset", (pageIndex - 1) * pageSize);

                var query = await _connection.QueryMultipleAsync(sql, queryParameters, commandType: CommandType.Text);

                var groups = (await query.ReadAsync<GroupsResponse>()).ToList();
                var totalRow = await query.ReadSingleOrDefaultAsync<long>();

                return new ListGroupsResponse
                {
                    Groups = groups,
                    TotalRow = (int)totalRow
                };
            }
            catch (Exception ex)
            {
                var error = $"GroupRepository - {Helpers.BuildErrorMessage(ex)}";
                throw new Exception(error);
            }
        }
    }
}
