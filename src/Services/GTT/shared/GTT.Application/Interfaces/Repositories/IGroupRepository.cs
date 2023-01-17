using GTT.Application.Response;
using GTT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.Interfaces.Repositories
{
    public interface IGroupRepository
    {
        Task<ListGroupResponse> GetGroups(int pageSize, int pageIndex);
    }
}
