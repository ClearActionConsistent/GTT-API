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

        public string ExerciseImage { get; set; } = null;

        public bool IsActive { get; set; } = true;
    }
}
