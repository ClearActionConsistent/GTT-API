using GTT.Application.Response;

namespace GTT.Application.Requests
{
    public class CreateChallengeData : BaseModel
    {
        public int Calories { get; set; }

        public int SplatPoints { get; set; }

        public int AvgHr { get; set; }

        public int MaxHr { get; set; }

        public int Miles { get; set; }

        public int Steps { get; set; }

        public int ClassID { get; set; }
    }
}
