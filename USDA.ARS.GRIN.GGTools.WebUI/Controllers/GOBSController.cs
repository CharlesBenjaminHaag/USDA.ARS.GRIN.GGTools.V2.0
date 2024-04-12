using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.GOBS.ViewModelLayer;
using NLog;
using System.Security.Permissions;
using DataTables;
using System.Linq.Expressions;
using System.Runtime.Remoting.Channels;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class GOBSController : Controller
    {
        // GET: GOBS
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult Get(string objectType)
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            SysDynamicQueryViewModel sysDynamicQueryViewModel = new SysDynamicQueryViewModel();
            sysDynamicQueryViewModel.SearchEntity.SQLStatement = "SELECT * FROM get_gobs_" + objectType;
            sysDynamicQueryViewModel.Search();

            viewModel.DataCollectionDataTable = sysDynamicQueryViewModel.DataCollectionDataTable;
            return View("~/Views/GOBS/Edit.cshtml", viewModel);
        }

        public PartialViewResult GetDatasetDetailEditor(int entityId)
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            viewModel.Get(entityId);
            return PartialView("~/Views/GOBS/_EditDataset.cshtml", viewModel);
        }

        public PartialViewResult GetDatasetFieldEditor(int entityId)
        {
            //TODO
            return null;
        }

        public PartialViewResult GetDatasetInventoryEditor(int entityId)
        {
            //TODO
            return null;
        }

        public PartialViewResult GetDatasetValueEditor(int entityId)
        {
            //TODO
            return null;
        }

        public PartialViewResult GetDatasetMarkerEditor(int entityId)
        {
            //TODO
            return null;
        }

        public PartialViewResult GetDatasetMarkerFieldEditor(int entityId)
        {
            //TODO
            return null;
        }

        public PartialViewResult GetDatasetMarkerValueEditor(int entityId)
        {
            //TODO
            return null;
        }

        

        public PartialViewResult GetDependentData(int entityId, string objectType)
        {
            SysDynamicQueryViewModel viewModel = new SysDynamicQueryViewModel();
            viewModel.SearchEntity.SQLStatement = "SELECT * FROM get_gobs_" + objectType + " WHERE gobs_dataset_id = " + entityId.ToString();
            viewModel.Search();
            return PartialView("~/Views/SysDynamicQuery/_SearchResultsList.cshtml", viewModel);
        }

        public PartialViewResult GetDatasets()
        {
            SysDynamicQueryViewModel viewModel = new SysDynamicQueryViewModel();
            viewModel.SearchEntity.SQLStatement = "SELECT * FROM get_gobs_dataset";
            viewModel.Search();
            return PartialView("~/Views/SysDynamicQuery/_SearchResultsList.cshtml", viewModel);
        }

        public PartialViewResult GetDataSetFields()
        {
            //TODO
            return null;
        }

        public PartialViewResult RenderSidebar()
        {
            return PartialView("~/Views/Shared/Sidebars/_MainSidebarGOBS.cshtml");
        }
    }
}