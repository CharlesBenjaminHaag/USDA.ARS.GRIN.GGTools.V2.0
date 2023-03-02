using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class SysGroupUserMapController : BaseController, IController<SysGroupUserMap>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public PartialViewResult _Add()
        {
            SysGroupUserMapViewModel viewModel = new SysGroupUserMapViewModel();
            //TODO
            return PartialView("~/Views/SysGroupUserMap/_Widget.cshtml", viewModel);
        }

        public PartialViewResult _Delete()
        {
            SysGroupUserMapViewModel viewModel = new SysGroupUserMapViewModel();
            //TODO
            return PartialView("~/Views/SysGroupUserMap/_Widget.cshtml", viewModel);
        }


        public PartialViewResult _List(int sysUserId = 0, string isAvailable="")
        {
            SysGroupUserMapViewModel viewModel = new SysGroupUserMapViewModel();
            try
            {
                viewModel.SearchEntity.SysUserID = sysUserId;
                viewModel.GetBySysUser(sysUserId, isAvailable);
                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult RenderWidget(int sysUserId)
        {
            SysGroupUserMapViewModel viewModel = new SysGroupUserMapViewModel();
            viewModel.GetBySysUser(sysUserId, "N");
            viewModel.GetBySysUser(sysUserId, "Y");
            return PartialView("~/Views/SysGroupUserMap/_Widget.cshtml", viewModel);
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
        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(SysGroupUserMap viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        // GET: SysGroupUserMap
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(SysGroupUserMap viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult _SelectList(int sysUserId = 0, int sysGroupId = 0)
        {
            SysGroupUserMapViewModel viewModel = new SysGroupUserMapViewModel();
            try
            {
                viewModel.SearchEntity.SysUserID = sysUserId;
                viewModel.SearchEntity.SysGroupID = sysGroupId;
                viewModel.Search();
                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        //public PartialViewResult _RenderLookupModal(string tableName = "")
        //{
        //    SysGroupUserMapViewModel viewModel = new SysGroupUserMapViewModel();
        //    viewModel.GetSysGroupUserMapsByTable(tableName);
        //    return PartialView("~/Views/SysGroupUserMap/Modals/_Lookup.cshtml", viewModel);
        //}

        //public PartialViewResult _RenderEditModal()
        //{
        //    SysGroupUserMapViewModel viewModel = new SysGroupUserMapViewModel();
        //    return PartialView("~/Views/SysGroupUserMap/Modals/_Edit.cshtml", viewModel);
        //}
    }
}