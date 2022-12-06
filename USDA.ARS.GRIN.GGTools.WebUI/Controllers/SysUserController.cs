using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class SysUserController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public PartialViewResult Add(FormCollection formCollection)
        {
            SysUserViewModel viewModel = new SysUserViewModel();

            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
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
                viewModel.Entity.SysUserPlainTextPassword = viewModel.Entity.Password;
            }
            viewModel.Insert();

            // Retrieve new account
            SysUserViewModel confirmationViewModel = new SysUserViewModel();
            confirmationViewModel.SearchEntity.ID = viewModel.Entity.ID;
            confirmationViewModel.Search();
            confirmationViewModel.Entity.SysUserPlainTextPassword = viewModel.Entity.SysUserPlainTextPassword;
            confirmationViewModel.SendNotification("N");
            return PartialView("~/Views/SysUser/_Widget.cshtml", confirmationViewModel);
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                SysUserViewModel viewModel = new SysUserViewModel();

                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.AuthenticatedUser = AuthenticatedUser;
                    viewModel.DisplayUserName = viewModel.Entity.SysUserName;
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _Edit(int entityId, string environment = "")
        {
            try 
            { 
                SysUserViewModel viewModel = new SysUserViewModel();
                viewModel.Get(entityId);
                return PartialView("~/Views/SysUser/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public PartialViewResult ResetPassword(FormCollection formCollection)
        {
            SysUserViewModel viewModel = new SysUserViewModel();

            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            if (!String.IsNullOrEmpty(formCollection["SysUserID"]))
            {
                viewModel.Entity.SysUserID = Int32.Parse(formCollection["SysUserID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["SysUserName"]))
            {
                viewModel.Entity.UserName = formCollection["SysUserName"];
            }

            if (!String.IsNullOrEmpty(formCollection["SysUserPassword"]))
            {
                viewModel.Entity.Password = formCollection["SysUserPassword"];
                viewModel.Entity.SysUserPlainTextPassword = viewModel.Entity.Password;
            }
            viewModel.Update();

            // Retrieve updated account
            SysUserViewModel confirmationViewModel = new SysUserViewModel();
            confirmationViewModel.SearchEntity.ID = viewModel.Entity.SysUserID;
            confirmationViewModel.Search();
            confirmationViewModel.Entity.SysUserPlainTextPassword = viewModel.Entity.SysUserPlainTextPassword;
            confirmationViewModel.SendNotification("P");
            return PartialView("~/Views/SysUser/_Widget.cshtml", confirmationViewModel);
        }

        [HttpPost]
        public ActionResult Edit(SysUserViewModel viewModel)
        {
            try
            {
                viewModel.AuthenticatedUser = AuthenticatedUser;
                if (!viewModel.Validate())
                {
                    return View("~/Views/SysUser/Edit.cshtml", viewModel);
                }
                viewModel.Update();
                if (viewModel.SendNotificationOption == true)
                {
                    viewModel.SendNotification("P");
                }
                return RedirectToAction("Edit", "SysUser", new {entityId = viewModel.Entity.ID});
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public JsonResult _Save(SysUserViewModel viewModel)
        {
            try
            {
                if (viewModel.Entity.ID > 0)
                {
                    viewModel.SendNotification("P");
                    viewModel.Update();
                }
                else
                {
                    viewModel.Insert();
                }
                return Json(new { sysUser = viewModel.Entity }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: SysUser
        public ActionResult Index()
        {
            SysUserViewModel viewModel = new SysUserViewModel();
            viewModel.PageTitle = "Sys User Search";
            viewModel.TableName = "sys_user";
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
            return View(viewModel);
        }

        public PartialViewResult _ListAvailableSysGroups(int sysUserId)
        {
            SysUserViewModel viewModel = new SysUserViewModel();
            viewModel.GetAvailableSysGroups(sysUserId);
            return PartialView("~/Views/SysUser/Group/_ListAvailable.cshtml", viewModel);
        }
        public PartialViewResult _ListAssignedSysGroups(int sysUserId)
        {
            SysUserViewModel viewModel = new SysUserViewModel();
            viewModel.GetAssignedSysGroups(sysUserId);
            return PartialView("~/Views/SysUser/Group/_ListAssigned.cshtml", viewModel);
        }

        [HttpPost]
        public JsonResult AssignGroups(FormCollection formCollection)
        {
            try
            {
                SysUserViewModel viewModel = new SysUserViewModel();
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

                if (!String.IsNullOrEmpty(formCollection["SysUserID"]))
                {
                    viewModel.Entity.ID = Int32.Parse(formCollection["SysUserID"]);
                    viewModel.Entity.SysUserID = viewModel.Entity.ID; 
                }

                if (!String.IsNullOrEmpty(formCollection["IDList"]))
                {
                    viewModel.Entity.ItemIDList = formCollection["IDList"];
                }
                viewModel.AssignSysGroups();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UnAssignSysGroups(FormCollection formCollection)
        {
            try
            {
                SysUserViewModel viewModel = new SysUserViewModel();
                if (!String.IsNullOrEmpty(formCollection["SysUserID"]))
                {
                    viewModel.Entity.ID = Int32.Parse(formCollection["SysUserID"]);
                    viewModel.Entity.SysUserID = viewModel.Entity.ID;
                }

                if (!String.IsNullOrEmpty(formCollection["IDList"]))
                {
                    viewModel.Entity.ItemIDList = formCollection["IDList"];
                }
                viewModel.UnAssignSysGroups();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Search(SysUserViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult RenderWidget(int sysUserId) 
        {
            SysUserViewModel viewModel = new SysUserViewModel();

            if (sysUserId > 0)
            {
                viewModel.SearchEntity.ID = sysUserId;
                viewModel.Search();
            }
            return PartialView("~/Views/SysUser/_Widget.cshtml", viewModel);

        }
        public PartialViewResult RenderSysGroupWidget(int sysUserId)
        {
            SysUserViewModel viewModel = new SysUserViewModel();
            if (sysUserId > 0)
            {
                viewModel.SearchEntity.ID = sysUserId;
                viewModel.GetGroups(sysUserId);
            }
            return PartialView("~/Views/SysUser/Group/_Widget.cshtml", viewModel);
        }

        public PartialViewResult RenderEditModal()
        {
            SysUserViewModel viewModel = new SysUserViewModel();
            viewModel.Entity.UserName = "";
            viewModel.Entity.Password = "";
            viewModel.Entity.PasswordConfirm = "";
            return PartialView("~/Views/SysUser/Modals/_Edit.cshtml", viewModel);
        }
        public PartialViewResult RenderPasswordResetModal()
        {
            return PartialView("~/Views/SysUser/Modals/_ResetPassword.cshtml");
        }

    }
}