using Microsoft.AspNetCore.Mvc;
using Workflow.Data;
using Workflow.Service.Interface;

namespace AniBoard_Admin.Controllers
{
    public class AdminUserController : Controller
    {
        private readonly IAPIService _apiService;
        private readonly ISessionService _sessionService;

        public AdminUserController(IAPIService apiService, ISessionService sessionService)
        {
            _apiService = apiService;
            _sessionService = sessionService;
        }
        public async Task<IActionResult> AdminUserList(string q = null)
        {
            List<AdminUser> adminUser = new List<AdminUser>();

            // calling api
            var queryParams = new Dictionary<string, string?>();

            if (!string.IsNullOrEmpty(q))
                queryParams["userEmail"] = q;

            
            var users = await _apiService.GetAsync<ApiResponse<List<AdminUser>>>("Backoffice/AdminUserGet", queryParams);
            if (users != null)
            {
                if (users.Data.Count > 0)
                {
                    adminUser = users.Data;
                }
            }
            return View(adminUser);
        }
    }
}
