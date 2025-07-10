using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using Workflow.Data;
using Workflow.Service.Interface;

namespace AniBoard_Admin.ViewComponents
{
    [ViewComponent(Name = "AddEditUser")]
    public class AddEditUserComponent : ViewComponent
    {
        private readonly IAPIService _apiService;

        public AddEditUserComponent(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId = null)
        {
            dynamic dyAddEditAdminUser = new ExpandoObject();

            User user = new User();
            if (!string.IsNullOrEmpty(userId))
            {
                List<User> userList = new List<User>();
                // calling api
                var queryParams = new Dictionary<string, string?>();
                queryParams["userId"] = userId.ToString();
                var users = await _apiService.GetAsync<ApiResponse<List<User>>>("Backoffice/UserGet", queryParams);
                if (users != null)
                {
                    if (users.Data.Count > 0)
                    {
                        user = users.Data.FirstOrDefault();
                    }
                }

            }
            dyAddEditAdminUser.userId = userId;
            dyAddEditAdminUser.User = user;

            return await Task.FromResult((IViewComponentResult)View("AddEditUser", dyAddEditAdminUser));
        }


    }
}
