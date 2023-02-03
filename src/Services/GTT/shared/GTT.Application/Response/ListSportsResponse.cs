namespace GTT.Application.Response
{
    public class ListSportsResponse
    {
        public List<SportResponse> Sports { get; set; } = new List<SportResponse>();
        public int TotalRow { get; set; }
    }

    public class SportResponse
    {
        public int SportId { get; set; }
        public string SportImage { get; set; }
        public string SportName { get; set; }
        public string SportType { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
