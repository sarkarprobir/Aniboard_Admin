using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using Workflow.Data;
using Workflow.Service.Interface;

namespace AniBoard_Admin.ViewComponents
{
    [ViewComponent(Name = "AddEditAdminUser")]
    public class AddEditAdminUserComponent: ViewComponent
    {
        private readonly IAPIService _apiService;

        public AddEditAdminUserComponent(IAPIService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userId = 0)
        {
            dynamic dyAddEditAdminUser = new ExpandoObject();
            
            AdminUser adminUser = new AdminUser();
            if (userId > 0)
            {
                List<AdminUser> adminUserList = new List<AdminUser>();
                // calling api
                var queryParams = new Dictionary<string, string?>();
                queryParams["userId"] = userId.ToString();
                var users = await _apiService.GetAsync<ApiResponse<List<AdminUser>>>("Backoffice/AdminUserGet", queryParams);
                if (users != null)
                {
                    if (users.Data.Count > 0)
                    {
                        adminUser = users.Data.FirstOrDefault();
                    }
                }

            }
            dyAddEditAdminUser.userId = userId;
            dyAddEditAdminUser.adminUser = adminUser;
            
            return await Task.FromResult((IViewComponentResult)View("AddEditAdminUser", dyAddEditAdminUser));
        }
    }
}
