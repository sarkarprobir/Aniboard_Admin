using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.Data
{
    public class EmailTemplate
    {
        public int TemplateId { get; set; }
        public string? TemplateCode { get; set; }
        public string? Heading { get; set; }
        public string? Content { get; set; }
        public string? Remarks { get; set; }
        public int? TemplateOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public int? isDelete { get; set; }
    }
}
