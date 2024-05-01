using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class SysFolderController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // GET: SysFolder
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult Add(SysFolderViewModel viewModel)
        {
            try
            {
                if (viewModel.Entity.ID == 0)
                {
                    viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Insert();
                }
                else
                {
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }
                viewModel.SearchEntity.ID = viewModel.Entity.ID;
                viewModel.Get();
                viewModel.EventAction = "ADD";
                return PartialView("~/Views/AppUserItemFolder/_Confirmation.cshtml", viewModel);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }


        public PartialViewResult GetEditModal()
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                return PartialView("~/Views/SysFolder/Modals/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}