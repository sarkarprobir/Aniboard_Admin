using AniBoard_Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Dynamic;
using Workflow.Data;
using Workflow.Service.Interface;

namespace AniBoard_Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAPIService _apiService;
        private readonly ISessionService _sessionService;
        public HomeController(ILogger<HomeController> logger,IAPIService apiService, ISessionService sessionService)
        {
            _logger = logger;
            _apiService = apiService;
            _sessionService = sessionService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #region UserLogin
        public IActionResult Login()
        {
            dynamic dyLogin = new ExpandoObject();
            dyLogin.adminPersonUserID = Request.Cookies["adminPersonUserID"];
            dyLogin.adminPersonUserPassword = Request.Cookies["adminPersonUserPassword"];
            dyLogin.errMsg = "";
            return View(dyLogin);
        }
    
        public IActionResult Logout() 
        {
            _sessionService.RemoveAllSession();
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(string username, string userpassword, bool remember_me)
        {
            dynamic dyLogin = new ExpandoObject();
            try
            {
                string logincookies = string.Empty;
                string loginip = string.Empty;
                CookieOptions option = new CookieOptions();
                
                dyLogin.adminPersonUserID = "";
                dyLogin.adminPersonUserPassword = "";
                dyLogin.errMsg = "";
                if (remember_me == true)
                {
                    var random = new Random();
                    logincookies = random.Next().ToString() + Convert.ToString(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    option.Expires = DateTime.Now.AddDays(180);
                    Response.Cookies.Append("adminloginKey", logincookies, option);
                    Response.Cookies.Append("adminPersonUserID", username, option);
                    Response.Cookies.Append("adminPersonUserPassword", userpassword, option);
                }
                else
                {
                    Response.Cookies.Delete("adminloginKey");
                    Response.Cookies.Delete("adminPersonUserID");
                    Response.Cookies.Delete("adminPersonUserPassword");
                }
                string remoteIpAddress = HttpContext.Connection.RemoteIpAddress.ToString();
                AniBoard_Admin.Utility.Encryption en = new AniBoard_Admin.Utility.Encryption();
                string md5Password = en.encryption(userpassword.Trim());
                AdminUser adminUser = new AdminUser();

                // calling api
                var queryParams = new Dictionary<string, string?>();

                if (!string.IsNullOrEmpty(username))
                    queryParams["userEmail"] = username;

                if (!string.IsNullOrEmpty(userpassword))
                    queryParams["userPassword"] = userpassword;

                var users = await _apiService.GetAsync<ApiResponse<List<AdminUser>>>("Backoffice/AdminUserGet", queryParams);
                
                if (users != null)
                {
                    if (users.Status==true && users.Data.Count>0)
                    {
                        adminUser = users.Data[0];
                        if (adminUser.adminId >0) 
                        {
                            _sessionService.SetUser(adminUser);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        dyLogin.errMsg = "User / Password incorrect";
                        return View(dyLogin);
                    }
                }
                else
                {
                    dyLogin.errMsg = "User / Password incorrect";
                    return View(dyLogin);
                }

            }
            catch (Exception ex)
            {
                int? userId = HttpContext.Session.GetInt32("sessionUserId");
                
            }
            return View(dyLogin);
        }
        #endregion
        #region FAQ
        public async Task<IActionResult> FAQ(string q = null)
        {
            List<FAQ> fAQs = new List<FAQ>();
            dynamic dyFAQ = new ExpandoObject();
            // calling api
            var queryParams = new Dictionary<string, string?>();
            if (!string.IsNullOrEmpty(q))
                queryParams["searchKeyword"] = q;


            var FAQList = await _apiService.GetAsync<ApiResponse<List<FAQ>>>("Backoffice/FAQGet", queryParams);
            if (FAQList != null)
            {
                if (FAQList.Data.Count > 0)
                {
                    fAQs = FAQList.Data;
                }
            }

            dyFAQ.fAQs = fAQs;
            dyFAQ.q = q;
            return View(dyFAQ);
        }

        #endregion
        public IActionResult ContentStatic()
        {
            return View("ContentStatic");
        }
        
        public IActionResult UserPayment()
        {
            return View("UserPayment");
        }
        public IActionResult Dashboard()
        {
            return View("Dashboard");
        }
        //public IActionResult Elements()
        //{
        //    return View("Elements");
        //}
        public IActionResult Background()
        {
            return View("Background");
        }

        
    }


}
