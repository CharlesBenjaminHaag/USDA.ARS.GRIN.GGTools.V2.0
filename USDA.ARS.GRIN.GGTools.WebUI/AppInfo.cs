using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Text;
using System.Configuration;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI
{
    public static class AppInfo
    {
        public static string GetPageTitle()
        {
            string pageTitle = System.Configuration.ConfigurationManager.AppSettings["AppTitle"];
            return pageTitle;
        }

        public static string GetVersion()
        {
            Version version = null;
            StringBuilder versionNumber = new StringBuilder();

            version = Assembly.GetExecutingAssembly().GetName().Version;
            versionNumber.Append(version.Major.ToString());
            versionNumber.Append(".");
            versionNumber.Append(version.Minor.ToString());
            versionNumber.Append(".");
            versionNumber.Append(version.Build.ToString());

            // TODO Store additional label in config
            versionNumber.Append(" Alpha");

            return versionNumber.ToString();
        }

        public static SysUser GetAuthenticatedUser()
        {
            SysUser authenticatedUser = new SysUser(); 
            AuthenticatedUserSession authenticatedUserSession = System.Web.HttpContext.Current.Session["AUTHENTICATED_USER_SESSION"] as AuthenticatedUserSession;
            
            if (authenticatedUserSession != null)
                authenticatedUser = authenticatedUserSession.User;
            
            return authenticatedUser;
        }
        public static string GetDatabase()
        {
            string databaseName = String.Empty;
            databaseName = ConfigurationManager.AppSettings["Database"];
            return databaseName.ToUpper();
        }
        public static string GetSupportEmail()
        {
            string emailAddress = String.Empty;
            emailAddress = ConfigurationManager.AppSettings["EmailAddressSupport"];
            return emailAddress;
        }
    }
}