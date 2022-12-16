using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class WebOrderController : BaseController, IController<WebOrderRequestViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // *********************************************************************************
        // EXPLORER
        // *********************************************************************************
        public ActionResult Explorer()
        {
            return View("~/Views/WebOrder/Explorer/Index.cshtml");
        }

        public PartialViewResult ExplorerSelectList(int entityId = 0)
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.SearchEntity.ID = entityId;
                viewModel.Search();
                if (viewModel.RowsAffected == 0)
                {
                    return PartialView("~/Views/Error/RecordNotFound.cshtml");
                }
                else
                {
                    return PartialView("~/Views/WebOrder/Explorer/_List.cshtml");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult ExplorerDetail(int entityId)
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.Get(entityId);

                if (viewModel.RowsAffected == 0)
                {
                    return PartialView("~/Views/Error/RecordNotFound.cshtml");
                }
                else
                {
                    return PartialView("~/Views/WebOrder/Explorer/_Detail.cshtml", viewModel);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult ExplorerTimeline(int entityId)
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.SearchEntity.ID = entityId;
                viewModel.Search();
                if (viewModel.RowsAffected == 0)
                {
                    return PartialView("~/Views/Error/RecordNotFound.cshtml");
                }
                else
                {
                    return PartialView("~/Views/WebOrder/Explorer/_Timeline.cshtml");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        //public PartialViewResult RenderApprovalModal()
        //{ 
        
        //}
        //public PartialViewResult RenderRejectionModal()
        //{ 
        
        //}

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
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }
        public ActionResult View(int entityId)
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.Get(entityId);
                if (viewModel.Entity.ID == 0)
                {
                    return View("~/Views/Error/RecordNotFound.cshtml");                
                }

                viewModel.PageTitle = String.Format("View Web Order [{0}]", entityId);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUserWebUserID = AuthenticatedUser.WebUserID;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Board()
        {
            return View();
        }

        public ActionResult Edit(int entityId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Edit(WebOrderRequestViewModel viewModel)
        {
            try 
            { 
                viewModel.Entity.WebUserID = AuthenticatedUser.WebUserID;
                viewModel.Entity.StatusCode = viewModel.EventValue;
                viewModel.Entity.OwnedByWebUserID = AuthenticatedUser.WebUserID;
                viewModel.Entity.Note = viewModel.EventNote;
                viewModel.Update();
                return RedirectToAction("View", "WebOrder", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public JsonResult BatchEdit(FormCollection formCollection)
        {
            try 
            {
                if (!String.IsNullOrEmpty(formCollection["IDList"]))
                {
                   var DEBUG = formCollection["IDList"];
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Index()
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.PageTitle = "Dashboard";
                return View("~/Views/WebOrder/Dashboard.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        //public ActionResult Search()
        //{
        //    try
        //    {
        //        WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
        //        viewModel.PageTitle = "Web Order Search";
        //        return View("~/Views/WebOrder/Index.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return RedirectToAction("InternalServerError", "Error");
        //    }
        //}

        public ActionResult Lookup(int entityId = 0)
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.SearchEntity.ID = entityId;
                viewModel.Search();
                if (viewModel.RowsAffected == 0)
                {
                    return View("~/Views/Error/RecordNotFound.cshtml");
                }
                else
                {
                    return RedirectToAction("View","WebOrder", new { entityId = viewModel.Entity.ID });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Search(int id=0, string status="", string requestorName="", string emailAddress="", string organization="", string intendedUseCode="")
        {
            WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();

            try
            {
                if (id > 0)
                {
                    viewModel.SearchEntity.ID = id;
                    viewModel.Search();
                }

                if (!String.IsNullOrEmpty(status))
                {
                    viewModel.SearchEntity.StatusCode = status;
                    viewModel.Search();
                }

                if (!String.IsNullOrEmpty(organization))
                {
                    viewModel.SearchEntity.WebCooperatorOrganization = organization;
                    viewModel.Search();
                }

                if (!String.IsNullOrEmpty(emailAddress))
                {
                    viewModel.SearchEntity.WebCooperatorEmailAddress = emailAddress;
                    viewModel.Search();
                }

                if (!String.IsNullOrEmpty(intendedUseCode))
                {
                    viewModel.SearchEntity.IntendedUseCode = intendedUseCode;
                    viewModel.Search();
                }
                return View("~/Views/WebOrder/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Search(WebOrderRequestViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();
                return View("~/Views/WebOrder/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult _Lookup(FormCollection formCollection)
        {
            WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();

            try
            {
                if (!String.IsNullOrEmpty(formCollection["TimeFrame"]))
                {
                    viewModel.SearchEntity.TimeFrame = formCollection["TimeFrame"];
                }

                if (!String.IsNullOrEmpty(formCollection["SelectedStatusList"]))
                {
                    viewModel.SearchEntity.StatusList = formCollection["SelectedStatusList"];
                }

                if (!String.IsNullOrEmpty(formCollection["SelectedWebUserList"]))
                {
                    viewModel.SearchEntity.WebUserList = formCollection["SelectedWebUserList"];
                }

                viewModel.Search();
                return PartialView("~/Views/WebOrder/Explorer/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        //public ActionResult DashboardList()
        //{
        //    WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
        //    try
        //    {
        //        viewModel.AuthenticatedUserWebUserID = AuthenticatedUser.WebUserID;
        //        viewModel.SearchEntity.TimeFrame = "TDY";
        //        viewModel.Search();
        //        ModelState.Clear();
        //        return PartialView("~/Views/WebOrder/Dashboard/_List.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return RedirectToAction("_InternalServerError", "Error");
        //    }
        //}

        //public ActionResult DashboardTotals()
        //{
        //    WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
        //    try
        //    {
        //        viewModel.GetDashboardTotals(DateTime.Now.Year);
        //        ModelState.Clear();
        //        return PartialView("~/Views/WebOrder/Dashboard/_ListByStatus.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return RedirectToAction("_InternalServerError", "Error");
        //    }
        //}

        [HttpPost]
        public JsonResult AddWebOrderRequestAction(FormCollection formCollection)
        {
            return null;
        }

        [HttpPost]
        public JsonResult SendEmail(FormCollection formCollection)
        {
            int entityId = 0;
            int webUserId = 0;
            string actionNote = String.Empty;
            WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();

            return null;

            //try
            //{
            //    webUserId = AuthenticatedUser.WebUserID;
            //    if (!String.IsNullOrEmpty(formCollection["EntityID"]))
            //    {
            //        entityId = Int32.Parse(formCollection["EntityID"]);
            //    }

            //    if (!String.IsNullOrEmpty(formCollection["ActionNote"]))
            //    {
            //        actionNote = formCollection["ActionNote"];
            //    }
            //    viewModel.InsertNote(entityId, actionNote, webUserId);
            //    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex);
            //    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            //}
        }

        [HttpPost]
        public JsonResult AddNote(FormCollection formCollection)
        {
            int entityId = 0;
            int webUserId = 0;
            string actionNote = String.Empty;
            WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();

            try
            {
                webUserId = AuthenticatedUser.WebUserID;
                if (!String.IsNullOrEmpty(formCollection["EntityID"]))
                {
                    entityId = Int32.Parse(formCollection["EntityID"]);
                }

                if (!String.IsNullOrEmpty(formCollection["ActionNote"]))
                {
                    actionNote = formCollection["ActionNote"];
                }
                viewModel.InsertNote(entityId, actionNote, webUserId);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
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