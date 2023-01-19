using Dapper;
using GTT.Application;
using GTT.Application.Extensions;
using GTT.Application.Interfaces.Repositories;
using GTT.Application.Requests;
using GTT.Application.Response;
using System.Data;
using System.Net;

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

        public async Task<BaseResponseModel> CreateGroup(CreateGroupRequestModel request)
        {
            try
            {
                var queryCheckName = @"SELECT GroupName FROM Groups e
                                            WHERE e.GroupName = @GrName";

                var query = @"INSERT INTO Groups (GroupName, GroupImage, IsActive, Description, Location, GroupType, TotalRunner, CreatedDate, CreatedBy, UpdatedBy, UpdatedDate)
                                VALUES (@GrName, @GrImage, @IsActive, @Description, @Location, @GrType, @TotalRunner, @CreatedDate, @CreatedBy, @UpdateBy, @UpdatedDate)";

                var parameters = new DynamicParameters();
                parameters.Add("@GrName", request.GroupName);
                parameters.Add("@GrImage", request.GroupImage);
                parameters.Add("@IsActive", request.IsActive);
                parameters.Add("@Description", request.Description);
                parameters.Add("@Location", request.Location);
                parameters.Add("@GrType", request.GroupType);
                parameters.Add("@TotalRunner", request.TotalRunner);
                parameters.Add("@CreatedDate", request.CreatedDate);
                parameters.Add("@CreatedBy", request.CreatedBy);
                parameters.Add("@UpdateBy", request.UpdatedBy);
                parameters.Add("@UpdatedDate", request.UpdatedDate);

                var checkName = await _connection.QueryFirstOrDefaultAsync(queryCheckName, parameters);

                if (checkName == null)
                {
                    await _connection.QueryFirstAsync<GroupsResponse>(query, parameters);

                    return new BaseResponseModel
                        (
                            HttpStatusCode.Created,
                            "Group Name be created success"
                        );
                }
                else
                {
                    return new BaseResponseModel
                    (
                        HttpStatusCode.BadRequest,
                        "Group Name must be unique"
                    );
                }
            }
            catch (Exception ex)
            {
                var error = $"GroupRepository - {Helpers.BuildErrorMessage(ex)}";
                throw new Exception(error);
            }
        }
        #endregion

        public async Task<ListGroupResponse> GetGroups(int pageSize, int pageIndex)
        {
            try
            {
                var sql = @$"SELECT GroupId ,GroupName, Description, Location, GroupType, CreatedDate, TotalRunner, IsActive
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
    }
}
