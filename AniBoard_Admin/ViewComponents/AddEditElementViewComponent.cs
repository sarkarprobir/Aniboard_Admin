using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using Workflow.Data;
using Workflow.Service.Interface;

namespace AniBoard_Admin.ViewComponents
{
    [ViewComponent(Name = "AddEditElement")]
    public class AddEditElementComponent : ViewComponent
    {
        private readonly IAPIService _apiService;
        private readonly IConfiguration _config;
        public AddEditElementComponent(IAPIService apiService, IConfiguration config)
        {
            _apiService = apiService;
            _config = config;
        }

        public async Task<IViewComponentResult> InvokeAsync(int elementId = 0)
        {
            Elementcls elementcls = new Elementcls();
            dynamic dyElement = new ExpandoObject();
            // calling api
            if (elementId > 0)
            {
                var queryParams = new Dictionary<string, string?>();
                if (elementId > 0)
                    queryParams["ElementId"] = elementId.ToString();

                var elementList = await _apiService.GetAsync<ApiResponse<List<Elementcls>>>("Backoffice/ElementGet", queryParams);
                if (elementList != null)
                {
                    if (elementList.Data.Count > 0)
                    {
                        elementcls = elementList.Data.FirstOrDefault();
                    }
                }
            }

            string imagePath = "/dynamicimage/" + _config["ElementFolderName"] + "/";

            //pupulate categorylist
            List<ElementCategory> elementCategoriesList = new List<ElementCategory>();
            var elementCategory= await _apiService.GetAsync<ApiResponse<List<ElementCategory>>>("Backoffice/ElementCategoryGet");
            if (elementCategory != null)
            {
                if (elementCategory.Data.Count > 0)
                {
                    elementCategoriesList= elementCategory.Data.ToList();
                }
            }
            dyElement.elementCategoriesList = elementCategoriesList;
            dyElement.elementcls = elementcls;
            dyElement.elementId = elementId;
            dyElement.imagePath = imagePath;

            return await Task.FromResult((IViewComponentResult)View("AddEditElement", dyElement));
        }

    }
}
