using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class AppUserItemListController : BaseController, IController<AppUserItemList>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public PartialViewResult GetTabList()
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
            try
            {
                viewModel.GetTabList(AuthenticatedUser.CooperatorID);

                return PartialView("~/Views/AppUserItemList/Import/_TabList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult GetListsByTab(string tabName)
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
            try
            {
                viewModel.GetListsByTab(AuthenticatedUser.CooperatorID, tabName);
                return PartialView("~/Views/AppUserItemList/Import/_ListsByTab.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult GetItemsByList(string listName)
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
            try
            {
                viewModel.GetItemsByList(AuthenticatedUser.CooperatorID, listName);
                return PartialView("~/Views/AppUserItemList/Import/_ItemsByList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListFolderItems(int folderId)
        {
            try
            {
                return PartialView("~/Views/Shared/_UnderConstruction.cshtml");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(AppUserItemList viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        // GET: AppUserItemList
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Import()
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();

            try
            {
                //TODO Init page with
                // 1) Tabs
                // 2) Lists per tab
                // 3) Data-type groups per list
                return View("~/Views/AppUserItemList/Import/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
    }
}
        //public PartialViewResult Import(FormCollection coll)
        //{
        //    FolderViewModel viewModel = new FolderViewModel();
        //    viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

        //    try
        //    {
        //        viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

        //        if (!String.IsNullOrEmpty(coll["FolderID"]))
        //        {
        //            viewModel.Entity.ID = Int32.Parse(coll["FolderID"]);
        //        }

        //        if (!String.IsNullOrEmpty(coll["IDList"]))
        //        {
        //            viewModel.Entity.ItemIDList = coll["IDList"];
        //        }

        //        if (!String.IsNullOrEmpty(coll["TableName"]))
        //        {
        //            viewModel.Entity.TableName = coll["TableName"];
        //        }

        //        if (!String.IsNullOrEmpty(coll["FolderName"]))
        //        {
        //            viewModel.Entity.FolderName = coll["FolderName"];
        //        }

        //        if (!String.IsNullOrEmpty(coll["FolderType"]))
        //        {
        //            viewModel.Entity.FolderType = coll["FolderType"];
        //        }

        //        if (!String.IsNullOrEmpty(coll["NewCategory"]))
        //        {
        //            viewModel.Entity.Category = coll["NewCategory"];
        //        }
        //        else
        //        {
        //            if (!String.IsNullOrEmpty(coll["Category"]))
        //            {
        //                viewModel.Entity.Category = coll["Category"];
        //            }
        //        }

        //        if (!String.IsNullOrEmpty(coll["Description"]))
        //        {
        //            viewModel.Entity.Description = coll["Description"];
        //        }

        //        if (!String.IsNullOrEmpty(coll["IsFavorite"]))
        //        {
        //            viewModel.Entity.IsFavorite = coll["IsFavorite"];
        //        }

        //        if (viewModel.Entity.ID == 0)
        //        {
        //            viewModel.Insert();
        //        }
        //        else
        //        {
        //            viewModel.Update();
        //        }

        //        // TEST Return new folder as JSON
        //        viewModel.SearchEntity.ID = viewModel.Entity.ID;
        //        viewModel.Search();
        //        viewModel.EventAction = "ADD";
        //        return PartialView("~/Views/Folder/_Confirmation.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}

        public PartialViewResult _List(string tabName = "", int cooperatorId = 0, int appUserItemFolderId = 0)
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
            try
            {
                viewModel.SearchEntity.ListName = tabName;
                viewModel.SearchEntity.CreatedByCooperatorID = cooperatorId;
                viewModel.SearchEntity.AppUserItemFolderID = appUserItemFolderId;
                viewModel.Search();
                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListByFolder(int cooperatorId = 0, int appUserItemFolderId = 0)
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
            try
            {
                viewModel.GetSysTablesByAppUserItemFolder(appUserItemFolderId);
                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult Search(AppUserItemList viewModel)
        {
            throw new NotImplementedException();
        }

        public ActionResult _Lookup(FormCollection formCollection)
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
            try 
            {
                if (!String.IsNullOrEmpty(formCollection["TabName"]))
                {
                    viewModel.SearchEntity.TabName = formCollection["TabName"];
                }
                viewModel.Search();
                return PartialView("~/Views/WebOrder/Dashboard/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
    }
}