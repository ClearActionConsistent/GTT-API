using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.Response
{
    public class GroupResponse
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupImage { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }
        public string GroupType { get; set; }
        public bool IsActive { get; set; }
    }
}
