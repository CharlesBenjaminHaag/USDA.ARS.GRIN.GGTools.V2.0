using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class CooperatorController : BaseController, IController<CooperatorViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Add()
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.PageTitle = String.Format("Add Cooperator");
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.MainSectionCSSClass = "col-md-12";
                return View("~/Views/Cooperator/Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
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


        public ActionResult Edit(int entityId)
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.Get(entityId);

                //TODO Get owned recs. Call only in edit mode.
                viewModel.GetRecordsOwned(entityId);
                viewModel.PageTitle = String.Format("Edit Cooperator [{0}]: {1}, {2}", entityId, viewModel.Entity.LastName, viewModel.Entity.FirstName);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.MainSectionCSSClass = "col-md-9";
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

        [GrinGlobalAuthentication]
        public ActionResult Index()
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            viewModel.PageTitle = "Cooperator Search";
            viewModel.TableName = "cooperator";
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
        public PartialViewResult _List(int siteId = 0, bool isDetailFormat = false)
        {
            string partialViewName = "_List.cshtml";

            try
            {
                if (isDetailFormat == true)
                {
                    partialViewName = "_DetailList.cshtml";
                }

                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.SearchEntity.SiteID = siteId;
                viewModel.Search();
                return PartialView("~/Views/Cooperator/" + partialViewName, viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListBySite(int siteId)
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.SearchSiteCurators(siteId);
                return PartialView("~/Views/Cooperator/_DetailList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
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

        public PartialViewResult RenderAppUserItemList(int cooperatorId)
        {
            //CooperatorViewModel viewModel = new CooperatorViewModel();
            try
            {
                //viewModel.SearchEntity.ID = cooperatorId;
                //viewModel.Search();
                return PartialView("~/Views/Cooperator/_AppUserItemList.cshtml");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult RenderRecordsOwnedList(int cooperatorId)
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

        [HttpPost]
        public PartialViewResult RenderRecordsOwnedList(FormCollection formCollection)
        {
            string partialViewName = String.Empty;
            CooperatorViewModel viewModel = new CooperatorViewModel();

            try
            {
                if (!String.IsNullOrEmpty(formCollection["CooperatorID"]))
                {
                    viewModel.Entity.ID = Int32.Parse(formCollection["CooperatorID"]);
                }

                if (!String.IsNullOrEmpty(formCollection["Category"]))
                {
                    partialViewName = "_RecordsOwnedList" + formCollection["Category"] + ".cshtml";
                }

                viewModel.GetRecordsOwned(viewModel.Entity.ID);
                return PartialView("~/Views/Cooperator/" + partialViewName, viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
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
    }
}