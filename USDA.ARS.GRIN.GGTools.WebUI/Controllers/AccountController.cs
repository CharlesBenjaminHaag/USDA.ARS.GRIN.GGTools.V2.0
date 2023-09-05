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

        public ActionResult Step(int seq)
        { 
            switch(seq)
            {
                case 1:
                    return RedirectToAction("Details", "Account");

                default:
                    return View("~/Views/Account/Request/Introduction.cshtml");
            }
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
                if (!viewModel.Validate())
                {
                    return View("~/Views/Account/Request/Details.cshtml", viewModel);
                }

                SiteViewModel siteViewModel = new SiteViewModel();
                siteViewModel.Get(viewModel.Entity.SiteID);

                viewModel.Entity.SiteID = siteViewModel.Entity.ID;
                viewModel.Entity.SiteName = siteViewModel.Entity.LongName;
                viewModel.Entity.AddressLine1 = siteViewModel.Entity.PrimaryAddress1;
                viewModel.Entity.AddressLine2 = siteViewModel.Entity.PrimaryAddress2;
                viewModel.Entity.AddressLine3 = siteViewModel.Entity.PrimaryAddress3;
                viewModel.Entity.City = siteViewModel.Entity.PrimaryCity;
                viewModel.Entity.GeographyID = siteViewModel.Entity.GeographyID;
                viewModel.Entity.PostalIndex = siteViewModel.Entity.PostalIndex;

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
        public ActionResult Final()
        {
            CooperatorViewModel viewModel = new CooperatorViewModel();
            return View("~/Views/Account/Request/Final.cshtml", viewModel);
        }
        [HttpPost]
        public ActionResult Final(CooperatorViewModel viewModel)
        {
            CooperatorViewModel sessionCooperatorViewModel = null;
            SysUserViewModel sysUserViewModel = new SysUserViewModel();

            //TODO
            //Get notes
            //Save coop and related records
            //Send primary email
            //Send CC if requested
            if (Session["NEW_ACCOUNT"] != null)
            {
                // Cooperator
                sessionCooperatorViewModel = Session["NEW_ACCOUNT"] as CooperatorViewModel;
                sessionCooperatorViewModel.Entity.StatusCode = "PENDING";
                sessionCooperatorViewModel.Entity.CreatedByCooperatorID = 48;
                sessionCooperatorViewModel.Insert();

                // Sys user
                if (sessionCooperatorViewModel.Entity.ID > 0)
                {
                    sysUserViewModel.Entity.UserName = sessionCooperatorViewModel.Entity.FirstName.ToLower() + "." + sessionCooperatorViewModel.Entity.LastName.ToLower();
                    sysUserViewModel.Entity.SysUserName = sysUserViewModel.Entity.UserName;                   
                    sysUserViewModel.Entity.CreatedByCooperatorID = 48;
                    sysUserViewModel.Entity.Password = sysUserViewModel.GetSecurePassword("GR1NGl@bal!2023");
                    sysUserViewModel.Entity.SysUserPassword = sysUserViewModel.Entity.Password;
                    sysUserViewModel.Entity.CooperatorID = sessionCooperatorViewModel.Entity.ID;
                    sysUserViewModel.Insert();
                }
            }
            
            return View("~/Views/Account/Request/Final.cshtml");
        }
        public PartialViewResult GetSite(int entityId)
        {
            try
            {
                SiteViewModel viewModel = new SiteViewModel();
                viewModel.Get(entityId);
                return PartialView("~/Views/Site/_EditAddress.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    }
}