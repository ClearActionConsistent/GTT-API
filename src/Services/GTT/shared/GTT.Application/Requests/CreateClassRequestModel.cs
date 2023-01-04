using GTT.Application.Response;

namespace GTT.Application.Requests
{
    public class CreateClassRequestModel : BaseModel
    {
        public string Title { get; set; }
        public int CoachId { get; set; }
        public int CommunityId { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}