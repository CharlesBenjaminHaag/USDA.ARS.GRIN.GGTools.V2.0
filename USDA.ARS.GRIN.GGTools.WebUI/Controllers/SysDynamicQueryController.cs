using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class SysDynamicQueryController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // GET: DynamicQuery
        public ActionResult Index()
        {
            SysDynamicQueryViewModel viewModel = new SysDynamicQueryViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Search(SysDynamicQueryViewModel viewModel)
        {
            viewModel.Search();
            return View("~/Views/SysDynamicQuery/Index.cshtml", viewModel);
        }
    }
}