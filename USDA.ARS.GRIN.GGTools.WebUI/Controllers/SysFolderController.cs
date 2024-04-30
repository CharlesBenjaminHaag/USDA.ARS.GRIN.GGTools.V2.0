using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class SysFolderController : Controller
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // GET: SysFolder
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetEditModal()
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                return PartialView("~/Views/SysFolder/Modals/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}