using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class SiteController : BaseController, IController<SiteViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }
        public ActionResult Add()
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            SiteViewModel viewModel = new SiteViewModel();
            viewModel.TableName = "site";
            viewModel.Get(entityId);
            viewModel.PageTitle = String.Format("Edit Site [{0}]", viewModel.Entity.ID);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(SiteViewModel viewModel)
        {
            try
            {
                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return View(viewModel);
                }

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
                return RedirectToAction("Edit", "Site", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Index()
        {
            SiteViewModel viewModel = new SiteViewModel();
            viewModel.PageTitle = "Site Search";
            viewModel.TableName = "site";
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Search(SiteViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();
                return View("~/Views/Site/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _List()
        {
            //TODO
            return null;
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult RenderWidget(int siteId)
        {
            SiteViewModel viewModel = new SiteViewModel();
            try
            {
                viewModel.Get(siteId);
                return PartialView("~/Views/Site/_Widget.cshtml", viewModel);
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