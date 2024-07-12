using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USDA.ARS.GRIN.GGTools.WebUI.Helpers
{ 
public static class ViewHelper
{
    public static string GetPartialViewName(string queryString)
    {
        // Retrieve the value from the query string
        string value = HttpContext.Current.Request.QueryString[queryString];

        // Determine which partial view to render based on the query string value
        switch (value)
        {
            case "view1":
                return "_PartialView1";
            case "view2":
                return "_PartialView2";
            default:
                return "_DefaultPartialView";
        }
    }
}
}