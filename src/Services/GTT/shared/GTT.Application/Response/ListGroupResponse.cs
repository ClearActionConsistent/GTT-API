using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.Response
{
    public class ListGroupResponse
    {
        public List<GroupsResponse> Groups { get; set; } = new List<GroupsResponse>();
        public int TotalRow { get; set; }
    }

    public class GroupsResponse
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public int Sport { get; set; }

        public string GroupType { get; set; }

        public DateTime CreatedDate { get; set; }

        public int TotalRunner { get; set; }

        public bool IsActive { get; set; }
    }
}
