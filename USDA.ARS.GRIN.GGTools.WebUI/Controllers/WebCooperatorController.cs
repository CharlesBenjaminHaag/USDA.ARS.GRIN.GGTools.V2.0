using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class WebCooperatorController : BaseController, IController<CooperatorViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
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

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult _Edit(int entityId, string environment = "")
        {
            try
            {
                WebCooperatorViewModel viewModel = new WebCooperatorViewModel();
                viewModel.Get(entityId, environment);
                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public ActionResult Edit(int entityId)
        {
            try
            {
                WebCooperatorViewModel viewModel = new WebCooperatorViewModel();
                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit Web Cooperator [{0}]: {1}", entityId, viewModel.Entity.FullName);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(CooperatorViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        // GET: WebCooperator
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(CooperatorViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}