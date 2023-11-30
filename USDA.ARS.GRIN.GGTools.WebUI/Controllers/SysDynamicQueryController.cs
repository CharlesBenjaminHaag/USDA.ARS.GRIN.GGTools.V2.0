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
            AppUserItemFolderViewModel appUserItemFolderViewModel = new AppUserItemFolderViewModel();

            appUserItemFolderViewModel.SearchEntity.FolderType = "SQLQUERY";
            appUserItemFolderViewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            appUserItemFolderViewModel.Search();

            viewModel.TableName = "app_user_item_folder";
            viewModel.DataCollectionAppUserItemFolders = appUserItemFolderViewModel.DataCollection;
            return View(viewModel);
        }
        public ActionResult Edit(int entityId)
        {
            try
            {
                SysDynamicQueryViewModel viewModel = new SysDynamicQueryViewModel();
                viewModel.Entity.ID = entityId;
                viewModel.Get();
                return View(viewModel);
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
            string sqlQueryDrivingTable = String.Empty;

            try
            {
                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return View("~/Views/SysDynamicQuery/Index.cshtml", viewModel);
                }
                viewModel.Clean();
                
                sqlQueryDrivingTable = viewModel.GetSQLQueryDrivingTable();
                
                if (String.IsNullOrEmpty(sqlQueryDrivingTable))
                {
                    throw new NullReferenceException("Unable to determine the table being used in the query.");
                }
                 
                //TODO
                //If user has not supplied a PK in the search, flag -- id must be in first position.

                //TODO
                //Get friendly name of table
                SysTableViewModel sysTableViewModel = new SysTableViewModel();
                sysTableViewModel.SearchEntity.TableName = sqlQueryDrivingTable;
                sysTableViewModel.Search();

                viewModel.Search();
                viewModel.TableID = sysTableViewModel.Entity.ID;
                viewModel.TableName = sysTableViewModel.Entity.SysTableName;
                viewModel.TableTitle = sysTableViewModel.Entity.SysTableTitle;
                return View("~/Views/SysDynamicQuery/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                viewModel.ValidationMessages.Add(new Common.Library.ValidationMessage { Message = ex.Message });
                return View("~/Views/SysDynamicQuery/Index.cshtml", viewModel);
            }
        }

        [HttpPost]
        public PartialViewResult _Search(SysDynamicQueryViewModel viewModel)
        {
            try
            {
                //TODO: REFACTOR
                viewModel.SearchEntity.SQLStatement = viewModel.Entity.SQLStatement;

                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return PartialView("~/Views/SysDynamicQuery/_Edit.cshtml", viewModel);
                }

                //DEBUG Find tables in FROM clause (?)
                Regex regex = new Regex(@"\bJOIN\s+(?<Retrieve>[a-zA-Z\._\d\[\]]+)\b|\bFROM\s+(?<Retrieve>[a-zA-Z\._\d\[\]]+)\b|\bUPDATE\s+(?<Update>[a-zA-Z\._\d]+)\b|\bINSERT\s+(?:\bINTO\b)?\s+(?<Insert>[a-zA-Z\._\d]+)\b|\bTRUNCATE\s+TABLE\s+(?<Delete>[a-zA-Z\._\d]+)\b|\bDELETE\s+(?:\bFROM\b)?\s+(?<Delete>[a-zA-Z\._\d]+)\b");

                var obj = regex.Matches(viewModel.SearchEntity.SQLStatement);

                foreach (Match m in obj)
                {
                    var DEBUG = m.ToString().Substring(m.ToString().IndexOf(" ")).Trim();
                }

                // TODO POC: Based on "key" table in query, determine data type
                // of results.

                viewModel.Search();
                return PartialView("~/Views/SysDynamicQuery/_SearchResultsList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                viewModel.ValidationMessages.Add(new Common.Library.ValidationMessage { Message = ex.Message });
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
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
        [HttpPost]
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
                sysDynamicQueryViewModel.Entity.ID = viewModel.Entity.ID;
                sysDynamicQueryViewModel.Get();
                return PartialView("~/Views/SysDynamicQuery/_Edit.cshtml", sysDynamicQueryViewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        
    }
}