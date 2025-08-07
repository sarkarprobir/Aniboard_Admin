using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.Data
{
    public class User
    {
        public string? CustomerId { get; set; }
        public string? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyShortName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Email_2 { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Country { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public string? Password { get; set; }
        public string? ImagePath { get; set; }
        public string? LastPaymentId { get; set; }
        public string? TimeZone { get; set; }
        public string? CustomerIP { get; set; }
        public int? isDelete { get; set; }
    }
}
