using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class SysTagController : Controller
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // GET: SysTag
        public ActionResult Index()
        {
            return View();
        }

        #region Components
        
        public PartialViewResult ComponentSysTagsByTable(string tableName, int idNumber)
        {
            try
            {
                SysTagViewModel viewModel = new SysTagViewModel();
                viewModel.SearchEntity.TableName = tableName;
                viewModel.SearchEntity.IDNumber = idNumber;
                viewModel.Search();
                return PartialView("~/Views/SysTag/Components/_SysTagsByTable.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }


        #endregion
    }
}