using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
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
                    if (viewModel.ValidationMessages.Count > 0) return View(viewModel);
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
                viewModel.EventAction = "SEARCH";
                viewModel.Search();
                ModelState.Clear();
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
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
        public PartialViewResult LookupTaxa(FormCollection formCollection)
        {
            AuthorViewModel viewModel = new AuthorViewModel();

            if (!String.IsNullOrEmpty(formCollection["TableName"]))
            {
                viewModel.SearchEntity.TableName = formCollection["TableName"];
            }

            if (!String.IsNullOrEmpty(formCollection["AuthorityText"]))
            {
                viewModel.SearchEntity.AuthorityText = formCollection["AuthorityText"];
            }
            viewModel.Search();
            return PartialView("~/Views/Author/Modals/_AuthoritySelectList.cshtml", viewModel);
        }

        public ActionResult Add()
        {
            AuthorViewModel viewModel = new AuthorViewModel();
            viewModel.TableName = "taxonomy_author";
            viewModel.PageTitle = "Add Author";
            return View(BASE_PATH + "Edit.cshtml", viewModel);
        }

        public PartialViewResult FolderItems(int folderId)
        {
            try
            {
                AuthorViewModel viewModel = new AuthorViewModel();
                viewModel.EventAction = "SEARCH";
                viewModel.EventValue = "FOLDER";
                viewModel.SearchEntity.FolderID = folderId;
                viewModel.SearchFolderItems();
                ModelState.Clear();
                return PartialView("~/Views/Author/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        [HttpPost]
        public PartialViewResult LookupNotes(FormCollection formCollection)
        {
            string partialViewName = "~/Views/Author/Modals/_NoteSelectList.cshtml";
            AuthorViewModel viewModel = new AuthorViewModel();

            if (!String.IsNullOrEmpty(formCollection["TableName"]))
            {
                viewModel.SearchEntity.TableName = formCollection["TableName"];
            }

            if (!String.IsNullOrEmpty(formCollection["Note"]))
            {
                viewModel.SearchEntity.Note = formCollection["Note"];
            }

            viewModel.SearchNotes();
            return PartialView(partialViewName, viewModel);
        }

        public ActionResult Search(Author viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
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
