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
            viewModel.CooperatorID = AuthenticatedUser.CooperatorID;
            viewModel.SysUserID = AuthenticatedUser.SysUserID;
            viewModel.SiteID = AuthenticatedUser.SiteID;
            return View(viewModel);
        }

        public JsonResult SetActiveNavMenu(FormCollection formCollection)
        {


            return null;
        }

        public ActionResult Navigate(string applicationCode)
        {
            Session["APP_CONTEXT"] = applicationCode;
            switch (applicationCode)
            {
                case "GGT-TAX":
                    return RedirectToAction("Index", "Folder");
                case "GGT-NRR":
                    return RedirectToAction("Index", "WebOrder");
                case "GGT-CUR":
                    return RedirectToAction("Index", "AccessionInventoryAttachment");
                case "GGT-ARM":
                    return RedirectToAction("Index", "Cooperator");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}