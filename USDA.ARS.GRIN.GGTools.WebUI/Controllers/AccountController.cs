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
            ViewBag.PageTitle = "Curator Tool Account Request: Introduction";
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
            const string DEFAULT_COOPERATOR_STATUS = "PENDING";
            const int DEFAULT_ADMIN_SYS_USER_ID = 48;
            const int DEFAULT_ADMIN_WEB_USER_ID = 1;
            const int SYS_GROUP_ID_ALLUSERS = 2;
            const int SYS_GROUP_ID_CTUSERS = 3;
            const int SYS_GROUP_ID_WEBTOOLS = 6;
            const string SYS_GROUP_TAG_GGTOOLS_MANAGE_COOPERATOR = "GGTOOLS_MANAGE_COOPERATOR";

            CooperatorViewModel sessionCooperatorViewModel = null;
            SysUserViewModel sysUserViewModel = new SysUserViewModel();
            SysUserViewModel verificationSysUserViewModel = new SysUserViewModel();
            SysGroupUserMapViewModel sysGroupUserMapViewModel = new SysGroupUserMapViewModel();
            WebCooperatorViewModel webCooperatorViewModel = new WebCooperatorViewModel();
            WebUserViewModel webUserViewModel = new WebUserViewModel();

            //TODO
            //Get notes
            //Save coop and related records
            //Send primary email
            //Send CC if requested
            if (Session["NEW_ACCOUNT"] != null)
            {
                // Cooperator
                sessionCooperatorViewModel = Session["NEW_ACCOUNT"] as CooperatorViewModel;
                sessionCooperatorViewModel.Entity.StatusCode = DEFAULT_COOPERATOR_STATUS;
                sessionCooperatorViewModel.Entity.CreatedByCooperatorID = DEFAULT_ADMIN_SYS_USER_ID;
                sessionCooperatorViewModel.Entity.WebCooperatorID = 0;
                sessionCooperatorViewModel.Insert();

                // Sys user
                if (sessionCooperatorViewModel.Entity.ID > 0)
                {
                    // Check for existing acct.
                    verificationSysUserViewModel.SearchEntity.UserName = sessionCooperatorViewModel.Entity.FirstName.ToLower() + "." + sessionCooperatorViewModel.Entity.LastName.ToLower();
                    verificationSysUserViewModel.Search();

                    if (verificationSysUserViewModel.Entity.ID > 0)
                    {
                        sysUserViewModel.Entity.UserName = verificationSysUserViewModel.Entity.UserName + DateTime.Now.Date.ToString();
                    }
                    else
                    {
                        sysUserViewModel.Entity.UserName = sessionCooperatorViewModel.Entity.FirstName.ToLower() + "." + sessionCooperatorViewModel.Entity.LastName.ToLower();
                    }

                    sysUserViewModel.Entity.SysUserName = sysUserViewModel.Entity.UserName;                   
                    sysUserViewModel.Entity.CreatedByCooperatorID = DEFAULT_ADMIN_SYS_USER_ID;
                    sysUserViewModel.Entity.Password = sysUserViewModel.GetSecurePassword("GR1NGl@bal!2023");
                    sysUserViewModel.Entity.SysUserPassword = sysUserViewModel.Entity.Password;
                    sysUserViewModel.Entity.CooperatorID = sessionCooperatorViewModel.Entity.ID;
                    sysUserViewModel.Entity.IsEnabled = "N";
                    sysUserViewModel.Insert();
                }

                // Groups
                if (sysUserViewModel.Entity.ID > 0)
                {
                    sysGroupUserMapViewModel.Entity.SysUserID = sysUserViewModel.Entity.ID;
                    sysGroupUserMapViewModel.Entity.SysGroupID = SYS_GROUP_ID_ALLUSERS;
                    sysGroupUserMapViewModel.Entity.CreatedByCooperatorID = DEFAULT_ADMIN_SYS_USER_ID;
                    sysGroupUserMapViewModel.Insert();

                    sysGroupUserMapViewModel.Entity.SysUserID = sysUserViewModel.Entity.ID;
                    sysGroupUserMapViewModel.Entity.SysGroupID = SYS_GROUP_ID_CTUSERS;
                    sysGroupUserMapViewModel.Entity.CreatedByCooperatorID = DEFAULT_ADMIN_SYS_USER_ID;
                    sysGroupUserMapViewModel.Insert();

                    sysGroupUserMapViewModel.Entity.SysUserID = sysUserViewModel.Entity.ID;
                    sysGroupUserMapViewModel.Entity.SysGroupID = SYS_GROUP_ID_WEBTOOLS;
                    sysGroupUserMapViewModel.Entity.CreatedByCooperatorID = DEFAULT_ADMIN_SYS_USER_ID;
                    sysGroupUserMapViewModel.Insert();
                }

                // Web coop
                webCooperatorViewModel.Entity.FirstName = viewModel.Entity.FirstName;
                webCooperatorViewModel.Entity.LastName = viewModel.Entity.LastName;
                webCooperatorViewModel.Entity.JobTitle = viewModel.Entity.JobTitle;
                webCooperatorViewModel.Entity.Address1 = viewModel.Entity.AddressLine1;
                webCooperatorViewModel.Entity.Address2 = viewModel.Entity.AddressLine2;
                webCooperatorViewModel.Entity.Address3 = viewModel.Entity.AddressLine3;
                webCooperatorViewModel.Entity.City = viewModel.Entity.City;
                webCooperatorViewModel.Entity.GeographyID = viewModel.Entity.GeographyID;
                webCooperatorViewModel.Entity.PostalCode = viewModel.Entity.PostalIndex;
                webCooperatorViewModel.Entity.CreatedByCooperatorID = DEFAULT_ADMIN_WEB_USER_ID;
                webCooperatorViewModel.Entity.StatusCode = DEFAULT_COOPERATOR_STATUS;
                webCooperatorViewModel.Insert();

                if (webCooperatorViewModel.Entity.ID > 0)
                {
                    sessionCooperatorViewModel.Get(sessionCooperatorViewModel.Entity.ID);
                    sessionCooperatorViewModel.Entity.WebCooperatorID = webCooperatorViewModel.Entity.ID;
                    sessionCooperatorViewModel.Update();
                }

                // Web user
                webUserViewModel.Entity.WebUserName = sessionCooperatorViewModel.Entity.EmailAddress;
                webUserViewModel.Entity.WebUserPassword = sysUserViewModel.GetSecurePassword("GR1NGl@bal!2023");
                webUserViewModel.Entity.CreatedByCooperatorID = DEFAULT_ADMIN_WEB_USER_ID;
                webUserViewModel.Entity.WebCooperatorID = webCooperatorViewModel.Entity.ID;
                webUserViewModel.Entity.IsEnabled = "N";
                webUserViewModel.Insert();

                //TODO
                //Emails to:
                //1. Approval list/group
                sessionCooperatorViewModel.SearchEntity.SysGroupTag = SYS_GROUP_TAG_GGTOOLS_MANAGE_COOPERATOR;
                sessionCooperatorViewModel.Search();

                foreach (var result in sessionCooperatorViewModel.DataCollection)
                { 
                    // TODO email each user
                }

                //2. IF SELECTED, requestor

            }

            return View("~/Views/Account/Request/ThankYou.cshtml");
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