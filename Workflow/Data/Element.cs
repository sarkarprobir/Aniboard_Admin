using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

    public class Elementcls
    {
        public int ElementId { get; set; }
        public string? ElementName { get; set; }
        public double? ImageSize { get; set; }
        public string? ImageName { get; set; }
        public string? ImageNameThumb { get; set; }
        public int? ImageW { get; set; }
        public int? ImageH { get; set; }
        public int? isDelete { get; set; }
        public int? TotalCount { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? CompanyUniqueId { get; set; }
        public string? ImageTag { get; set; }
    }
    public class ElementCategory
    {
        public int ElementId { get; set; }
        public string? ElementName { get; set; }
        
    }

}
