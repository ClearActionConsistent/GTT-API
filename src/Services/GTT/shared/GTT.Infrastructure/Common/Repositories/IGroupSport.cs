using GTT.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.Common.Repositories
{
    public interface IGroupSport
    {
        public Task<bool> CreateGroupSport(List<int> groupSport, int groupId);
    }
}
