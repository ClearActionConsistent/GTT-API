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

        public string Name { get; set; }

        public int ClassId { get; set; }

        public int CommunityId { get; set; }

        public string Image { get; set; }

        public string ExerciseVideo { get; set; } = null;

        public string Description { get; set; } = string.Empty;

        public string Equipment { get; set; }

        public string FocusArea { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}
