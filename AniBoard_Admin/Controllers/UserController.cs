using Microsoft.AspNetCore.Mvc;
using Workflow.Service.Interface;
using Newtonsoft.Json;
using Workflow.Data;
using System.Dynamic;
namespace AniBoard_Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly IAPIService _apiService;
        private readonly ISessionService _sessionService;
        public UserController(IAPIService apiService, ISessionService sessionService)
        {
            _apiService = apiService;
            _sessionService = sessionService;
        }

        public async Task<IActionResult> UserList(string q = null)
        {
            List<User> user = new List<User>();
            dynamic dyUser = new ExpandoObject();
            // calling api
            var queryParams = new Dictionary<string, string?>();

            if (!string.IsNullOrEmpty(q))
                queryParams["searchKeyword"] = q;


            var users = await _apiService.GetAsync<ApiResponse<List<User>>>("Backoffice/UserGet", queryParams);
            if (users != null)
            {
                if (users.Data.Count > 0)
                {
                    user = users.Data;
                }
            }
            dyUser.user = user;
            dyUser.q = q;
            return View(dyUser);
        }
        public IActionResult ShowAddEditModal(string userId = null)
        {
            try
            {
                return ViewComponent("AddEditUser", new { userId = userId });
            }
            catch (Exception err)
            {
                //_sessionService.SetServerException(err);
                return Redirect("/error/");
            }
        }
        public async Task<IActionResult> SaveUser(string data)
        {
            var fromdata = Request.Form["data"];
            User user = JsonConvert.DeserializeObject<User>(fromdata);
            var result = await _apiService.PostAsync<User, ApiResponse<User>>("Backoffice/SaveUser", user);

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
