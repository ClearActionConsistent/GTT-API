using GTT.Application.Common.Models;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using GTT.Application;
using GTT.Application.Extensions;

namespace GTT.Application.Common.Repositories
{
    public class GroupSportRepository : IGroupSport
    {
        #region Private Members
        private readonly IDbConnection _connection;
        #endregion

        #region Constructors
        public GroupSportRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _connection = dbConnectionFactory.CreateConnection();
        }

        public GroupSportRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public GroupSportRepository() { }
        #endregion
        public async Task<bool> CreateGroupSport(List<int> groupSport, int groupId)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < groupSport.Count; i++)
            {
                if (groupSport.Count == 1 || i == groupSport.Count - 1)
                {
                    sb.Append($"({groupSport[i]}, {groupId});");
                    break;
                }

                sb.Append($"({groupSport[i]}, {groupId}), ");
            }

            var insert = $"INSERT INTO SportGroup (SportId, GroupId) VALUES {sb}";
            var insertSports = await _connection.ExecuteAsync(insert, commandType: CommandType.Text);

            return insertSports > 0;
        }
    }
}
