using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workflow.Data;
using Workflow.Service.Interface;


namespace Workflow.Service
{
    public class SessionService : ISessionService
    {
        private const string _userSessionKey = "_userKey";
        
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void RemoveAllSession()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }

        public AdminUser SetUser(AdminUser user)
        {
            try
            {
                if (user != null)
                {
                    _httpContextAccessor.HttpContext.Session.SetObject(_userSessionKey, user);
                    
                    return user;
                }
            }
            catch (Exception e) { throw e; }
            return null;
        }

        public AdminUser GetUser()
        {
            try
            {
                var user = _httpContextAccessor.HttpContext.Session.GetObject<AdminUser>(_userSessionKey);
                
                return user;
            }
            catch (Exception e) { throw e; }
        }

    }
}

public static class SessionContextExtensions
{
    public static void SetObject(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }
    public static T GetObject<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }
}