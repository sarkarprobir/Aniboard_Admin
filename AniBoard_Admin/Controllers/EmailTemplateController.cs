using AniBoard_Admin.Models;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult EmailTemplateList()
        {
            return View();
        }
    }
}
