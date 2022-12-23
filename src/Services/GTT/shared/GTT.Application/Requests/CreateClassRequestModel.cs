using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTT.Application.Response;

namespace GTT.Application.Requests
{
    public class CreateClassRequestModel : BaseModel
    {
        public string Title { get; set; } = String.Empty;
        public int CoachId { get; set; }
        public int CommunityId { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}