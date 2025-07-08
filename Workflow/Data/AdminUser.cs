using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow.Data
{
    public class AdminUser
    {
        public int adminId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string userPassword { get; set; }
        public int userRoleId { get; set; }
        public DateTime? lastLogin { get; set; }
        public bool isActive { get; set; } = true;
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public int updatedBy { get; set; }
        public int isDelete { get; set; }
        //public int userStatus { get; set; }

    }
}
