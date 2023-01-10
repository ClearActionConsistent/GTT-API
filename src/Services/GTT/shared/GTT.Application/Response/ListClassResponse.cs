using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.Response
{
    public class ListClassResponse
    {
        public List<ClassResponse> Classes { get; set; } = new List<ClassResponse>();
        public int TotalRow { get; set; }
    }

    public class ClassResponse : BaseModel
    {
        public int ClassId { get; set; }
        public string Title { get; set; }
        public int CoachId { get; set; }
        public int CommunityId { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
