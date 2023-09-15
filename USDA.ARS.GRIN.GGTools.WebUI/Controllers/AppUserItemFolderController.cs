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
    public class AppUserItemFolderController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult Edit(AppUserItemFolderViewModel viewModel)
        {
            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                
                if (viewModel.Entity.ID == 0)
                {
                    viewModel.Insert();
                }
                else
                {
                    viewModel.Update();
                }
                viewModel.SearchEntity.ID = viewModel.Entity.ID;
                //viewModel.Search();
                viewModel.EventAction = "ADD";
                return PartialView("~/Views/Folder/_Confirmation.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderEditModal(string sysTableName)
        {
            try
            {
                AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel(AuthenticatedUser.CooperatorID);

                if (String.IsNullOrEmpty(sysTableName))
                {
                    throw new IndexOutOfRangeException("Table name not specified.");
                }

                //viewModel.GetFolderCategories(AuthenticatedUser.CooperatorID);
                //viewModel.GetAvailableFolders(AuthenticatedUser.CooperatorID, tableName);
                return PartialView("~/Views/AppUserItemFolder/Modals/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    }
}