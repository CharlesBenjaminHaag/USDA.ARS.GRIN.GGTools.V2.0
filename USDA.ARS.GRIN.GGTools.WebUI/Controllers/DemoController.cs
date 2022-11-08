using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Explorer()
        {
            TaxonomyExplorerViewModel viewModel = new TaxonomyExplorerViewModel();
            GenusViewModel viewModelGenus = new GenusViewModel();
            FamilyMapViewModel viewModelFamily = new FamilyMapViewModel();
            SpeciesViewModel viewModelSpecies = new SpeciesViewModel();
           
            viewModelGenus.SearchEntity.FullName = "Poa";
            viewModelGenus.Search();
            viewModel.DataCollectionGenus = viewModelGenus.DataCollection;
            viewModel.SpeciesViewModel = viewModelSpecies;
            return View("~/Views/Demo/Explorer.cshtml", viewModel);
        }
    }
}