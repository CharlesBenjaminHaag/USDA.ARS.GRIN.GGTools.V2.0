using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI
{
    public class BaseController : Controller
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public SysUser AuthenticatedUser
        {
            get
            {
                AuthenticatedUserSession authenticatedUserSession = null;

                if (Session != null)
                {
                    if (Session["AUTHENTICATED_USER_SESSION"] != null)
                    {
                        authenticatedUserSession = Session["AUTHENTICATED_USER_SESSION"] as AuthenticatedUserSession;
                    }
                }
                return authenticatedUserSession.User;
            }
        }

        [HttpGet]
        public ActionResult Refresh()
        {
            if (Session["type"] != null && Session["resulttype"] != null)
                return View();
            else
                return Redirect(Request.UrlReferrer.ToString());
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    filterContext.ExceptionHandled = true;

        //    //Log the error!!
        //    Log.Error(filterContext.Exception);
        //}
        protected string GetFormFieldValue(FormCollection formCollection, string fieldName)
        {
            if (formCollection.Count == 0)
            {
                return String.Empty;
            }

            if (formCollection[fieldName] == null)
            {
                return String.Empty;
            }
            else
            {
                return formCollection[fieldName];
            }
        }
    }
}