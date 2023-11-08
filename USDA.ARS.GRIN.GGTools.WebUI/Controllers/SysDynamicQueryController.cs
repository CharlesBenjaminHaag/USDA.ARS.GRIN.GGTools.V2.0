using NLog;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class SysDynamicQueryController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        // GET: DynamicQuery
        public ActionResult Index()
        {
            SysDynamicQueryViewModel viewModel = new SysDynamicQueryViewModel();
            return View(viewModel);
        }
        public ActionResult Edit(int entityId)
        {
            try
            {
                
                AppUserItemFolderViewModel appUserItemFolderViewModel = new AppUserItemFolderViewModel();
                AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                SysDynamicQueryViewModel sysDynamicQueryViewModel = new SysDynamicQueryViewModel();

                appUserItemFolderViewModel.SearchEntity.ID = entityId;
                appUserItemFolderViewModel.Get();

                appUserItemListViewModel.SearchEntity.AppUserItemFolderID = entityId;
                appUserItemListViewModel.Search();

                sysDynamicQueryViewModel.PageTitle = "Edit Saved Search";
                sysDynamicQueryViewModel.Entity.ParentID = appUserItemListViewModel.Entity.ID;
                sysDynamicQueryViewModel.Entity.ID = appUserItemFolderViewModel.Entity.ID;
                sysDynamicQueryViewModel.Entity.Title = appUserItemFolderViewModel.Entity.FolderName;
                sysDynamicQueryViewModel.Entity.Description = appUserItemFolderViewModel.Entity.Description;
                sysDynamicQueryViewModel.Entity.SQLStatement = appUserItemListViewModel.Entity.Properties;
                sysDynamicQueryViewModel.Entity.CreatedByCooperatorID = appUserItemFolderViewModel.Entity.CreatedByCooperatorID;
                sysDynamicQueryViewModel.Entity.CreatedByCooperatorName = appUserItemFolderViewModel.Entity.CreatedByCooperatorName;
                sysDynamicQueryViewModel.Entity.CreatedDate = appUserItemFolderViewModel.Entity.CreatedDate;
                sysDynamicQueryViewModel.Entity.ModifiedByCooperatorID = appUserItemFolderViewModel.Entity.ModifiedByCooperatorID;
                sysDynamicQueryViewModel.Entity.ModifiedByCooperatorName = appUserItemFolderViewModel.Entity.ModifiedByCooperatorName;
                sysDynamicQueryViewModel.Entity.ModifiedDate = appUserItemFolderViewModel.Entity.ModifiedDate;
                return View(sysDynamicQueryViewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        [HttpPost]
        public ActionResult Search(SysDynamicQueryViewModel viewModel)
        {
            try
            {
                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return View("~/Views/SysDynamicQuery/Index.cshtml", viewModel);
                }

                //DEBUG Find tables in FROM clause (?)

                Regex regex = new Regex(@"\bJOIN\s+(?<Retrieve>[a-zA-Z\._\d\[\]]+)\b|\bFROM\s+(?<Retrieve>[a-zA-Z\._\d\[\]]+)\b|\bUPDATE\s+(?<Update>[a-zA-Z\._\d]+)\b|\bINSERT\s+(?:\bINTO\b)?\s+(?<Insert>[a-zA-Z\._\d]+)\b|\bTRUNCATE\s+TABLE\s+(?<Delete>[a-zA-Z\._\d]+)\b|\bDELETE\s+(?:\bFROM\b)?\s+(?<Delete>[a-zA-Z\._\d]+)\b");

                var obj = regex.Matches(viewModel.SearchEntity.SQLStatement);

                foreach (Match m in obj)
                {
                    var DEBUG = m.ToString().Substring(m.ToString().IndexOf(" ")).Trim();

                }


                viewModel.Search();
                return View("~/Views/SysDynamicQuery/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                viewModel.ValidationMessages.Add(new Common.Library.ValidationMessage { Message = ex.Message });
                return View("~/Views/SysDynamicQuery/Index.cshtml", viewModel);
            }
        }
        public PartialViewResult RenderEditModal()
        {
            try
            {
                SysDynamicQueryViewModel viewModel = new SysDynamicQueryViewModel();
                return PartialView("~/Views/SysDynamicQuery/Modals/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult SaveSearch(SysDynamicQueryViewModel viewModel)
        {
            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.SaveSearch();
                return PartialView("~/Views/SysDynamicQuery/Modals/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _Edit(int entityId)
        {
            SysDynamicQueryViewModel viewModel = new SysDynamicQueryViewModel();
            
            try
            {
                viewModel.SearchEntity.ID = entityId;
                viewModel.Search();
                return PartialView("~/Views/SysDynamicQuery/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        [HttpPost]
        public PartialViewResult _Edit(SysDynamicQueryViewModel viewModel)
        {
            AppUserItemFolderViewModel appUserItemFolderViewModel = new AppUserItemFolderViewModel();
            AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
            try
            {
                if (viewModel.Entity.ID > 0)
                {
                    // TODO: REFACTOR -- CBH 11/8/23
                    // Update folder.
                    appUserItemFolderViewModel.SearchEntity.ID = viewModel.Entity.ID;
                    appUserItemFolderViewModel.Search();
                    appUserItemFolderViewModel.Entity.FolderName = viewModel.Entity.Title;
                    appUserItemFolderViewModel.Entity.Description = viewModel.Entity.Description;
                    appUserItemFolderViewModel.Entity.IsFavorite = viewModel.Entity.IsFavorite;
                    appUserItemFolderViewModel.Entity.CreatedByCooperatorID = viewModel.Entity.CreatedByCooperatorID;
                    appUserItemFolderViewModel.Entity.ModifiedByCooperatorID = viewModel.Entity.ModifiedByCooperatorID;
                    appUserItemFolderViewModel.Update();

                    // Update related app user item list record with any changes
                    // to SQL statement.
                    appUserItemListViewModel.SearchEntity.ID = viewModel.Entity.ParentID;
                    appUserItemListViewModel.Search();
                    appUserItemListViewModel.Entity.Properties = viewModel.Entity.SQLStatement;
                    appUserItemListViewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    appUserItemListViewModel.Update();
                }

                SysDynamicQueryViewModel sysDynamicQueryViewModel = new SysDynamicQueryViewModel();
                viewModel.SearchEntity.ID = viewModel.Entity.ID;
                viewModel.Search();
                return PartialView("~/Views/SysDynamicQuery/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    }
}