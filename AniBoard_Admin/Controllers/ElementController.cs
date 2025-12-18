using AniBoard_Admin.Models;
using AniBoard_Admin.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
//using static System.Net.Mime.MediaTypeNames;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing; // For Resize
using System.Diagnostics;
using System.Dynamic;
using Workflow.Data;
using Workflow.Service.Interface;
using static System.Net.Mime.MediaTypeNames;
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
            string paginationHtml = "";
            int iTotal = 0;
            if (bgImage.Count > 0)
            {
                //PaginationHtml start
                int rowsPerpage = Convert.ToInt32(_config["Recordsperpage"]);
                int totalCount = (int)bgImage.FirstOrDefault().TotalCount;
                int iPageno = 0;
                iTotal = (int)Math.Ceiling((decimal)totalCount / rowsPerpage);
                
                if (totalCount > rowsPerpage)
                {
                    while (iTotal > iPageno)
                    {
                        iPageno++;
                        if (iPageno == 1)
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
            }


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
        public async Task<IActionResult> SaveBackgroundImage(IFormFile file, int imageId, string customName, int isDelete = 0)
        {
            try
            {
                BackgroundImage bgImage = new BackgroundImage();
                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        var uploadPath = _config["DynamicImageFolderPath"] + "/" + _config["BackgroudImageFolderName"];
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        var extension = Path.GetExtension(file.FileName);
                        var filename = Path.GetFileNameWithoutExtension(file.FileName);
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

                        using (var image = await SixLabors.ImageSharp.Image.LoadAsync(filePath))
                        {
                            width = image.Width;
                            height = image.Height;

                            using (var thumbImage = image.Clone(ctx => ctx.Resize(new Size(50, 50))))
                            {
                                await thumbImage.SaveAsync(thumbFilePath);
                            }
                        }
                        bgImage.ImageH = height;
                        bgImage.ImageW = width;
                    }

                }

                bgImage.DisplayName = customName;
                bgImage.ImageId = imageId;
                if (isDelete > 0)
                {
                    bgImage.isDelete = 1;
                }
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

            catch (Exception err)
            {
                return Json(new
                {
                    Ok = false,
                    Errors = err.Message
                });
            }


        }


        #endregion
        #region Element
        public async Task<IActionResult> Elements(string q = null, string categoryId = null, string pageNo = "1")
        {
            string recordPerPage = _config["Recordsperpage"];
            List<Elementcls> elementcls = new List<Elementcls>();
            dynamic dyElement = new ExpandoObject();
            // calling api
            var queryParams = new Dictionary<string, string?>();
            queryParams["pageNo"] = pageNo;
            queryParams["recordPerPage"] = recordPerPage;
            if (!string.IsNullOrEmpty(q))
                queryParams["searchKeyword"] = q;
            if (!string.IsNullOrEmpty(categoryId))
                queryParams["categoryId"] = categoryId;

            var elementclsList = await _apiService.GetAsync<ApiResponse<List<Elementcls>>>("Backoffice/ElementGet", queryParams);
            if (elementclsList != null)
            {
                if (elementclsList.Data.Count > 0)
                {
                    elementcls = elementclsList.Data;
                }
            }
            string paginationHtml = "";
            
            int totalCount = 0;
            int rowsPerpage = Convert.ToInt32(_config["Recordsperpage"]);
            if (elementcls.Count>0)
            {
                totalCount = (int)elementcls.FirstOrDefault().TotalCount;
            }

            List<ElementCategory> elementCategoriesList = new List<ElementCategory>();
            var elementCategory = await _apiService.GetAsync<ApiResponse<List<ElementCategory>>>("Backoffice/ElementCategoryGet");
            if (elementCategory != null)
            {
                if (elementCategory.Data.Count > 0)
                {
                    elementCategoriesList = elementCategory.Data.ToList();
                }
            }

            dyElement.elementCategoriesList = elementCategoriesList;
            dyElement.totalCount = totalCount;
            //dyElement.rowsPerpage = rowsPerpage;
            dyElement.elementcls = elementcls;
            dyElement.q = q;
            //dyElement.categoryId = 0;
            //if (!string.IsNullOrEmpty(categoryId))
            //{
            //    dyElement.categoryId = Convert.ToInt32(categoryId);
            //}
            if (int.TryParse(_config["Recordsperpage"], out int parsedRows))
            {
                rowsPerpage = parsedRows;
            }

            int parsedCategoryId = 0;
            if (!string.IsNullOrWhiteSpace(categoryId) && int.TryParse(categoryId, out int cid))
            {
                parsedCategoryId = cid;
            }

            dyElement.rowsPerpage = rowsPerpage;
            dyElement.categoryId = parsedCategoryId;

            return View(dyElement);
        }
        public IActionResult ShowElementlist(string q = null, string catId = null, string pageNo = "1")
        {
            try
            {
                return ViewComponent("ElementList", new { q = q, categoryId=catId, pageNo = pageNo });
            }
            catch (Exception err)
            {
                //_sessionService.SetServerException(err);
                return Redirect("/error/");
            }
        }
        public IActionResult ShowAddEditElementModal(int elementId = 0)
        {
            try
            {
                return ViewComponent("AddEditElement", new { elementId = elementId });
            }
            catch (Exception err)
            {
                //_sessionService.SetServerException(err);
                return Redirect("/error/");
            }
        }

        [HttpPost()]
        public async Task<IActionResult> SaveElement(IFormFile file, int elementId, string elementName, int categoryId, string imageTag = null, int isDelete = 0)
        {
            try
            {
                Elementcls elementcls = new Elementcls();
                int width = 0;
                int height = 0;
                if (file != null)
                {

                    if (file.Length > 0)
                    {
                        var uploadPath = _config["DynamicImageFolderPath"] + "/" + _config["ElementFolderName"];
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }

                        var extension = Path.GetExtension(file.FileName);
                        var filename = Path.GetFileNameWithoutExtension(file.FileName);
                        // ✅ Generate unique base name
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        var baseFileName = !string.IsNullOrWhiteSpace(filename)
                            ? $"{filename}_{timestamp}"
                            : $"{Path.GetFileNameWithoutExtension(file.FileName)}_{timestamp}";

                        var fileName = baseFileName + extension;
                        elementcls.ImageName = fileName;
                        var filePath = Path.Combine(uploadPath, fileName);

                        // ✅ Save original image
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        var fileSizeBytes = new FileInfo(filePath).Length;
                        var fileSizeKB = Math.Round((double)fileSizeBytes / 1024, 2);
                        elementcls.ImageSize = fileSizeKB;
                        // ✅ Get dimensions & create thumbnail
                        string thumbFileName = baseFileName + "_thumb" + extension;
                        elementcls.ImageNameThumb = thumbFileName;
                        string thumbFilePath = Path.Combine(uploadPath, thumbFileName);

                        (width, height) = ImageHelper.GetImageSize(filePath);
                        if (extension == ".ico" || extension == ".svg")
                        {
                            // Just copy original file as thumbnail
                            System.IO.File.Copy(filePath, thumbFilePath, overwrite: true);
                        }
                        else
                        {
                            //using (var image = await SixLabors.ImageSharp.Image.LoadAsync(filePath))
                            //{
                            //    width = image.Width;
                            //    height = image.Height;

                            //    using (var thumbImage = image.Clone(ctx => ctx.Resize(new Size(50, 50))))
                            //    {
                            //        await thumbImage.SaveAsync(thumbFilePath);
                            //    }
                            //}
                            using (var image = await SixLabors.ImageSharp.Image.LoadAsync(filePath))
                            {
                                width = image.Width;
                                height = image.Height;

                                var options = new ResizeOptions
                                {
                                    Mode = ResizeMode.Max,       // proportional fit
                                    Size = new Size(150, 150),   // max box
                                    Sampler = KnownResamplers.Lanczos3, // good quality
                                    Compand = true               // better colors when resizing
                                };

                                using (var thumbImage = image.Clone(ctx => ctx.Resize(options)))
                                {
                                    await thumbImage.SaveAsync(thumbFilePath);
                                }
                            }
                        }

                    }

                }
                if (elementId > 0)
                {
                    elementcls.ElementId = elementId;
                }
                elementcls.ImageH = height;
                elementcls.ImageW = width;
                elementcls.CategoryId = categoryId;
                elementcls.ElementName = elementName;
                elementcls.ElementId = elementId;
                elementcls.ImageTag = imageTag;
                if (isDelete > 0)
                {
                    elementcls.isDelete = 1;
                }

                var result = await _apiService.PostAsync<Elementcls, ApiResponse<Elementcls>>("Backoffice/SaveElement", elementcls);

                if (result?.Status == true)
                {
                    Console.WriteLine("Element Image created: " + result?.Message);
                    return Json(new
                    {
                        Ok = true,
                        Errors = ""
                    });
                }
                else
                {
                    Console.WriteLine("Failed to create element image: " + result?.Message);
                    return Json(new
                    {
                        Ok = false,
                        Errors = result?.Message
                    });
                }
            }
            catch (Exception err)
            {
                return Json(new
                {
                    Ok = false,
                    Errors = err?.Message
                });
            }



        }



        public void CreateThumbnails()
        {
            string folderPath = _config["DynamicImageFolderPath"] + "/" + _config["ElementFolderName"]; 
            int maxSize = 150;


            //var files = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
            //                     .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
            //                                 f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
            //                                 f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
            //                                 f.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
            //                                 f.EndsWith(".gif", StringComparison.OrdinalIgnoreCase));
            var files = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly)
                             .Where(f =>
                                (f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                 f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                 f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                 f.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                 f.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)) &&
                                !Path.GetFileNameWithoutExtension(f).Contains("_thumb", StringComparison.OrdinalIgnoreCase) // ✅ skip thumbs
                             );

            foreach (var file in files)
            {
                using (var image = SixLabors.ImageSharp.Image.Load(file))
                {
                    var options = new ResizeOptions
                    {
                        Mode = ResizeMode.Max,       // keep aspect ratio
                        Size = new Size(maxSize, maxSize),
                        Sampler = KnownResamplers.Lanczos3,
                        Compand = true
                    };

                    using (var thumbImage = image.Clone(ctx => ctx.Resize(options)))
                    {
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                        string extension = Path.GetExtension(file);
                        string thumbPath = Path.Combine(folderPath, $"{fileNameWithoutExt}_thumb{extension}");

                        if (extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                            extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
                        {
                            thumbImage.Save(thumbPath, new JpegEncoder { Quality = 85 });
                        }
                        else if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase))
                        {
                            thumbImage.Save(thumbPath, new PngEncoder { CompressionLevel = PngCompressionLevel.DefaultCompression });
                        }
                        else
                        {
                            thumbImage.Save(thumbPath); // fallback for bmp/gif
                        }
                    }
                }
            }
        }

        #endregion

    }
}
