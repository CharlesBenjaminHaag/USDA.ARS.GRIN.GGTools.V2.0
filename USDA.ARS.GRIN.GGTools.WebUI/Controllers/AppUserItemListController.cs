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

        public PartialViewResult _List(string tabName = "", int cooperatorId = 0)
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
            try
            {
                viewModel.SearchEntity.ListName = tabName;
                viewModel.SearchEntity.CreatedByCooperatorID = cooperatorId;
                viewModel.Search();
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