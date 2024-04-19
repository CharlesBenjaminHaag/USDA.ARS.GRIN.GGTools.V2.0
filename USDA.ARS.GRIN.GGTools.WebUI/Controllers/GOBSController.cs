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
            viewModel.GetSysTables(AuthenticatedUser.SysUserID, "GOBS");
            return View(viewModel);
        }

        public ViewResult Get(string objectType)
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            SysDynamicQueryViewModel sysDynamicQueryViewModel = new SysDynamicQueryViewModel();
            sysDynamicQueryViewModel.SearchEntity.SQLStatement = "SELECT * FROM get_" + objectType;
            sysDynamicQueryViewModel.Search();

            viewModel.DataCollectionDataTable = sysDynamicQueryViewModel.DataCollectionDataTable;
            return View("~/Views/GOBS/Edit.cshtml", viewModel);
        }

        public ViewResult GetAll(string objectType, string objectTitle)
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            SysDynamicQueryViewModel sysDynamicQueryViewModel = new SysDynamicQueryViewModel();
            sysDynamicQueryViewModel.SearchEntity.SQLStatement = "SELECT * FROM get_" + objectType;
            sysDynamicQueryViewModel.Search();

            viewModel.DataCollectionDataTable = sysDynamicQueryViewModel.DataCollectionDataTable;
            viewModel.TableName = objectTitle;
            return View("~/Views/GOBS/EditDependentData.cshtml", viewModel);
        }

        public PartialViewResult GetDatasetDetailEditor(int dataSetId)
        {
            GOBSViewModel viewModel = new GOBSViewModel();
            viewModel.GetDataset(dataSetId);
            viewModel.TableTitle = "GOBS Dataset";
            return PartialView("~/Views/GOBS/_EditDataset.cshtml", viewModel);
        }

        public PartialViewResult GetDetailEditor(int entityId, string objectType)
        {
            string partialViewName = String.Empty;
            GOBSViewModel viewModel = new GOBSViewModel();
            viewModel.GetDataset(entityId);

            switch(objectType)
            {
                case "admin":
                    break;
                case "dataset_attach":
                    partialViewName = "~/Views/GOBS/_EditDatasetAttach.cshtml";
                    break;
                case "dataset_cooperator":
                    partialViewName = "~/Views/GOBS/_EditDatasetCooperator.cshtml";
                    break;
                case "dataset_field":
                    partialViewName = "~/Views/GOBS/_EditDatasetField.cshtml";
                    break;
                case "dataset_inventory":
                    partialViewName = "~/Views/GOBS/_EditDatasetInventory.cshtml";
                    break;
                case "dataset_marker":
                    partialViewName = "~/Views/GOBS/_EditDatasetMarker.cshtml";
                    break;
                case "dataset_marker_field":
                    partialViewName = "~/Views/GOBS/_EditDatasetMarkerField.cshtml";
                    break;
                case "dataset_marker_value":
                    partialViewName = "~/Views/GOBS/_EditDatasetMarkerValue.cshtml";
                    break;
                case "dataset_value":
                    partialViewName = "~/Views/GOBS/_EditDatasetValue.cshtml";
                    break;
            }
            return PartialView(partialViewName, viewModel);
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