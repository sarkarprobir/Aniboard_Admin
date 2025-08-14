using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using Workflow.Data;
using Workflow.Service.Interface;
namespace AniBoard_Admin.ViewComponents
{
    [ViewComponent(Name = "AddEditEmailTemplate")]
    public class AddEditEmailTemplateComponent : ViewComponent
    {
        private readonly IAPIService _apiService;

        public AddEditEmailTemplateComponent(IAPIService apiService)
        {
            _apiService = apiService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int templateId = 0)
        {
            dynamic dyAddEditEmailTemplate = new ExpandoObject();

            EmailTemplate emailTemplate = new EmailTemplate();
            if (templateId>0)
            {
                List<EmailTemplate> emailTemplateList = new List<EmailTemplate>();
                // calling api
                var queryParams = new Dictionary<string, string?>();
                queryParams["templateId"] = templateId.ToString();
                var template = await _apiService.GetAsync<ApiResponse<List<EmailTemplate>>>("Backoffice/EmailTemplateGet", queryParams);
                if (template != null)
                {
                    if (template.Data.Count > 0)
                    {
                        emailTemplate = template.Data.FirstOrDefault();
                    }
                }

            }
            dyAddEditEmailTemplate.templateId = templateId;
            dyAddEditEmailTemplate.emailTemplate = emailTemplate;

            return await Task.FromResult((IViewComponentResult)View("AddEditEmailTemplate", dyAddEditEmailTemplate));
        }


    }
}
