namespace GTT.Application.Requests
{
    public class UpdateGroupRequestModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupImage { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Website { get; set; }
        public string GroupType { get; set; }
        public List<int> Sports { get; set; }
        public bool? IsActive { get; set; }
    }
}