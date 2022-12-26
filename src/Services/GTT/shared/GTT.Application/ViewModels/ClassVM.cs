using GTT.Application.Response;

namespace GTT.Application.ViewModels
{
    public class ClassVM : BaseModel
    {
        public int ClassId { get; set; }
        public string Title { get; set; } = String.Empty;
        public int CoachId { get; set; }
        public int CommunityId { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
    }
}
