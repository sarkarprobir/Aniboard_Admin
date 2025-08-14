
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using Workflow.Data;
using Workflow.Service.Interface;

namespace AniBoard_Admin.ViewComponents
{
    [ViewComponent(Name = "AddEditBackgroundImage")]
    public class AddEditBackgroundImageComponent: ViewComponent
    {
        private readonly IAPIService _apiService;
        private readonly IConfiguration _config;
        public AddEditBackgroundImageComponent(IAPIService apiService, IConfiguration config)
        {
            _apiService = apiService;
            _config = config;
        }

        public async Task<IViewComponentResult> InvokeAsync(int imageId = 0)
        {
            BackgroundImage bgImage = new BackgroundImage();
            dynamic dybgImage = new ExpandoObject();
            // calling api
            if (imageId>0)
            {
                var queryParams = new Dictionary<string, string?>();
                if (imageId > 0)
                    queryParams["ImageId"] = imageId.ToString();

                var bgImageList = await _apiService.GetAsync<ApiResponse<List<BackgroundImage>>>("Backoffice/BackgroundImageGet", queryParams);
                if (bgImageList != null)
                {
                    if (bgImageList.Data.Count > 0)
                    {
                        bgImage = bgImageList.Data.FirstOrDefault();
                    }
                }
            }
            
            string imagePath = _config["DynamicImageFolderPath"] + "/" + _config["BackgroudImageFolderName"];

            dybgImage.bgImage = bgImage;
            dybgImage.imageId = imageId;
            dybgImage.imagePath = imagePath;

            return await Task.FromResult((IViewComponentResult)View("AddEditBackgroundImage", dybgImage));
        }
    }
}
