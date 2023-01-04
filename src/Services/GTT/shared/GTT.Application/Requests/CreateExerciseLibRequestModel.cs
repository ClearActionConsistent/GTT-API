using GTT.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.Requests.ExerciseLib
{
    public class CreateExerciseLibRequestModel : BaseModel
    {
        public string ExerciseName { get; set; }

        public int ClassId { get; set; }

        public int CommunityId { get; set; }

        public string Description { get; set; } = string.Empty;

        public string ExerciseImage { get; set; }

        public string ExerciseVideo { get; set; } = null;

        public string Equipment { get; set; }

        public string FocusArea { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
