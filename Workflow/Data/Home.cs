using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.Data
{
    public class FAQ
    {
        public int FaqId { get; set; }
        public string? FaqQuestion { get; set; }
        public int? DisplayOrder { get; set; }
        public string? FaqAnswer { get; set; }
        public int? isDelete { get; set; }
    }
}
