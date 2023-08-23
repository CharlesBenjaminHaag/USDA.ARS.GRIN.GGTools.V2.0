using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class HomeController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public ActionResult Index()
        {
            Session["APP_CONTEXT"] = "HOME";
            HomeViewModel viewModel = new HomeViewModel();
            viewModel.AuthenticatedUser = AuthenticatedUser;
            viewModel.CooperatorID = AuthenticatedUser.CooperatorID;
            viewModel.SysUserID = AuthenticatedUser.SysUserID;
            viewModel.SiteID = AuthenticatedUser.SiteID;
            viewModel.SiteShortName = AuthenticatedUser.SiteShortName;
            return View(viewModel);
        }

        public ActionResult Navigate(string applicationCode)
        {
            Session["APP_CONTEXT"] = applicationCode;
            switch (applicationCode)
            {
                case "GGT-TAX":
                    return RedirectToAction("Index", "Taxonomy");
                case "GGT-NRR":
                    return RedirectToAction("Index", "WebOrderRequest");
                case "GGT-CUR":
                    return RedirectToAction("Index", "AccessionInventoryAttachment");
                case "GGT-ARM":
                    return RedirectToAction("Explorer", "Cooperator");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Search()
        {
            try
            {
                return View("~/Views/Search/Index.cshtml");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public PartialViewResult RenderSidebar()
        {
            return PartialView("~/Views/Shared/Sidebars/_MainSidebar.cshtml");
        }
    }
}