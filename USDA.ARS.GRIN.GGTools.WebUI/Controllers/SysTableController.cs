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
        public PartialViewResult _List(int sysUserId = 0)
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            try
            {
                //TODO
                //viewModel.SearchEntity = new SpeciesSearch { GenusID = genusId };
                //viewModel.Search();
                //viewModel.Entity.GenusID = genusId;

                //if (formatCode == "S")
                //{
                //    return PartialView("~/Views/Taxonomy/Species/Modals/_SelectListSimple.cshtml", viewModel);
                //}
                //else
                //{
                return PartialView("_List.cshtml", viewModel);
                //}
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