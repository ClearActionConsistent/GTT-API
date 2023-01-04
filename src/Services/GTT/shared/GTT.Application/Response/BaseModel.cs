using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.Response
{
    public class BaseModel
    {
        public string CreatedBy { get; set; } 
        public string UpdatedBy { get; set; } 
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
