using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class AccessionController : Controller
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // GET: Accession
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult _List(int speciesId = 0)
        {
            try
            {
                AccessionViewModel viewModel = new AccessionViewModel();
                viewModel.SearchEntity.SpeciesID = speciesId;
                viewModel.Search();
                return PartialView("~/Views/Accession/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    }
}