using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.ViewModels
{
    public class ClassesVM : BaseVM
    {
        public int ClassId { get; set; }    
        public string Title { get; set; } = String.Empty;
        public int CoachId { get; set; }
        public int ActId { get; set; }
        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
    }
}
