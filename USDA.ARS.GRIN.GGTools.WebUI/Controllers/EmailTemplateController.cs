using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class EmailTemplateController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // GET: EmailTemplate
        public ActionResult Index()
        {
            EmailTemplateViewModel viewModel = new EmailTemplateViewModel();

            try
            {
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.Search();
                viewModel.PageTitle = "Email Templates";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }

        }


        public PartialViewResult View(int entityId)
        {
            EmailTemplateViewModel viewModel = new EmailTemplateViewModel();
            try
            {
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.PageTitle = String.Format("Edit Email Template [{0}]", entityId);
                viewModel.Get(entityId);
                return PartialView("~/Views/EmailTemplate/_View.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml", "Error");
            }
        }

        public ActionResult Edit(int entityId)
        {
            EmailTemplateViewModel viewModel = new EmailTemplateViewModel();
            try
            {
                viewModel.PageTitle = String.Format("Edit Email Template [{0}]", entityId);
                viewModel.Get(entityId);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(EmailTemplateViewModel viewModel)
        {
            try
            {
                viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Update();

                viewModel.Get(viewModel.Entity.ID);
                return View("~/Views/EmailTemplate/Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _ListFolderItems(int sysFolderId)
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
        
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }
     
        public ActionResult Search(EmailTemplateViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
    }
}