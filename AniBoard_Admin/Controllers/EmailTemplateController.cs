using AniBoard_Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Dynamic;
using Workflow.Data;
using Workflow.Service.Interface;

namespace AniBoard_Admin.Controllers
{
    public class EmailTemplateController : Controller
    {
        private readonly IAPIService _apiService;
        private readonly ISessionService _sessionService;

        public EmailTemplateController(IAPIService apiService, ISessionService sessionService)
        {
            _apiService = apiService;
            _sessionService = sessionService;
        }

        public async Task<IActionResult> EmailTemplateList(string q = null)
        {
            List<EmailTemplate> emailTemplates = new List<EmailTemplate>();
            dynamic dyEmailtemplates = new ExpandoObject();
            // calling api
            var queryParams = new Dictionary<string, string?>();

            if (!string.IsNullOrEmpty(q))
                queryParams["searchKeyword"] = q;


            var emailTemplate = await _apiService.GetAsync<ApiResponse<List<EmailTemplate>>>("Backoffice/EmailTemplateGet", queryParams);
            if (emailTemplate != null)
            {
                if (emailTemplate.Data.Count > 0)
                {
                    emailTemplates = emailTemplate.Data;
                }
            }
            dyEmailtemplates.emailTemplates = emailTemplates;
            dyEmailtemplates.q = q;
            return View(dyEmailtemplates);
        }

        public IActionResult ShowAddEditEmailTemplateModal(int templateId = 0)
        {
            try
            {
                return ViewComponent("AddEditEmailTemplate", new { templateId = templateId });
            }
            catch (Exception err)
            {
                //_sessionService.SetServerException(err);
                return Redirect("/error/");
            }
        }
        [HttpPost()]
        public async Task<IActionResult> SaveEmailTemplate(string data)
        {
            var fromdata = Request.Form["data"];
            EmailTemplate emailTemplate = JsonConvert.DeserializeObject<EmailTemplate>(fromdata);
            var result = await _apiService.PostAsync<EmailTemplate, ApiResponse<EmailTemplate>>("Backoffice/SaveEmailTemplate", emailTemplate);

            if (result?.Status == true)
            {
                Console.WriteLine("Email Template created: " + result?.Message);
                return Json(new
                {
                    Ok = true,
                    Errors = ""
                });
            }
            else
            {
                Console.WriteLine("Failed to create email template: " + result?.Message);
                return Json(new
                {
                    Ok = false,
                    Errors = result?.Message
                });
            }





        }

    }
}
