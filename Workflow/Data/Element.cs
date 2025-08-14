using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.Data
{
    public class BackgroundImage
    {
        public int ImageId { get; set; }
        public string? DisplayName { get; set; }
        public double? ImageSize { get; set; }
        public string? ImageName { get; set; }
        public string? ImageNameThumb { get; set; }
        public int? ImageW { get; set; }
        public int? ImageH { get; set; }
        public int? isDelete { get; set; }
        public int? TotalCount { get; set; }
    }
}
