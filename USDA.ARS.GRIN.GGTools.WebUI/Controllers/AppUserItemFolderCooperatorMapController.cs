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
    public class AppUserItemFolderCooperatorMapController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // GET: AppUserItemFolderCooperatorMap
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Map()
        { }
        public JsonResult UnMap()
        { }
        public PartialViewResult _List()
        {
            AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();

            viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            // TODO
            viewModel.Search();

            return PartialView("~/Views/AppUserItemFolder/_ListWidget.cshtml", viewModel);
        }
        public PartialViewResult RenderEditModal(int appUserItemFolderId)
        {
            try
            {
                AppUserItemFolderCooperatorMapViewModel viewModel = new AppUserItemFolderCooperatorMapViewModel();
                viewModel.GetMapped();
                viewModel.GetNonMapped();
                return PartialView("~/Views/AppUserItemFolderCooperatorMap/Modals/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderWidget(int appUserItemFolderId)
        {
            try
            {
                AppUserItemFolderCooperatorMapViewModel viewModel = new AppUserItemFolderCooperatorMapViewModel();
                viewModel.SearchEntity.FolderID = appUserItemFolderId;
                viewModel.GetNonMapped();
                viewModel.GetMapped();
                return PartialView("~/Views/AppUserItemFolderCooperatorMap/_Widget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    }
}