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
    public class CitationController : BaseController, IController<CitationViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Citation/";
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
        [HttpPost]
        public ActionResult Search(CitationViewModel vm)
        {
            try
            {
                vm.Search();
                ModelState.Clear();
                return View(BASE_PATH + "Index.cshtml", vm);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public PartialViewResult _List(int familyId = 0, int genusId = 0, int speciesId = 0, int literatureId = 0)
        {
            CitationViewModel viewModel = new CitationViewModel();
            viewModel.SearchEntity.FamilyID = familyId;
            viewModel.SearchEntity.GenusID = genusId;
            viewModel.SearchEntity.SpeciesID = speciesId;
            viewModel.SearchEntity.LiteratureID = literatureId;
            viewModel.Search();
            return PartialView(BASE_PATH + "_List.cshtml", viewModel);
        }

        [HttpPost]
        public PartialViewResult _List(FormCollection formCollection)
        {
            CitationViewModel viewModel = new CitationViewModel();

            if (!String.IsNullOrEmpty(formCollection["FamilyID"]))
            {
                viewModel.SearchEntity.FamilyID = Int32.Parse(formCollection["FamilyID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["GenusID"]))
            {
                viewModel.SearchEntity.GenusID = Int32.Parse(formCollection["GenusID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["SpeciesID"]))
            {
                viewModel.SearchEntity.SpeciesID = Int32.Parse(formCollection["SpeciesID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["FormatCode"]))
            {
                viewModel.EventValue = formCollection["FormatCode"];
            }

            viewModel.Search();
            return PartialView(BASE_PATH + "_List.cshtml", viewModel);
        }

        public ActionResult Index()
        {
            try
            {
                CitationViewModel viewModel = new CitationViewModel();
                viewModel.PageTitle = "Citation Search";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
     
        //public ActionResult _Search()
        //{
        //    CitationViewModel vm = new CitationViewModel();
        //    return PartialView("~/Views/Citation/_Search.cshtml", vm);
        //}

        public ActionResult Edit(int entityId)
        {
            try
            {
                CitationViewModel viewModel = new CitationViewModel();
                viewModel.TableName = "citation";
                viewModel.TableCode = "Citation";
                viewModel.EventAction = "Edit";

                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.PageTitle = String.Format("Edit Citation [{0}]", viewModel.Entity.ID);
                }
                else
                {
                    viewModel.PageTitle = String.Format("Add Citation");
                }
                return View(BASE_PATH + "Edit.cshtml",  viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(CitationViewModel viewModel)
        {
            try
            {
                viewModel.Entity.TableName = viewModel.TableName;

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
                return RedirectToAction("Edit", "Citation", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _Save(CitationViewModel viewModel)
        {
            try
            {
                viewModel.Entity.TableName = viewModel.TableName;

                //if (!viewModel.Validate())
                //{
                //    if (viewModel.ValidationMessages.Count > 0) return View(viewModel);
                //}

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
                return PartialView("~/Views/Taxonomy/Citation/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult Add()
        {
            CitationViewModel viewModel = new CitationViewModel();
            viewModel.EventAction = "Add";
            viewModel.TableName = "citation";
            viewModel.TableCode = "Citation";
            viewModel.PageTitle = viewModel.EventAction + " " + viewModel.TableCode;
            return View(BASE_PATH + "Edit.cshtml", viewModel);
        }
        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            CitationViewModel viewModel = new CitationViewModel();

            if (!String.IsNullOrEmpty(formCollection["TableName"]))
            {
                viewModel.Entity.TableName = formCollection["TableName"];
            }

            if (!String.IsNullOrEmpty(formCollection["EntityID"]))
            {
                viewModel.ReferencedEntityID = Int32.Parse(formCollection["EntityID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["LiteratureID"]))
            {
                viewModel.Entity.LiteratureID = Int32.Parse(formCollection["LiteratureID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["CitationID"]))
            {
                viewModel.Entity.CitationID = Int32.Parse(formCollection["CitationID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["IDList"]))
            {
                viewModel.Entity.ItemIDList = formCollection["IDList"];
            }

            if (!String.IsNullOrEmpty(formCollection["EntityIDList"]))
            {
                viewModel.EntityIDList = formCollection["EntityIDList"];
            }

            if (!String.IsNullOrEmpty(formCollection["StandardAbbreviation"]))
            {
                viewModel.Entity.StandardAbbreviation = formCollection["StandardAbbreviation"];
            }

            if (!String.IsNullOrEmpty(formCollection["CitationTitle"]))
            {
                viewModel.Entity.CitationTitle = formCollection["CitationTitle"];
            }

            if (!String.IsNullOrEmpty(formCollection["EditorAuthorName"]))
            {
                viewModel.Entity.EditorAuthorName = formCollection["EditorAuthorName"];
            }

            if (!String.IsNullOrEmpty(formCollection["CitationYear"]))
            {
                viewModel.Entity.CitationYear = Int32.Parse(formCollection["CitationYear"]);
            }

            if (!String.IsNullOrEmpty(formCollection["DOIReference"]))
            {
                viewModel.Entity.DOIReference = formCollection["DOIReference"];
            }

            if (!String.IsNullOrEmpty(formCollection["VolumeOrPage"]))
            {
                viewModel.Entity.VolumeOrPage = formCollection["VolumeOrPage"];
            }

            if (!String.IsNullOrEmpty(formCollection["Note"]))
            {
                viewModel.Entity.Note = formCollection["Note"];
            }

            switch (viewModel.Entity.TableName)
            {
                case "taxonomy_family_map":
                    viewModel.Entity.FamilyID = viewModel.ReferencedEntityID;
                    break;
                case "taxonomy_genus":
                    viewModel.Entity.GenusID = viewModel.ReferencedEntityID;
                    break;
                case "taxonomy_species":
                    viewModel.Entity.SpeciesID = viewModel.ReferencedEntityID;
                    break;
            }

            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            viewModel.Insert();
            viewModel.Get(viewModel.Entity.ID);

            return Json(new { citation = viewModel.Entity }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Clone(FormCollection formCollection)
        {
            CitationViewModel viewModel = new CitationViewModel();
            CitationViewModel cloneViewModel = new CitationViewModel();

            // Retrieve and clone the citation whose ID is passed as a parameter.
            if (!String.IsNullOrEmpty(formCollection["CitationID"]))
            {
                cloneViewModel.SearchEntity.ID = Int32.Parse(formCollection["CitationID"]);
            }
            cloneViewModel.Search();
            viewModel.Entity = cloneViewModel.Entity;

            if (!String.IsNullOrEmpty(formCollection["TableName"]))
            {
                viewModel.TableName = formCollection["TableName"];
            }
             
            if (!String.IsNullOrEmpty(formCollection["FamilyID"]))
            {
                viewModel.Entity.FamilyID = Int32.Parse(formCollection["FamilyID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["GenusID"]))
            {
                viewModel.Entity.GenusID = Int32.Parse(formCollection["GenusID"]);
            }

            if (!String.IsNullOrEmpty(formCollection["SpeciesID"]))
            {
                viewModel.Entity.SpeciesID = Int32.Parse(formCollection["SpeciesID"]);
            }
            viewModel.Entity.ID = 0;
            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            viewModel.Insert();

            return Json(new { citation = viewModel.Entity }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(int folderId)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        // ======================================================================================
        // MODALS 
        // ======================================================================================
        public PartialViewResult RenderLookupModal(string tableName = "", int entityId = 0, int familyId = 0, int genusId = 0, int speciesId = 0, string isMultiSelect = "")
        {
            CitationViewModel viewModel = new CitationViewModel();
            viewModel.IsMultiSelect = isMultiSelect;

            // TEMP: If no table name is passed in, assume use of species citations.
            if (String.IsNullOrEmpty(tableName))
            {
                tableName = "taxonomy_species";
                entityId = speciesId;
            }
            viewModel.GetTaxonCitations(tableName, entityId);
            return PartialView(BASE_PATH + "/Modals/_Lookup.cshtml", viewModel);
        }
        public PartialViewResult RenderEditModal(int entityId)
        {
            CitationViewModel viewModel = new CitationViewModel();
            viewModel.Get(entityId);
            return PartialView("~/Views/Taxonomy/Citation/Modals/_Edit.cshtml", viewModel);
        }

        public PartialViewResult RenderWidget(int entityId)
        {
            try
            {
                if (entityId > 0)
                {
                    CitationViewModel viewModel = new CitationViewModel();
                    viewModel.Get(entityId);
                    return PartialView("~/Views/Taxonomy/Citation/_Widget.cshtml", viewModel);
                }
                else
                {
                    return PartialView("~/Views/Taxonomy/Citation/_NotCited.cshtml");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    

        [HttpPost]
        public PartialViewResult Lookup(FormCollection coll)
        {
            CitationViewModel vm = new CitationViewModel();

            try
            {
                //if (!String.IsNullOrEmpty(coll["SpeciesID"]))
                //{
                //    vm.SearchEntity.SpeciesID = Int32.Parse(coll["SpeciesID"]);
                //}

                if (!String.IsNullOrEmpty(coll["LiteratureTypeCode"]))
                {
                    vm.SearchEntity.LiteratureTypeCode = coll["LiteratureTypeCode"];
                }

                if (!String.IsNullOrEmpty(coll["StandardAbbreviation"]))
                {
                    vm.SearchEntity.StandardAbbreviation = coll["StandardAbbreviation"];
                }

                if (!String.IsNullOrEmpty(coll["Abbreviation"]))
                {
                    vm.SearchEntity.Abbreviation = coll["Abbreviation"];
                }

                if (!String.IsNullOrEmpty(coll["CitationTitle"]))
                {
                    vm.SearchEntity.CitationTitle = coll["CitationTitle"];
                }

                if (!String.IsNullOrEmpty(coll["EditorAuthorName"]))
                {
                    vm.SearchEntity.EditorAuthorName = coll["EditorAuthorName"];
                }

                if (!String.IsNullOrEmpty(coll["CitationYear"]))
                {
                    vm.SearchEntity.CitationYear = coll["CitationYear"];
                }

                if (!String.IsNullOrEmpty(coll["IsMultiSelect"]))
                {
                    vm.IsMultiSelect = coll["IsMultiSelect"];
                }
      
                vm.Search();
                return PartialView(BASE_PATH + "/Modals/_SelectList.cshtml", vm);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
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
                CitationViewModel viewModel = new CitationViewModel();
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
