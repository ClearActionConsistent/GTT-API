using Dapper;
using GTT.Application;
using GTT.Application.Extensions;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Requests;
using GTT.Application.Response;
using System.Data;
using System.Text;

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

        public async Task<ListGroupResponse> GetGroups(int pageSize, int pageIndex)
        {
            try
            {
                var sql = @$"SELECT GroupId ,GroupName, Description, Location, Sport, GroupType, CreatedDate, TotalRunner, IsActive
                            FROM Groups
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

                return new ListGroupResponse
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

        public async Task<GroupResponse> UpdateGroup(UpdateGroupRequestModel group)
        {
            try
            {
                var checkGroup = $"SELECT GroupId, GroupName FROM Groups WHERE GroupId = {group.GroupId};";
                var queryGroup = await _connection.QueryFirstOrDefaultAsync<GroupResponse>(checkGroup, commandType: CommandType.Text);

                if (queryGroup == null)
                {
                    throw new Exception("Group is invalid !");
                }

                //Remove all current list sport 
                var queryRemoveSport = $"DELETE FROM SportGroup WHERE GroupId = {group.GroupId};";
                await _connection.ExecuteAsync(queryRemoveSport, commandType: CommandType.Text);

                //Check group name unique
                var queryName = $"SELECT GroupName FROM Groups WHERE GroupName LIKE '%{group.GroupName}%'";
                var groupName = await _connection.QueryFirstOrDefaultAsync<string>(queryName, commandType: CommandType.Text);

                if (groupName != null && queryGroup.GroupName != group.GroupName)
                {
                    throw new Exception("Group name has already existed !");
                }

                //Update sport for group
                if (group.Sports.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    for(int i = 0; i < group.Sports.Count; i++)
                    {
                        if(group.Sports.Count == 1 || i == group.Sports.Count - 1)
                        {
                            sb.Append($"({group.Sports[i]}, {group.GroupId});");
                            break;
                        }

                        sb.Append($"({group.Sports[i]}, {group.GroupId}), ");
                    }

                    var query = $"INSERT INTO SportGroup (SportId, GroupId) VALUES {sb}";
                    var insertSports = await _connection.ExecuteAsync(query, commandType: CommandType.Text);

                    if(insertSports <= 0)
                    {
                        throw new Exception("Can not insert sports for group !");
                    }
                }

                //Update Group   
                var queryUpdateGroup = @"UPDATE Groups 
                                            SET GroupName = @groupName,
                                                Description = @description,
                                                GroupImage = @groupImage,                                              
                                                Website = @website,
                                                Location = @location,
                                                GroupType = @groupType,
                                                TotalRunner = g.TotalRunner,
                                                CreatedDate = g.CreatedDate,
                                                UpdatedDate = @updatedDate,
                                                CreatedBy = g.CreatedBy,
                                                UpdatedBy = @updatedBy,    
                                                IsActive = @isActive,
                                                IsDeleted = g.isDeleted
                                          FROM Groups g
                                          WHERE GroupId = @groupId                     
                                          SELECT * FROM Groups WHERE GroupId = @groupId;";
             
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@groupName", group.GroupName);
                queryParameters.Add("@groupImage", group.GroupImage);
                queryParameters.Add("@description", group.Description);
                queryParameters.Add("@location", group.Location);
                queryParameters.Add("@website", group.Website);
                queryParameters.Add("@groupType", group.GroupType);
                queryParameters.Add("@isActive", group.IsActive);
                queryParameters.Add("@updatedBy", Guid.NewGuid());
                queryParameters.Add("@updatedDate", DateTime.UtcNow);
                queryParameters.Add("@groupId", group.GroupId);

                var result = await _connection.QueryFirstAsync<GroupResponse>(queryUpdateGroup, queryParameters, commandType: CommandType.Text);
                
                return result;
            }
            catch (Exception ex)
            {
                var error = $"GroupRepository - {Helpers.BuildErrorMessage(ex)}";
                throw new Exception(error);
            }
        }
    }
}