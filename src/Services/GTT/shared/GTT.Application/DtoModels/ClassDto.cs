using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.DtoModels
{
    public class ClassDto
    {
        public int ClassId { get; set; }
        public string Title { get; set; } = String.Empty;
        public int CoachId { get; set; }
    }
}
