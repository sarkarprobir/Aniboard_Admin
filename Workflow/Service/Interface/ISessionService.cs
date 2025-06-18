using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workflow.Data;

namespace Workflow.Service.Interface
{
    public interface ISessionService
    {
        void RemoveAllSession();
        AdminUser SetUser(AdminUser user);
        AdminUser GetUser();
        
    }
}
