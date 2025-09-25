using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using Workflow.Data;
using Workflow.Service.Interface;
namespace AniBoard_Admin.ViewComponents
{
    [ViewComponent(Name = "ElementList")]
    public class ElementListComponent: ViewComponent
    {
        private readonly IAPIService _apiService;
        private readonly IConfiguration _config;
        public ElementListComponent(IAPIService apiService, IConfiguration config)
        {
            _apiService = apiService;
            _config = config;
        }


        public async Task<IViewComponentResult> InvokeAsync(string q = null, string categoryId = null, string pageNo = "1")
        {
            string recordPerPage = _config["Recordsperpage"];
            List<Elementcls> elementcls = new List<Elementcls>();
            dynamic dyElement= new ExpandoObject();
            // calling api
            var queryParams = new Dictionary<string, string?>();
            queryParams["pageNo"] = pageNo;
            queryParams["recordPerPage"] = recordPerPage;
            if (!string.IsNullOrEmpty(q))
                queryParams["searchKeyword"] = q;
            if (!string.IsNullOrEmpty(categoryId))
                queryParams["categoryId"] = categoryId;

            var elementList = await _apiService.GetAsync<ApiResponse<List<Elementcls>>>("Backoffice/ElementGet", queryParams);
            if (elementList != null)
            {
                if (elementList.Data.Count > 0)
                {
                    elementcls = elementList.Data;
                }
            }
            //string imagePath= _config["DynamicImageFolderPath"] + "/" + _config["BackgroudImageFolderName"] + "/";
            string imagePath = "/dynamicimage/" + _config["ElementFolderName"] + "/";
            dyElement.elementcls = elementcls;
            dyElement.q = q;
            dyElement.pageNo = pageNo;
            dyElement.recordPerPage = recordPerPage;
            dyElement.imagePath = imagePath;

            return await Task.FromResult((IViewComponentResult)View("ElementList", dyElement));
        }
    }
}
