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

        public PartialViewResult _List(string tabName = "")
        {
            AppUserItemListViewModel viewModel = new AppUserItemListViewModel();
            try
            {
                viewModel.SearchEntity = new AppUserItemListSearch { ListName = tabName };
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
    }
}