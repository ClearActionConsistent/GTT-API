namespace GTT.Application.Requests
{
    public class ExGroupRequestModel
    {
        public int GroupNumber { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string Community { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int Quotation { get; set; }
        public string Phone { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
