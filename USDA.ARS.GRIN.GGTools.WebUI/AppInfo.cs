using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Text;
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

        public static List<SysTable> GetNavigationList()
        {
            return new List<SysTable>()
            {
                new SysTable { Name="CropForCWR", Title="Crop For CWR" },
                new SysTable { Name="CWRMap", Title="CWR Map" },
                new SysTable { Name="CWRTrait", Title="CWR Trait" },
                new SysTable { Name="Species", Title="Species" },
                new SysTable { Name="Family", Title="Family" },
                new SysTable { Name="Genus", Title="Genus" },
                new SysTable { Name="CommonName", Title="Common Name" },
                new SysTable { Name="EconomicUse", Title="Economic Use" },
                new SysTable { Name="Geography", Title="Geography" },
                new SysTable { Name="GeographyMap", Title="Geography Map" },
                new SysTable { Name="Author", Title="Author" },
                new SysTable { Name="Citation", Title="Citation" },
                new SysTable { Name="Literature", Title="Literature" },
                new SysTable { Name="Regulation", Title="Regulation" },
                new SysTable { Name="RegulationMap", Title="Regulation Map" }
            }.OrderBy(x=>x.Title).ToList();
        }

        public static List<SysTable> GetReportList()
        {
            return new List<SysTable>()
            {
                new SysTable { Name="MBAS", Title="Missing Basionyms" },
                //new SysTable { Name="CWRMap", Title="CWR Map" },
                //new SysTable { Name="CWRTrait", Title="CWR Trait" },
                //new SysTable { Name="Species", Title="Species" },
                //new SysTable { Name="Family", Title="Family" },
                //new SysTable { Name="Genus", Title="Genus" },
                //new SysTable { Name="CommonName", Title="Common Name" },
                //new SysTable { Name="EconomicUse", Title="EconomicUse" },
                //new SysTable { Name="Geography Map", Title="Geography Map" },
                //new SysTable { Name="Author", Title="Author" },
                //new SysTable { Name="Citation", Title="Citation" },
                //new SysTable { Name="Literature", Title="Literature" },
                //new SysTable { Name="Regulation", Title="Regulation" },
                //new SysTable { Name="RegulationMap", Title="Regulation Map" }
            };
        }
    }
}