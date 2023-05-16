using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class SiteController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public PartialViewResult _Get(int entityId)
        {
            try
            {
                SiteViewModel viewModel = new SiteViewModel();
                viewModel.Get(entityId);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                return PartialView("~/Views/Site/_EditAddress.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult Save(CooperatorViewModel viewModel)
        {
            try
            {
                //if (!viewModel.Validate())
                //{
                //    if (viewModel.ValidationMessages.Count > 0) return View(viewModel);
                //}

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
                return _Get(viewModel.Entity.ID);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }










        // ******************************************************************************
        // BEGIN OLD CODE
        // ******************************************************************************

        //public PartialViewResult _ListFolderItems(int folderId)
        //{
        //    try
        //    {
        //        return PartialView("~/Views/Shared/_UnderConstruction.cshtml");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}
        //public ActionResult Delete(int entityId)
        //{
        //    throw new NotImplementedException();
        //}
        //public ActionResult Add()
        //{
        //    throw new NotImplementedException();
        //}

        //public ActionResult Edit(int entityId)
        //{
        //    SiteViewModel viewModel = new SiteViewModel();
        //    viewModel.TableName = "site";
        //    viewModel.Get(entityId);
        //    viewModel.PageTitle = String.Format("Edit Site [{0}]", viewModel.Entity.ID);
        //    return View(viewModel);
        //}

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

        public PartialViewResult _Edit(int entityId)
        {
            try
            {
                SiteViewModel viewModel = new SiteViewModel();
                viewModel.Get(entityId);
                return PartialView("~/Views/Site/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        //[HttpPost]
        //public JsonResult _Get(int siteId)
        //{
        //    SiteViewModel viewModel = new SiteViewModel();
        //    try
        //    {
        //        viewModel.Get(siteId);
        //        return Json(new { site = viewModel.Entity }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message);
        //        return Json(new { site = viewModel.Entity }, JsonRequestBehavior.AllowGet);
        //    }
        //}

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

        //public PartialViewResult FolderItems(FormCollection formCollection)
        //{
        //    throw new NotImplementedException();
        //}

        public PartialViewResult RenderWidget(int siteId)
        {
            SiteViewModel viewModel = new SiteViewModel();
            try
            {
                viewModel.Get(siteId);

                // Get a list of all active cooperators at the specified site.
                //CooperatorViewModel cooperatorViewModel = new CooperatorViewModel();
                //cooperatorViewModel.SearchEntity.SiteID = siteId;
                //cooperatorViewModel.SearchEntity.SysUserIsEnabled = "Y";
                //cooperatorViewModel.SearchEntity.StatusCode = "ACTIVE";
                //cooperatorViewModel.Search();
                
                //viewModel.DataCollectionSiteCooperators = cooperatorViewModel.DataCollection;
                viewModel.AuthenticatedUser = AuthenticatedUser;

                return PartialView("~/Views/Site/_Widget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        //public PartialViewResult RenderListWidget()
        //{
        //    SiteViewModel viewModel = new SiteViewModel();
        //    try
        //    {
        //        viewModel.Search();
        //        return PartialView("~/Views/Site/_ListWidget.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
    }
}