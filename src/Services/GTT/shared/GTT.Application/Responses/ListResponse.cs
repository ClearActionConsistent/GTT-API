using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTT.Application.Responses
{
    public class ListResponse<T>
    {
        public int HttpStatus { get; set; } = 200;
        public string Code { get; set; } = "OK";
        public string Message { get; set; } = string.Empty;
        public List<T> DataDto { get; set; } = new List<T>();
    }
}
