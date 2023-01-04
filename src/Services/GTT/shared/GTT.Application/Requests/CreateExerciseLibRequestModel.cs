using GTT.Application.Response;

namespace GTT.Application.Requests.ExerciseLib
{
    public class CreateExerciseLibRequestModel : BaseModel
    {
        public string ExerciseName { get; set; }

        public int ClassId { get; set; }

        public int CommunityId { get; set; }

        public string Description { get; set; }

        public string ExerciseImage { get; set; }

        public string ExerciseVideo { get; set; }

        public string Equipment { get; set; }

        public string FocusArea { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
