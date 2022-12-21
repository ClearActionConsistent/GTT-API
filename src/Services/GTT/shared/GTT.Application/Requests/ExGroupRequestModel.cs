namespace GTT.Application.Requests
{
    public class ExGroupRequestModel
    {
        public int GroupNumber { get; set; }
        public string GroupName { get; set; }
        public string Community { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Quotation { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
    }
}
