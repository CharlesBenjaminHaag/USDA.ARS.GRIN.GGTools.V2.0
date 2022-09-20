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
                viewModel.UpdatePassword();
                viewModel.SendNotification("P");
                return RedirectToAction("Edit", "SysUser", new {entityId = viewModel.Entity.ID});
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
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

        public ActionResult Search(SysUserViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult RenderWidget(int entityId) 
        {
            SysUserViewModel viewModel = new SysUserViewModel();

            if (entityId > 0)
            {
                viewModel.SearchEntity.ID = entityId;
                viewModel.Search();
            }
            return PartialView("~/Views/SysUser/_Widget.cshtml", viewModel);

        }
        public PartialViewResult RenderGroupWidget(int entityId)
        {
            SysUserViewModel viewModel = new SysUserViewModel();
            if (entityId > 0)
            {
                viewModel.SearchEntity.ID = entityId;
                viewModel.GetGroups(entityId);
            }
            return PartialView("~/Views/SysUser/Group/_Widget.cshtml", viewModel);
        }

        public PartialViewResult RenderEditModal()
        {
            return PartialView("~/Views/SysUser/Modals/_Edit.cshtml");
        }
        public PartialViewResult RenderPasswordResetModal()
        {
            return PartialView("~/Views/SysUser/Modals/_ResetPassword.cshtml");
        }

    }
}