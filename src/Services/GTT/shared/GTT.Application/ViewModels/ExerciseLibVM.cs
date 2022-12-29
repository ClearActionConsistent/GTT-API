using GTT.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.ViewModels
{
    public class ExerciseLibVM : BaseModel
    {
        public int ExerciseId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int ClassId { get; set; }

        public string Image { get; set; } = String.Empty;

        public bool IsActive { get; set; }
    }
}
