using NLog;
using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class CooperatorController : BaseController, IController<CooperatorViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Explorer()
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            return View("~/Views/Cooperator/Explorer/Index.cshtml", viewModel);
        }
        
        public ActionResult Get(int entityId)
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.Get(entityId, "");
                viewModel.PageTitle = String.Format("Edit Cooperator [{0}]: {1}, {2}", entityId, viewModel.Entity.LastName, viewModel.Entity.FirstName);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                return View("~/Views/Cooperator/Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public PartialViewResult _Get(int entityId, string environment)
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.Get(entityId, environment);
                viewModel.PageTitle = String.Format("Edit Cooperator [{0}]: {1}", entityId, viewModel.Entity.FullName);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                return PartialView("~/Views/Cooperator/_Edit.cshtml", viewModel);
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

                    // SYS USER
                    SysUserViewModel sysUserViewModel = new SysUserViewModel();
                    sysUserViewModel.Entity.SysUserName = viewModel.Entity.FirstName.ToLower() + "." + viewModel.Entity.LastName.ToLower();
                    sysUserViewModel.Entity.UserName = sysUserViewModel.Entity.SysUserName;
                    sysUserViewModel.Entity.SysUserPassword = sysUserViewModel.GetSecurePassword("GRINGl@bal!2023Pa$$");
                    sysUserViewModel.Entity.CooperatorID = viewModel.Entity.ID;
                    sysUserViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    sysUserViewModel.Insert();

                    SysGroupUserMapViewModel sysGroupUserMapViewModel = new SysGroupUserMapViewModel();

                    // GROUP, ALLUSERS
                    sysGroupUserMapViewModel.Entity.SysGroupID = 2;
                    sysGroupUserMapViewModel.Entity.SysUserID = sysUserViewModel.Entity.ID;
                    sysGroupUserMapViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    sysGroupUserMapViewModel.Insert();

                    // GROUP, CT USERS
                    sysGroupUserMapViewModel.Entity.SysGroupID = 3;
                    sysGroupUserMapViewModel.Entity.SysUserID = sysUserViewModel.Entity.ID;
                    sysGroupUserMapViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    sysGroupUserMapViewModel.Insert();

                    // GROUP, WEB QUERY USERS
                    sysGroupUserMapViewModel.Entity.SysGroupID = 6;
                    sysGroupUserMapViewModel.Entity.SysUserID = sysUserViewModel.Entity.ID;
                    sysGroupUserMapViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    sysGroupUserMapViewModel.Insert();
                    
                    // WEB COOPERATOR
                    WebCooperatorViewModel webCooperatorViewModel = new WebCooperatorViewModel();
                    webCooperatorViewModel.Entity.FirstName = viewModel.Entity.FirstName;
                    webCooperatorViewModel.Entity.LastName = viewModel.Entity.LastName;
                    webCooperatorViewModel.Entity.EmailAddress = viewModel.Entity.EmailAddress;

                    webCooperatorViewModel.Entity.Address1 = viewModel.Entity.AddressLine1;
                    webCooperatorViewModel.Entity.Address2 = viewModel.Entity.AddressLine2;
                    webCooperatorViewModel.Entity.Address3 = viewModel.Entity.AddressLine3;
                    webCooperatorViewModel.Entity.City = viewModel.Entity.City;
                    webCooperatorViewModel.Entity.PostalCode = viewModel.Entity.PostalIndex;
                    webCooperatorViewModel.Entity.GeographyID = viewModel.Entity.GeographyID;
                    webCooperatorViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    webCooperatorViewModel.Insert();

                    viewModel.SearchEntity.ID = viewModel.Entity.ID;
                    viewModel.Search();
                    viewModel.Entity.WebCooperatorID = webCooperatorViewModel.Entity.ID;
                    viewModel.Update();
                    
                }
                else
                {
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }
                return _Get(viewModel.Entity.ID, "");
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

        //public ViewResult Explorer()
        //{
        //    return View();
        //}
        //public PartialViewResult _RenderLookupModal()
        //{
        //    try
        //    {
        //        CooperatorViewModel viewModel = new CooperatorViewModel();
        //        return PartialView("~/Views/Cooperator/Modals/_Lookup.cshtml",viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}
        [HttpPost]
        //public PartialViewResult _Lookup(CooperatorViewModel viewModel)
        //{
        //    try
        //    {
        //        viewModel.Search();
        //        return PartialView("~/Views/Cooperator/Modals/_SelectList.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}
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
        //[HttpPost]
        //public JsonResult Add(FormCollection formCollection)
        //{
        //    throw new NotImplementedException();
        //}

        //public ActionResult Delete(int entityId)
        //{
        //    throw new NotImplementedException();
        //}

        //public ActionResult View(int entityId)
        //{
        //    Retrieve main cooperator record(to be copied). Assumption is use case 
        //     where user is updating a training record.
        //    CooperatorViewModel viewModel = new CooperatorViewModel();
        //    viewModel.SearchEntity.ID = entityId;
        //    viewModel.Get(entityId, "PROD");
        //    viewModel.AuthenticatedUser = AuthenticatedUser;
        //    return View(viewModel);
        //}
        public ActionResult Add()
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.PageTitle = String.Format("Add Cooperator");
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Entity.StatusCode = "ACTIVE";
                return View("~/Views/Cooperator/Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public PartialViewResult _Add(int siteId = 0)
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();

                SiteViewModel siteViewModel = new SiteViewModel();
                siteViewModel.Get(siteId);
                viewModel.Entity.OrganizationAbbrev = siteViewModel.Entity.OrganizationAbbrev;
                viewModel.Entity.SiteID = siteViewModel.Entity.ID;
                viewModel.Entity.AddressLine1 = siteViewModel.Entity.PrimaryAddress1;
                viewModel.Entity.AddressLine2 = siteViewModel.Entity.PrimaryAddress2;
                viewModel.Entity.City = siteViewModel.Entity.PrimaryCity;
                viewModel.Entity.GeographyID = siteViewModel.Entity.GeographyID;
                viewModel.Entity.StateName = siteViewModel.Entity.StateName;
                viewModel.Entity.PostalIndex = siteViewModel.Entity.PostalIndex;
                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _Edit(int entityId, string environment = "")
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.Get(entityId, environment);
                viewModel.PageTitle = String.Format("Edit Cooperator [{0}]: {1}", entityId, viewModel.Entity.FullName);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public ActionResult Activate(int entityId)
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.Get(entityId, "");
                viewModel.Entity.StatusCode = "ACTIVE";
                viewModel.Update();
                viewModel.Get(entityId, "");

                viewModel.PageTitle = String.Format("Edit Cooperator [{0}]: {1}, {2}", entityId, viewModel.Entity.LastName, viewModel.Entity.FirstName);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.MainSectionCSSClass = "col-md-9";
                return View("~/Views/Cooperator/Edit.cshtml", viewModel);
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
                return RedirectToAction("Edit", "Cooperator", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        [HttpPost]
        public JsonResult _Edit(CooperatorViewModel viewModel)
        {
            SysUserViewModel sysUserViewModel = new SysUserViewModel();
            try
            {
                //if (!viewModel.Validate())
                //{
                //    if (viewModel.ValidationMessages.Count > 0) return View(viewModel);
                //}

                if (viewModel.Entity.ID == 0)
                {
                    viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    // TODO Set default pw
                    viewModel.Entity.SysUserPassword = sysUserViewModel.GetSecurePassword("GRIN!Gl@balPa$$");
                    viewModel.Insert();
                }
                else
                {
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }
                viewModel.Get(viewModel.Entity.ID);
                return Json(new { cooperator = viewModel.Entity }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                viewModel.Entity.ID = -1;
                return Json(new { cooperator = viewModel.Entity }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddSysUser(FormCollection formCollection)
        {
            SysUserViewModel viewModel = new SysUserViewModel();
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

            try
            {
                if (!String.IsNullOrEmpty(formCollection["CooperatorID"]))
                {
                    viewModel.Entity.CooperatorID = Int32.Parse(formCollection["CooperatorID"]);
                }

                if (!String.IsNullOrEmpty(formCollection["SysUserName"]))
                {
                    viewModel.Entity.UserName = formCollection["SysUserName"];
                }

                if (!String.IsNullOrEmpty(formCollection["SysUserPassword"]))
                {
                    viewModel.Entity.Password = formCollection["SysUserPassword"];
                }

                viewModel.Insert();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult UpdateSysUser(FormCollection formCollection)
        {
            SysUserViewModel viewModel = new SysUserViewModel();
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

            try
            {
                if (!String.IsNullOrEmpty(formCollection["CooperatorID"]))
                {
                    viewModel.Entity.CooperatorID = Int32.Parse(formCollection["CooperatorID"]);
                }

                if (!String.IsNullOrEmpty(formCollection["SysUserName"]))
                {
                    viewModel.Entity.UserName = formCollection["SysUserName"];
                }

                if (!String.IsNullOrEmpty(formCollection["SysUserPassword"]))
                {
                    viewModel.Entity.Password = formCollection["SysUserPassword"];
                }

                viewModel.Insert();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }


        [GrinGlobalAuthentication]
        public ActionResult Index()
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            viewModel.PageTitle = "Cooperator Search";
            viewModel.TableName = "cooperator";
            viewModel.AuthenticatedUser = AuthenticatedUser;
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Search(CooperatorViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();
                return View("~/Views/Cooperator/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public PartialViewResult _List(int siteId = 0, string sysUserIsEnabled = "Y", string statusCode = "ACTIVE", string formatCode = "", string sysGroupTag = "", bool isDetailFormat = false)
        {
            string partialViewName = "_List.cshtml";

            try
            {
                switch(formatCode)
                {
                    case "LIST":
                        partialViewName = "_List.cshtml";
                        break;
                    case "SLST":
                        partialViewName = "_SelectList.cshtml";
                        break;
                    case "LWGT":
                        partialViewName = "_ListWidget.cshtml";
                        break;
                }

                if (isDetailFormat == true)
                {
                    partialViewName = "_DetailList.cshtml";
                }

                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.SearchEntity.SiteID = siteId;
                viewModel.SearchEntity.StatusCode = statusCode;
                viewModel.SearchEntity.SysUserIsEnabled = sysUserIsEnabled;
                viewModel.Search();
                return PartialView("~/Views/Cooperator/" + partialViewName, viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _ListBySysGroup(string groupTag = "")
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.GetCooperatorsBySysGroup(groupTag);
                return PartialView("~/Views/Cooperator/_ContactListWidget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderLookupModal()
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            return PartialView("~/Views/Cooperator/Modals/_Lookup.cshtml",viewModel);
        }
        public PartialViewResult RenderWidget(int cooperatorId)
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            try
            {
                viewModel.SearchEntity.ID = cooperatorId;
                viewModel.Search();
                return PartialView("~/Views/Cooperator/_Widget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderStatusWidget(int cooperatorId)
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            try
            {
                viewModel.GetStatus(cooperatorId);
                return PartialView("~/Views/Cooperator/_ChecklistWidget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _RenderRecordsOwnedList(int cooperatorId)
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            try
            {
                viewModel.GetRecordsOwned(cooperatorId);
                return PartialView("~/Views/Cooperator/_RecordsOwnedDetailList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public ActionResult RecordOwnershipEdit(int entityId = 0)
        {
            try
            {
                CooperatorRecordTransferViewModel viewModel = new CooperatorRecordTransferViewModel();
                viewModel.GetSiteCooperators(AuthenticatedUser.SiteID);
                viewModel.SourceCooperatorID = entityId;
                return View("~/Views/Cooperator/Transfer.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public JsonResult Transfer(FormCollection formCollection)
        {
            try
            {
                CooperatorRecordTransferViewModel viewModel = new CooperatorRecordTransferViewModel();

                if (!String.IsNullOrEmpty(formCollection["SourceCooperatorID"]))
                {
                    viewModel.SourceCooperatorID = Int32.Parse(formCollection["SourceCooperatorID"]);
                }

                if (!String.IsNullOrEmpty(formCollection["TargetCooperatorID"]))
                {
                    viewModel.TargetCooperatorID = Int32.Parse(formCollection["TargetCooperatorID"]);
                }

                if (!String.IsNullOrEmpty(formCollection["SourceTableList"]))
                {
                    viewModel.SourceTableList = formCollection["SourceTableList"];
                }
                viewModel.Transfer();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public JsonResult Sync(int cooperatorId)
        {
            //TODO
            return null;
        }

        public JsonResult SyncSysUser(int sysUserId)
        {
            //TODO
            return null;
        }
        public JsonResult SyncWebCooperator(int webCooperatorId)
        {
            //TODO
            return null;
        }
        public JsonResult SyncWebUser(int webUserId)
        {
            //TODO
            return null;
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.Get(entityId, "");
                viewModel.PageTitle = String.Format("Edit Cooperator [{0}]: {1}, {2}", entityId, viewModel.Entity.LastName, viewModel.Entity.FirstName);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.MainSectionCSSClass = "col-md-9";
                return View("~/Views/Cooperator/Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
    }
}