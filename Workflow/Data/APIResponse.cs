using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.Data
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
