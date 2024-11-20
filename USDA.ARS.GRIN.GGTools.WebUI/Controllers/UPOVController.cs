using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using USDA.ARS.GRIN.GGTools.DataLayer.EntityClasses.UPOV;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    public class ApiResponse
    {
        public List<upovCodeItem> upovCodeList { get; set;}   
    }

    [GrinGlobalAuthentication]
    public class UPOVController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly HttpClient _httpClient;

        public UPOVController()
        {
            _httpClient = new HttpClient();
        }
       
        public async Task<ActionResult> Index()
        {
            string apiUrl = "https://www.upov.int/geniews/upovcode/UpovCodeList"; // Replace with the actual API URL
            UPOVViewModel viewModel = new UPOVViewModel();

            try
            {
                var response = await _httpClient.GetStringAsync(apiUrl);
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);
                viewModel.DataCollection = apiResponse.upovCodeList;

                // Pass data to the view
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., logging, showing error messages)
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
    }
}

