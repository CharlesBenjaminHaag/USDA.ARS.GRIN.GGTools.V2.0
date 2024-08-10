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
    [GrinGlobalAuthentication]
    public class GOBSController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            viewModel.GetDatasets(AuthenticatedUser.CooperatorID);
            return View(viewModel);
        }

        #region Dataset

        public ViewResult GetDataset(string objectType)
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            SysDynamicQueryViewModel sysDynamicQueryViewModel = new SysDynamicQueryViewModel();
            sysDynamicQueryViewModel.SearchEntity.SQLStatement = "SELECT * FROM get_" + objectType;
            sysDynamicQueryViewModel.Search();

            //viewModel.DataCollectionDataTable = sysDynamicQueryViewModel.DataCollectionDataTable;
            return View("~/Views/GOBS/Edit.cshtml", viewModel);
        }

        public ActionResult AddDataset()
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            return View("~/Views/GOBS/EditDataset.cshtml", viewModel);
        }

        public ActionResult EditDataset(int datasetId)
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            viewModel.GetDataset(AuthenticatedUser.CooperatorID, datasetId);

            if (viewModel.DatasetEntity.authorized == 1)
            {
                ViewBag.PageTitle = "Edit Dataset";
            }
            else
            {
                ViewBag.PageTitle = "View Dataset";
            }

            return View("~/Views/GOBS/EditDataset.cshtml", viewModel);
        }

        //public ActionResult EditDataset(GOBSViewModel viewModel)
        //{ 
        
        //}

        public PartialViewResult GetAll()
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            viewModel.GetDatasets(AuthenticatedUser.CooperatorID);
            return PartialView("~/Views/GOBS/_ListDatasets.cshtml", viewModel);
        }

        public PartialViewResult GetDatasetDetailEditor(int dataSetId)
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            //viewModel.GetDataset(dataSetId);
            viewModel.TableTitle = "GOBS Dataset";
            return PartialView("~/Views/GOBS/_EditDataset.cshtml", viewModel);
        }

        #endregion Dataset

        #region Dataset Marker

        public ActionResult AddDatasetMarker()
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            return View("~/Views/GOBS/EditDatasetMarker.cshtml", viewModel);
        }

        public ActionResult EditDatasetMarker(int datasetMarkerId)
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            viewModel.GetDataSetMarker(AuthenticatedUser.CooperatorID, datasetMarkerId);

            if (viewModel.DatasetEntity.authorized == 1)
            {
                ViewBag.PageTitle = "Edit Dataset Marker";
            }
            else
            {
                ViewBag.PageTitle = "View Dataset Marker";
            }

            return View("~/Views/GOBS/EditDatasetMarker.cshtml", viewModel);
        }

        #endregion 

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

            try
            {
                viewModel.SearchEntity.SQLStatement = "SELECT * FROM get_gobs_" + objectType + " WHERE gobs_dataset_id = " + entityId.ToString();
                viewModel.Search();
                viewModel.TableName = objectType;
                return PartialView("~/Views/GOBS/_DependentDataList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
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