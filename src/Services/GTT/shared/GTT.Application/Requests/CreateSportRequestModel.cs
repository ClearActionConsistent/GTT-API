namespace GTT.Application.Requests
{
    public class CreateSportRequestModel
    {
        public string SportImage { get; set; }
        public string SportName { get; set; }
        public string SportType { get; set; }
        public bool IsActive { get; set; }
    }
}
