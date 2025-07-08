using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Principal;
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
                queryParams["searchKeyword"] = q;

            
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
        public IActionResult ShowAddEditModal(int userId = 0)
        {
            try
            {
                return ViewComponent("AddEditAdminUser", new { userId = userId});
            }
            catch (Exception err)
            {
                //_sessionService.SetServerException(err);
                return Redirect("/error/");
            }
        }
        public async Task<IActionResult> SaveAdminUser(string data)
        {
            var fromdata = Request.Form["data"];
            AdminUser adminUser= JsonConvert.DeserializeObject<AdminUser>(fromdata);
            var result = await _apiService.PostAsync<AdminUser, ApiResponse<AdminUser>>("Backoffice/SaveAdminUser", adminUser);

            if (result?.Status == true)
            {
                Console.WriteLine("User created: " + result?.Message);
                return Json(new
                {
                    Ok = true,
                    Errors = ""
                });
            }
            else
            {
                Console.WriteLine("Failed to create user: " + result?.Message);
                return Json(new
                {
                    Ok = false,
                    Errors = result?.Message
                });
            }



            

        }
    }
}
