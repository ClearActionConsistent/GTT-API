using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.Requests
{
    public class UpdateClassData
    {
        public int ClassId { get; set; }
        public string Title { get; set; }
        public int CoachId { get; set; }
        public int CommunityId { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}