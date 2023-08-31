using NLog;
using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult New()
        {
            return View("~/Views/Account/Request/Introduction.cshtml");
        }

        public ActionResult Details()
        {
            try
            {
                CooperatorViewModel viewModel = new CooperatorViewModel();
                return View("~/Views/Account/Request/Details.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Details(CooperatorViewModel viewModel)
        {
            try
            {
                //TODO
                //Check required fields
                //Check email address
                Session["NEW_ACCOUNT"] = viewModel;
                return RedirectToAction("Confirm", "Account");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Confirm()
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            try
            {
                if (Session["NEW_ACCOUNT"] != null)
                {
                    viewModel = Session["NEW_ACCOUNT"] as CooperatorViewModel;
                }
                return View("~/Views/Account/Request/Confirm.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Confirm(CooperatorViewModel viewModel)
        {
            try
            {
                viewModel.Entity.StatusCode = "PENDING";
                viewModel.Entity.CreatedByCooperatorID = 48;
                viewModel.Insert();
                return View("~/Views/Account/Request/Final.cshtml");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
    }
}