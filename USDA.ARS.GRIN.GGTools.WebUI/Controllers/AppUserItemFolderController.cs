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
        public PartialViewResult _List(int formatCode = 1, int cooperatorId = 0, string isFavorite = null, string timeFrame = "", string isShared = "N")
        {
            AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
            viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            viewModel.SearchEntity.IsFavorite = isFavorite;
            viewModel.SearchEntity.TimeFrame = timeFrame;
            viewModel.SearchEntity.IsShared = isShared;
            viewModel.Search();

            return PartialView("~/Views/AppUserItemFolder/_ListWidget.cshtml", viewModel);
        }
        public ActionResult Edit(int entityId)
        {
            try
            {
                AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel(AuthenticatedUser.CooperatorID);
                viewModel.TableName = "app_user_item_folder";
                viewModel.TableCode = "Folder";
                viewModel.AuthenticatedUser = AuthenticatedUser;

                if (entityId > 0)
                {
                    viewModel.SearchEntity.ID = entityId;
                    viewModel.Get();
                    viewModel.PageTitle = String.Format("Edit Folder: {0}", viewModel.Entity.FolderName);
                
                    //TODO Get list of ID types/tables within the folder.

                
                }
                else
                {
                    viewModel.PageTitle = String.Format("Add Folder");
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        [HttpPost]
        public PartialViewResult Add(AppUserItemFolderViewModel viewModel)
        {
            try
            {
                if (viewModel.Entity.ID == 0)
                {
                    viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Insert();
                }
                else
                {
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }
                viewModel.SearchEntity.ID = viewModel.Entity.ID;
                viewModel.Get();
                viewModel.EventAction = "ADD";
                return PartialView("~/Views/Folder/_Confirmation.cshtml", viewModel);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public ActionResult EditDetails(AppUserItemFolderViewModel viewModel)
        {
            viewModel.Update();
            return RedirectToAction("Edit","AppUserItemFolder", new { @entityId = viewModel.Entity.ID });
        }
        public PartialViewResult AddItems(AppUserItemFolderViewModel viewModel)
        {
            AppUserItemFolderViewModel appUserItemFolderViewModel = new AppUserItemFolderViewModel();
            appUserItemFolderViewModel.SearchEntity.ID = viewModel.Entity.ID;
            appUserItemFolderViewModel.Get();
            appUserItemFolderViewModel.Entity.TableName = viewModel.Entity.TableName;
            appUserItemFolderViewModel.EntityIDList = viewModel.EntityIDList;
            appUserItemFolderViewModel.InsertItems();
            return PartialView("~/Views/Folder/_Confirmation.cshtml", appUserItemFolderViewModel);
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
        [HttpPost]
        public JsonResult DeleteItems(FormCollection coll)
        {
            AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

                if (!String.IsNullOrEmpty(coll["FolderID"]))
                {
                    viewModel.SearchEntity.ID = Int32.Parse(coll["FolderID"]);
                }

                if (!String.IsNullOrEmpty(coll["ItemIDList"]))
                {
                    viewModel.ItemIDList = coll["ItemIDList"].ToString();
                }

                viewModel.Get();
                viewModel.DeleteItems();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}