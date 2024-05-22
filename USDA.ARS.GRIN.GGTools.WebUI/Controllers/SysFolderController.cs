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

                    // If there are tag attributes, add specified tag.
                    if (!String.IsNullOrEmpty(viewModel.TagEntity.TagText))
                    {
                        SysTagViewModel sysTagViewModel = new SysTagViewModel();
                        sysTagViewModel.Entity.TagText = viewModel.TagEntity.TagText;
                        sysTagViewModel.Entity.TagFormatString = viewModel.TagEntity.TagFormatString;
                        sysTagViewModel.Entity.TableName = "sys_folder";
                        sysTagViewModel.Entity.IDNumber = viewModel.Entity.ID;
                        sysTagViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                        sysTagViewModel.Insert();
                    }
                }
                else
                {
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }

                // Re-retrieve new folder to verify existence.
                viewModel.SearchEntity.ID = viewModel.Entity.ID;
                viewModel.Get(viewModel.Entity.ID);
                viewModel.EventAction = "ADD";
                return PartialView("~/Views/SysFolder/Components/_Confirmation.cshtml", viewModel);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                viewModel.Get(entityId);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        //public PartialViewResult GetList()
        //{ }

        //public PartialViewResult GetItemList()
        //{ }

        public PartialViewResult GetEditModal()
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                return PartialView("~/Views/SysFolder/Modals/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        #region Components

        /// <summary>
        /// Retrieves an icon-formatted list of folders. Defaults to folders owned by the logged-in user.
        /// </summary>
        /// <returns></returns>
        
        public PartialViewResult ComponentListWithIcons()
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Search();
                return PartialView("~/Views/SysFolder/Components/_ListWithIcons.cshtml", viewModel);
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