using GTT.Application.Response;

namespace GTT.Application.ViewModels
{
    public class CreateChallengeData : BaseModel
    {
        public int Calories { get; set; }

        public int SplatPoints { get; set; }

        public int AvgHr { get; set; }

        public int MaxHr { get; set; }

        public int Miles { get; set; }

        public int Steps { get; set; }

        public int memberID { get; set; }

        public int classID { get; set; }
    }
}
