using NLog;
using System;
using System.CodeDom;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class WebOrderRequestController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult View(int entityId)
        { 
            WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
            try 
            { 
                viewModel.Get(entityId);
                return View("~/Views/WebOrderRequest/Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
       
        public PartialViewResult _ListItems(int webOrderRequestId)
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.GetWebOrderRequestItems(webOrderRequestId);
                return PartialView("~/Views/WebOrder/_ListItems.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        public JsonResult Approve(WebOrderRequestViewModel viewModel)
        {
            viewModel.Get(viewModel.Entity.ID);
            viewModel.Entity.StatusCode = "NRR_APPROVE";
            viewModel.Entity.OwnedByWebUserID = AuthenticatedUser.WebUserID;
           //viewModel.Update();

            WebOrderRequestAction webOrderRequestAction = new WebOrderRequestAction();
            webOrderRequestAction.WebOrderRequestID = viewModel.Entity.ID;
            webOrderRequestAction.ActionCode = viewModel.EventAction;
            webOrderRequestAction.Note = viewModel.EventNote + "==========================================" + viewModel.ActionEmailBody;
            webOrderRequestAction.OwnedByWebUserID = AuthenticatedUser.WebUserID;
            viewModel.InsertAction(webOrderRequestAction);

            // TODO Send internal notification

            return null;
        }
        
        public JsonResult Reject(WebOrderRequestViewModel viewModel)
        {
            viewModel.Get(viewModel.Entity.ID);
            viewModel.Entity.StatusCode = "NRR_REJECT";
            viewModel.Entity.OwnedByWebUserID = AuthenticatedUser.WebUserID;
            //viewModel.Update();

            WebOrderRequestAction webOrderRequestAction = new WebOrderRequestAction();
            webOrderRequestAction.WebOrderRequestID = viewModel.Entity.ID;
            webOrderRequestAction.ActionCode = viewModel.EventAction;
            webOrderRequestAction.Note = viewModel.EventNote + "==========================================" + viewModel.ActionEmailBody;
            webOrderRequestAction.OwnedByWebUserID = AuthenticatedUser.WebUserID;
            viewModel.InsertAction(webOrderRequestAction);

            // TODO Send internal notification

            return null;
        }

        public JsonResult SendEmail(WebOrderRequestViewModel viewModel)
        {
            try
            {
                viewModel.Entity.OwnedByWebUserID = AuthenticatedUser.WebUserID;
                viewModel.SendEmail();
                
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        //public PartialViewResult RenderRejectModal(int entityId)
        //{
        //    WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
        //    viewModel.Edit(entityId);
        //    viewModel.EventValue = "Reject Web Order Request";

        //    //TODO Load appropriate template
        //    EmailTemplate emailTemplate = viewModel.GetEmailTemplate("CCL");
        //    viewModel.ActionEmailFrom = "gringlobal.orders@usda.gov";
        //    viewModel.ActionEmailTo = viewModel.Entity.WebCooperatorEmail;
        //    viewModel.ActionEmailBody = emailTemplate.Body;

        //    return PartialView("~/Views/WebOrder/Modals/_Email.cshtml", viewModel);
        //}
        
        //public PartialViewResult RenderNoteModal(int entityId)
        //{
        //    WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
        //    viewModel.SearchEntity.ID = entityId;
        //    viewModel.GetNotes();
        //    return PartialView("~/Views/WebOrder/Modals/_Note.cshtml", viewModel);
        //}
        
        [HttpPost]
        public JsonResult AddNote(int webOrderRequestId, string noteText)
        {
            try 
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.InsertNote(webOrderRequestId, noteText, AuthenticatedUser.WebUserID);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public PartialViewResult _Edit(WebOrderRequestViewModel viewModel)
        {
            throw new NotImplementedException();
            //try
            //{
            //    viewModel.Entity.WebUserID = AuthenticatedUser.WebUserID;
            //    viewModel.Entity.StatusCode = viewModel.EventValue;
            //    viewModel.Entity.OwnedByWebUserID = AuthenticatedUser.WebUserID;
            //    viewModel.Entity.Note = viewModel.EventNote;
            //    viewModel.Update();
            //    return RedirectToAction("Edit", "WebOrder", new { entityId = viewModel.Entity.ID });
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex);
            //    return RedirectToAction("InternalServerError", "Error");
            //}
        }

        [HttpPost]
        public JsonResult Edit(WebOrderRequestViewModel viewModel)
        {
            try
            {
               
                viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Entity.WebUserID = AuthenticatedUser.WebUserID;
                viewModel.Entity.StatusCode = viewModel.NewActionCode;
                //viewModel.Update();
                return Json(new { success = true, data = viewModel.Entity.ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Index()
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.PageTitle = "NRR Tool Web Order Explorer";
                return View("~/Views/WebOrderRequest/Index.cshtml", viewModel);
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

        [HttpPost]
        public JsonResult AddWebOrderRequestAction(FormCollection formCollection)
        {
            return null;
        }
        
        public PartialViewResult _RenderNoteWidget(int webOrderRequestId)
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.SearchEntity.ID = webOrderRequestId;
                viewModel.GetNotes();
                return PartialView("~/Views/WebOrder/Explorer/_NoteWidget.cshtml", viewModel);
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
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
        
        public PartialViewResult RenderSidebar()
        {
            return PartialView("~/Views/Shared/Sidebars/_MainSidebarNRR.cshtml");
        }

        #region Explorer
        
        public ActionResult Explorer()
        {
            try 
            { 
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                ViewBag.PageTitle = "Web Order Request Explorer";
                return View("~/Views/WebOrderRequest/Explorer/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
    }
}
        
        [HttpPost]
        public PartialViewResult ExplorerList(WebOrderRequestViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                return PartialView("~/Views/WebOrderRequest/Explorer/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        [HttpPost]
        public PartialViewResult ExplorerDetail(int webOrderRequestId)
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.Get(webOrderRequestId);
                viewModel.GetWebOrderRequestItems(webOrderRequestId);
                return PartialView("~/Views/WebOrderRequest/Explorer/_Detail.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult ExplorerTimeline(int webOrderRequestId)
        {
            WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();

            try
            {
                viewModel.SearchEntity.ID = webOrderRequestId;
                viewModel.GetWebOrderRequestActions();
                return PartialView("~/Views/WebOrderRequest/Explorer/_Timeline.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult ExplorerNotes(int webOrderRequestId)
        {
            try
            {
                WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
                viewModel.SearchEntity.ID = webOrderRequestId;
                viewModel.GetNotes();
                return PartialView("~/Views/WebOrderRequest/Explorer/_Notes.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        #endregion Explorer

        /// <summary>
        /// Returns HTML for an email-entry modal pre-populated with the template that
        /// corresponds to the supplied action code.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="webOrderRequestAction"></param>
        /// <returns></returns>
        public PartialViewResult Component_EmailModal(int entityId, string webOrderRequestAction)
        {
            WebOrderRequestViewModel viewModel = new WebOrderRequestViewModel();
            EmailTemplate emailTemplate = new EmailTemplate();

            try 
            { 
                switch (webOrderRequestAction)
                {
                    case "NRR_APPROVE":
                        viewModel.EventValue = "Approve Web Order Request";
                        emailTemplate = viewModel.GetEmailTemplate("CAP");
                        break;
                    case "NRR_REJECT":
                        viewModel.EventValue = "Reject Web Order Request";
                        emailTemplate = viewModel.GetEmailTemplate("RRJ");
                        break;
                    case "NRR_INFO":
                        viewModel.EventValue = "Request Additional Information";
                        emailTemplate = viewModel.GetEmailTemplate("RQI");
                        break;
                    case "NRR_CUR":
                        viewModel.EventValue = "Email Curators";
                        emailTemplate = viewModel.GetEmailTemplate("CUR");
                        break;
                    default:
                        throw new NullReferenceException("No email template found for WOR action code " + webOrderRequestAction);
                }
                viewModel.Get(entityId);
                if (viewModel.Entity == null)
                {
                    throw new IndexOutOfRangeException("WOR not found.");
                }

                viewModel.NewActionCode = webOrderRequestAction;
                viewModel.Entity.ID = entityId;
                viewModel.ActionEmailTo = viewModel.Entity.WebCooperatorEmail;
                viewModel.ActionEmailSubject = emailTemplate.Subject;
                viewModel.ActionEmailFrom = "gringlobal.orders@usda.gov";
                viewModel.ActionEmailBody = emailTemplate.Body;
                viewModel.ActionEmailBodyOriginal = emailTemplate.Body;

                // REFACTOR: Replace placeholder variables with WOR data.
                //viewModel.ActionEmailBody.Replace("@WebCooperatorFullName", viewModel.Entity.WebCooperatorFullName);
                viewModel.ActionEmailBody = viewModel.ActionEmailBody.Replace("[ID_HERE]", entityId.ToString());
                viewModel.ActionEmailSubject = viewModel.ActionEmailSubject.Replace("[ID_HERE]", entityId.ToString());
                return PartialView("~/Views/WebOrderRequest/Components/_EmailEditor.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    }
}