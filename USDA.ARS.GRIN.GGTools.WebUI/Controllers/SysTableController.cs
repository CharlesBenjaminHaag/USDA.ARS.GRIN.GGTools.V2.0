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
    public class SysTableController : BaseController, IController<SysTableViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public PartialViewResult _List()
        {
            SysTableViewModel viewModel = new SysTableViewModel();
            try
            {
                viewModel.SearchEntity.DatabaseAreaCode = "TAXONOMY";
                viewModel.Search();
                return PartialView("~/Views/SysTable/_MenuList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListFolderItems(int folderId)
        {
            try
            {
                return PartialView("~/Views/Shared/_UnderConstruction.cshtml");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult RenderWidget()
        {
            SysTableViewModel viewModel = new SysTableViewModel();
            return PartialView("~/Views/SysTable/_Widget.cshtml", viewModel);
        }
        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(SysTableViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        // GET: SysTable
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult _RenderLookupModal(string tableName = "")
        {
            SysPermissionViewModel viewModel = new SysPermissionViewModel();
            viewModel.SearchEntity.SysUserID = AuthenticatedUser.SysUserID;
            viewModel.SearchEntity.TableName = tableName;
            viewModel.GetPermissionsByTable();
            return PartialView("~/Views/SysTable/Modals/_Lookup.cshtml", viewModel);
        }

        public ActionResult Search(SysTableViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}