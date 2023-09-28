using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class AuthorController : BaseController, IController<AuthorViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Author/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        public PartialViewResult _ListFolderItems(int folderId)
        {
            AuthorViewModel viewModel = new AuthorViewModel();
            try
            {
                viewModel.EventAction = "FOLDER";
                viewModel.SearchEntity.FolderID = folderId;
                viewModel.GetFolderItems();
                return PartialView(BASE_PATH + "_ListFolder.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListFamilyReferences(string shortName)
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
        public PartialViewResult _ListGenusReferences(string shortName)
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
        public PartialViewResult _ListSpeciesReferences(string shortName)
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

        public ActionResult Index()
        {
            try
            {
                AuthorViewModel viewModel = new AuthorViewModel();
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.PageTitle = "Author Search";
                viewModel.TableName = "taxonomy_author";

                string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
                if (Session[targetKey] != null)
                {
                    viewModel = Session[targetKey] as AuthorViewModel;
                }

                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        
        public ActionResult Edit(int entityId)
        {
            try
            {
                AuthorViewModel viewModel = new AuthorViewModel();
                viewModel.TableName = "taxonomy_author";
                viewModel.TableCode = "Author";
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.EventAction = "Edit";
                    viewModel.PageTitle = String.Format("Edit Author [{0}]: {1}", entityId, viewModel.Entity.FullName);
                }
                else
                {
                    viewModel.EventAction = "Add";
                    viewModel.PageTitle = "Add Author";
                }
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(AuthorViewModel viewModel)
        {
            try
            {
                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return View(BASE_PATH + "Edit.cshtml", viewModel);
                }

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
                return RedirectToAction("Edit", "Author", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public ActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Search(AuthorViewModel viewModel)
        {
            try
            {
                Session[SessionKeyName] = viewModel;
                viewModel.EventAction = "SEARCH";
                viewModel.SearchEntity.IsShortNameExactMatch = viewModel.FromBool(viewModel.SearchEntity.IsShortNameExactMatchOption);
                viewModel.Search();
                ModelState.Clear();

                // Save search if attribs supplied.
                if ((viewModel.EventAction == "SEARCH") && (viewModel.EventValue == "SAVE"))
                {
                    AppUserItemFolderViewModel appUserItemFolderViewModel = new AppUserItemFolderViewModel();
                    appUserItemFolderViewModel.Entity.FolderName = viewModel.SearchEntity.SearchTitle;
                    appUserItemFolderViewModel.Entity.Description = viewModel.SearchEntity.SearchDescription;
                    appUserItemFolderViewModel.Entity.Category = "";
                    appUserItemFolderViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    appUserItemFolderViewModel.Insert();

                    if (appUserItemFolderViewModel.Entity.ID <= 0)
                    {
                        throw new IndexOutOfRangeException("Error adding new folder.");
                    }

                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.Entity.AppUserItemFolderID = appUserItemFolderViewModel.Entity.ID;
                    appUserItemListViewModel.Entity.CooperatorID = AuthenticatedUser.CooperatorID;
                    appUserItemListViewModel.Entity.TabName = "GGTools Taxon Editor";
                    appUserItemListViewModel.Entity.ListName = appUserItemFolderViewModel.Entity.FolderName;
                    appUserItemListViewModel.Entity.IDNumber = 0;
                    appUserItemListViewModel.Entity.IDType = "FOLDER";
                    appUserItemListViewModel.Entity.SortOrder = 0;
                    appUserItemListViewModel.Entity.Title = appUserItemFolderViewModel.Entity.FolderName;
                    appUserItemListViewModel.Entity.Description = "Added in GGTools Taxonomy Editor";
                    appUserItemListViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CreatedByCooperatorID;
                    appUserItemListViewModel.Entity.Properties = viewModel.SerializeToXml(viewModel.SearchEntity);
                    appUserItemListViewModel.Insert();
                }

                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _Search(AuthorViewModel viewModel)
        {
            try
            {
                viewModel.EventAction = "SEARCH";
                viewModel.SearchEntity.IsShortNameExactMatch = viewModel.FromBool(viewModel.SearchEntity.IsShortNameExactMatchOption);
                viewModel.Search();
                return PartialView(BASE_PATH + "/Modals/_SelectList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml", viewModel);
            }
        }

        [HttpPost]
        public PartialViewResult Lookup(FormCollection formCollection)
        {
            AuthorViewModel viewModel = new AuthorViewModel();

            if (!String.IsNullOrEmpty(formCollection["TableName"]))
            {
                viewModel.SearchEntity.TableName = formCollection["TableName"];
            }

            if (!String.IsNullOrEmpty(formCollection["AuthorLookupFullName"]))
            {
                viewModel.SearchEntity.FullName = formCollection["AuthorLookupFullName"];
            }

            if (!String.IsNullOrEmpty(formCollection["AuthorLookupShortName"]))
            {
                viewModel.SearchEntity.ShortName = formCollection["AuthorLookupShortName"];
            }

            if (!String.IsNullOrEmpty(formCollection["AuthorLookupIsExactMatch"]))
            {
                viewModel.SearchEntity.IsShortNameExactMatch = formCollection["AuthorLookupIsExactMatch"];
            }
            viewModel.Search();
            return PartialView(BASE_PATH + "/Modals/_SelectList.cshtml", viewModel);
        }

        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult RenderLookupModal()
        {
            try
            {
                AuthorViewModel viewModel = new AuthorViewModel();
                return PartialView("~/Views/Taxonomy/Author/Modals/_Lookup.cshtml",viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        //[HttpPost]
        //public PartialViewResult LookupTaxa(FormCollection formCollection)
        //{
        //    AuthorViewModel viewModel = new AuthorViewModel();

        //    if (!String.IsNullOrEmpty(formCollection["TableName"]))
        //    {
        //        viewModel.SearchEntity.TableName = formCollection["TableName"];
        //    }

        //    if (!String.IsNullOrEmpty(formCollection["AuthorityText"]))
        //    {
        //        viewModel.SearchEntity.AuthorityText = formCollection["AuthorityText"];
        //    }
        //    viewModel.Search();
        //    return PartialView("~/Views/Author/Modals/_AuthoritySelectList.cshtml", viewModel);
        //}

        public ActionResult Add()
        {
            try
            {
                AuthorViewModel viewModel = new AuthorViewModel();
                viewModel.TableName = "taxonomy_author";
                viewModel.PageTitle = "Add Author";
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Search(Author viewModel)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
                AuthorViewModel viewModel = new AuthorViewModel();
                viewModel.Entity.ID = Int32.Parse(GetFormFieldValue(formCollection, "EntityID"));
                viewModel.TableName = GetFormFieldValue(formCollection, "TableName");
                viewModel.Delete();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
