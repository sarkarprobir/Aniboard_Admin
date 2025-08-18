using AniBoard_Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
//using static System.Net.Mime.MediaTypeNames;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing; // For Resize
using System.Diagnostics;
using System.Dynamic;
using Workflow.Data;
using Workflow.Service.Interface;
namespace AniBoard_Admin.Controllers
{
    public class ElementController : Controller
    {
        private readonly IAPIService _apiService;
        private readonly ISessionService _sessionService;
        private readonly IConfiguration _config;
        public ElementController(IAPIService apiService, ISessionService sessionService, IConfiguration config)
        {
            _apiService = apiService;
            _sessionService = sessionService;
            _config = config;
        }
        #region BackgroundImage
        public async Task<IActionResult> BackgroundImage(string q = null, string pageNo = "1")
        {
            string recordPerPage = _config["Recordsperpage"];
            List<BackgroundImage> bgImage = new List<BackgroundImage>();
            dynamic dybgImage = new ExpandoObject();
            // calling api
            var queryParams = new Dictionary<string, string?>();
            queryParams["pageNo"] = pageNo;
            queryParams["recordPerPage"] = recordPerPage;
            if (!string.IsNullOrEmpty(q))
                queryParams["searchKeyword"] = q;


            var bgImageList = await _apiService.GetAsync<ApiResponse<List<BackgroundImage>>>("Backoffice/BackgroundImageGet", queryParams);
            if (bgImageList != null)
            {
                if (bgImageList.Data.Count > 0)
                {
                    bgImage = bgImageList.Data;
                }
            }

            //PaginationHtml start
            int rowsPerpage = Convert.ToInt32(_config["Recordsperpage"]);
            int totalCount = (int)bgImage.FirstOrDefault().TotalCount;
            int iPageno = 0;
            int iTotal = (int)Math.Ceiling((decimal)totalCount / rowsPerpage);
            string paginationHtml = "";
            if (totalCount > rowsPerpage)
            {
                while ( iTotal > iPageno) 
                {
                    iPageno++;
                    if (iPageno==1)
                    {
                        paginationHtml = "<li class='page-item active' id='page_1'><a class='page-link' href='javascript:void(0);' onclick='javascript:gotopage(1);'>" + iPageno + "</a></li>";
                    }
                    else
                    {
                        paginationHtml = paginationHtml + "<li class='page-item' id='page_" + iPageno + "'><a class='page-link' href='javascript:void(0);' onclick='javascript:gotopage(" + iPageno + ");'>" + iPageno + "</a></li>";
                    }
                }
            }
            //end
            dybgImage.paginationHtml = paginationHtml;
            dybgImage.totalCount = iTotal;
            dybgImage.bgImage = bgImage;
            dybgImage.q = q;
            return View(dybgImage);
        }
        public IActionResult ShowBackgroundImagelist(string q = null, string pageNo = "1")
        {
            try
            {
                return ViewComponent("BackgroundImageList", new { q = q , pageNo = pageNo });
            }
            catch (Exception err)
            {
                //_sessionService.SetServerException(err);
                return Redirect("/error/");
            }
        }
        public IActionResult ShowAddEditBackgroundImageModal(int imageId = 0)
        {
            try
            {
                return ViewComponent("AddEditBackgroundImage", new { imageId = imageId });
            }
            catch (Exception err)
            {
                //_sessionService.SetServerException(err);
                return Redirect("/error/");
            }
        }
        [HttpPost()]
        public async Task<IActionResult> SaveBackgroundImage(IFormFile file, int imageId, string customName)
        {
            BackgroundImage bgImage = new BackgroundImage();
            if (file != null )
            {
                if (file.Length > 0)
                {
                    var uploadPath = _config["DynamicImageFolderPath"] + "/" + _config["BackgroudImageFolderName"]; 
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var extension = Path.GetExtension(file.FileName);
                    var filename=Path.GetFileNameWithoutExtension(file.FileName);
                    // ✅ Generate unique base name
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    var baseFileName = !string.IsNullOrWhiteSpace(filename)
                        ? $"{filename}_{timestamp}"
                        : $"{Path.GetFileNameWithoutExtension(file.FileName)}_{timestamp}";

                    var fileName = baseFileName + extension;
                    bgImage.ImageName = fileName;
                    var filePath = Path.Combine(uploadPath, fileName);

                    // ✅ Save original image
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var fileSizeBytes = new FileInfo(filePath).Length;
                    var fileSizeKB = Math.Round((double)fileSizeBytes / 1024, 2);
                    bgImage.ImageSize = fileSizeKB;
                    // ✅ Get dimensions & create thumbnail
                    string thumbFileName = baseFileName + "_thumb" + extension;
                    bgImage.ImageNameThumb = thumbFileName;
                    string thumbFilePath = Path.Combine(uploadPath, thumbFileName);
                    int width, height;

                    using (var image = await Image.LoadAsync(filePath))
                    {
                        width = image.Width;
                        height = image.Height;

                        using (var thumbImage = image.Clone(ctx => ctx.Resize(new Size(50, 50))))
                        {
                            await thumbImage.SaveAsync(thumbFilePath);
                        }
                    }
                    bgImage.ImageH=height;
                    bgImage.ImageW=width;
                }

            }

            bgImage.DisplayName = customName;
            bgImage.ImageId = imageId;

            //return Ok(new
            //{
            //    fileName,
            //    filePath = $"/uploads/{fileName}"
            //});

            var result = await _apiService.PostAsync<BackgroundImage, ApiResponse<BackgroundImage>>("Backoffice/SaveBackgroundImage", bgImage);

            if (result?.Status == true)
            {
                Console.WriteLine("Background Image created: " + result?.Message);
                return Json(new
                {
                    Ok = true,
                    Errors = ""
                });
            }
            else
            {
                Console.WriteLine("Failed to create background image: " + result?.Message);
                return Json(new
                {
                    Ok = false,
                    Errors = result?.Message
                });
            }


        }


        #endregion


    }
}
