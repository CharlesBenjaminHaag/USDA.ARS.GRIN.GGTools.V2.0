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
        public PartialViewResult _List(int formatCode = 1, int cooperatorId = 0, string folderType = "", string isFavorite = null, string timeFrame = "", string isShared = "N")
        {
           AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
             
            viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            viewModel.SearchEntity.IsFavorite = isFavorite;
            viewModel.SearchEntity.TimeFrame = timeFrame;
            viewModel.SearchEntity.IsShared = isShared;
            viewModel.SearchEntity.FolderType = folderType;
            viewModel.Search();

            return PartialView("~/Views/AppUserItemFolder/_ListWidget.cshtml", viewModel);
        }
        public ActionResult Edit(int entityId)
        {
            try
            {
                AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel(AuthenticatedUser.CooperatorID);
                viewModel.TableName = "app_user_item_folder";
                viewModel.TableCode = "AppUserItemFolder";
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
                return PartialView("~/Views/AppUserItemFolder/_Confirmation.cshtml", viewModel);

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
            return PartialView("~/Views/AppUserItemFolder/_Confirmation.cshtml", appUserItemFolderViewModel);
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
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult RenderFolderItemDeleteModal(int entityId)
        {
            try
            {
                FolderViewModel viewModel = new FolderViewModel();
                viewModel.SearchEntity.ID = entityId;
                viewModel.Get();
                return PartialView("~/Views/AppUserItemFolder/Modals/_BatchDelete.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderFolderCooperatorEditModal(int entityId)
        {
            try
            {
                FolderViewModel viewModel = new FolderViewModel();
                viewModel.Entity.ID = entityId;
                viewModel.GetAvailableCooperators();
                viewModel.GetCurrentCooperators();
                return PartialView("~/Views/AppUserItemFolder/Cooperator/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderFolderCooperatorWidget(int folderId)
        {
            FolderViewModel viewModel = new FolderViewModel();
            viewModel.Entity.ID = folderId;
            viewModel.GetCurrentCooperators();
            return PartialView("~/Views/AppUserItemFolder/Cooperator/_Widget.cshtml", viewModel);
        }
        public PartialViewResult RenderRelatedFoldersMenu(string sysTableName, int entityId = 0)
        {
            AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();

            try
            {
                if (String.IsNullOrEmpty(sysTableName))
                {
                    throw new IndexOutOfRangeException("Error in FolderController._RelatedFoldersMenu(): Table name not specified.");
                }

                if (entityId == 0)
                {
                    throw new IndexOutOfRangeException("Error in FolderController._RelatedFoldersMenu(): ID field not specified.");
                }

                viewModel.Entity.ID = entityId;
                viewModel.TableName = sysTableName;
                viewModel.SearchEntity.TableName = sysTableName;
                viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.GetRelatedFolders();
                return PartialView("~/Views/AppUserItemFolder/_RelatedFoldersMenu.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        #region Dynamic Folder

        public PartialViewResult RenderDynamicFolderEditModal()
        {
            try
            {
                AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel(AuthenticatedUser.CooperatorID);
                return PartialView("~/Views/AppUserItemFolder/Modals/_EditDynamic.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        //public PartialViewResult AddDynamicFolder(AppUserItemFolderViewModel viewModel)
        //{
        //    try
        //    {
        //        if (viewModel.Entity.ID == 0)
        //        {
        //            viewModel.Entity.FolderType = "DYNAMIC";
        //            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
        //            viewModel.Insert();
        //        }
        //        else
        //        {
        //            viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
        //            viewModel.Update();
        //        }
        //        viewModel.SearchEntity.ID = viewModel.Entity.ID;
        //        viewModel.Get();
        //        viewModel.EventAction = "ADD";
        //        return PartialView("~/Views/AppUserItemFolder/_ConfirmationDynamic.cshtml", viewModel);

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}
        public PartialViewResult _ListDynamic(int formatCode = 1, int cooperatorId = 0, string folderType = "", string dataType = "", string isFavorite = null, string timeFrame = "", string isShared = "N")
        {
            AppUserItemFolderViewModel viewModel = new AppUserItemFolderViewModel();
            try
            {
                viewModel.GetDynamicFolders(cooperatorId, dataType);
                return PartialView("~/Views/AppUserItemFolder/_ListWidgetDynamic.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        #endregion
    }
}