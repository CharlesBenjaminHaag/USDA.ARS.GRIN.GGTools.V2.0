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

        public PartialViewResult Save(CitationViewModel viewModel)
        {
            try
            {
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
                return _Edit(viewModel.Entity.ID, "");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }


        public PartialViewResult _ListFolderItems(int folderId)
        {
            CitationViewModel viewModel = new CitationViewModel();
            try
            {
                viewModel.EventAction = "FOLDER";
                viewModel.SearchEntity.FolderID = folderId;
                viewModel.GetFolderItems();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        [HttpPost]
        public ActionResult Search(CitationViewModel viewModel)
        {
            try
            {
                Session[SessionKeyName] = viewModel;
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
        public PartialViewResult _Search(CitationViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                return PartialView(BASE_PATH + "/Modals/_SelectList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
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

        //[HttpPost]
        //public PartialViewResult _List(FormCollection formCollection)
        //{
        //    CitationViewModel viewModel = new CitationViewModel();

        //    if (!String.IsNullOrEmpty(formCollection["FamilyID"]))
        //    {
        //        viewModel.SearchEntity.FamilyID = Int32.Parse(formCollection["FamilyID"]);
        //    }

        //    if (!String.IsNullOrEmpty(formCollection["GenusID"]))
        //    {
        //        viewModel.SearchEntity.GenusID = Int32.Parse(formCollection["GenusID"]);
        //    }

        //    if (!String.IsNullOrEmpty(formCollection["SpeciesID"]))
        //    {
        //        viewModel.SearchEntity.SpeciesID = Int32.Parse(formCollection["SpeciesID"]);
        //    }

        //    if (!String.IsNullOrEmpty(formCollection["FormatCode"]))
        //    {
        //        viewModel.EventValue = formCollection["FormatCode"];
        //    }

        //    viewModel.Search();
        //    return PartialView(BASE_PATH + "_List.cshtml", viewModel);
        //}

        public ActionResult Index()
        {
            try
            {
                CitationViewModel viewModel = new CitationViewModel();

                string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
                if (Session[targetKey] != null)
                {
                    viewModel = Session[targetKey] as CitationViewModel;
                }

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

        public PartialViewResult _Edit(int entityId, string isClone = "N", string eventAction = "", string eventValue = "")
        {
            CitationViewModel viewModel = new CitationViewModel();
            try
            {
                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit Citation [{0}]: {1}", viewModel.Entity.ID, viewModel.Entity.AssembledName);

                if (isClone == "Y")
                {
                    CitationViewModel cloneViewModel = new CitationViewModel();
                    cloneViewModel.Entity = viewModel.Entity;
                    cloneViewModel.EventAction = eventAction;
                    cloneViewModel.EventValue = eventValue;
                    cloneViewModel.Entity.SpeciesID = 0;
                    cloneViewModel.Entity.FamilyID = 0;
                    cloneViewModel.Entity.GenusID = 0;
                    return PartialView("~/Views/Taxonomy/Citation/_Edit.cshtml", cloneViewModel);
                }
                else
                {
                    if (viewModel.Entity.FamilyID > 0) viewModel.EventValue = "taxonomy_family_map";
                    if (viewModel.Entity.GenusID > 0) viewModel.EventValue = "taxonomy_genus";
                    if (viewModel.Entity.SpeciesID > 0) viewModel.EventValue = "taxonomy_species";
                }
                return PartialView("~/Views/Taxonomy/Citation/_Edit.cshtml", viewModel);
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
                CitationViewModel viewModel = new CitationViewModel();
                viewModel.TableName = "citation";
                viewModel.TableCode = "Citation";
                viewModel.EventAction = "Edit";

                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.PageTitle = String.Format("Edit Citation [{0}]: {1}", viewModel.Entity.ID, viewModel.Entity.AssembledName);
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
                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return View(BASE_PATH + "Edit.cshtml", viewModel);
                }

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
       
        //public PartialViewResult _Save(CitationViewModel viewModel)
        //{
        //    try
        //    {
        //        viewModel.Entity.TableName = viewModel.TableName;

        //        //if (!viewModel.Validate())
        //        //{
        //        //    if (viewModel.ValidationMessages.Count > 0) return View(viewModel);
        //        //}

        //        if (viewModel.Entity.ID == 0)
        //        {
        //            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
        //            viewModel.Insert();
        //        }
        //        else
        //        {
        //            viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
        //            viewModel.Update();
        //        }
        //        return PartialView("~/Views/Taxonomy/Citation/_Edit.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}

        public ActionResult Add(int literatureId = 0, int familyMapId = 0, int genusId = 0, int speciesId = 0, string eventValue = "")
        {
            try 
            { 
                CitationViewModel viewModel = new CitationViewModel();
                viewModel.EventAction = "Add";
                viewModel.EventValue = eventValue;
                viewModel.TableName = "citation";
                viewModel.TableCode = "Citation";
                viewModel.Entity.LiteratureID = literatureId;
                viewModel.Entity.FamilyID = familyMapId;
                viewModel.Entity.GenusID = genusId;
                viewModel.Entity.SpeciesID = speciesId;
                viewModel.PageTitle = viewModel.EventAction + " " + viewModel.TableCode;
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Entity.CreatedByCooperatorName = AuthenticatedUser.FullName;
                viewModel.Entity.CreatedDate = System.DateTime.Now;

                if (familyMapId > 0)
                {
                    viewModel.Entity.FamilyID = familyMapId;
                    FamilyMapViewModel familyMapViewModel = new FamilyMapViewModel();
                    familyMapViewModel.SearchEntity.ID = familyMapId;
                    familyMapViewModel.Search();
                    viewModel.Entity.FamilyID = familyMapViewModel.Entity.ID;
                    viewModel.Entity.FamilyName = familyMapViewModel.Entity.FamilyName;
                }

                if (genusId > 0)
                {
                    GenusViewModel genusViewModel = new GenusViewModel();
                    genusViewModel.SearchEntity.ID = genusId;
                    genusViewModel.Search();
                    viewModel.Entity.GenusID = genusViewModel.Entity.ID;
                    viewModel.Entity.GenusName = genusViewModel.Entity.Name;
                }

                if (speciesId > 0)
                {
                    SpeciesViewModel speciesViewModel = new SpeciesViewModel();
                    speciesViewModel.SearchEntity.ID = speciesId;
                    speciesViewModel.Search();
                    viewModel.Entity.SpeciesID = speciesViewModel.Entity.ID;
                    viewModel.Entity.SpeciesName = speciesViewModel.Entity.Name;
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

            //if (!String.IsNullOrEmpty(formCollection["StandardAbbreviation"]))
            //{
            //    viewModel.Entity.StandardAbbreviation = formCollection["StandardAbbreviation"];
            //}

            if (!String.IsNullOrEmpty(formCollection["CitationTitle"]))
            {
                viewModel.Entity.CitationTitle = formCollection["CitationTitle"];
            }

            //if (!String.IsNullOrEmpty(formCollection["EditorAuthorName"]))
            //{
            //    viewModel.Entity.EditorAuthorName = formCollection["EditorAuthorName"];
            //}

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
                viewModel.Entity.Reference = formCollection["VolumeOrPage"];
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

        /// <summary>
        /// Creates a copy of a specified citation, and renders a view allowing a side-by-side
        /// comparison and edit.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        //public ActionResult Clone(int entityId)
        //{
        //    CitationViewModel viewModel = new CitationViewModel();
        //    CitationViewModel viewModelClone = new CitationViewModel();
        //    try
        //    {
        //        viewModel.Get(entityId);
        //        viewModelClone.Entity = viewModel.Entity;
        //        viewModelClone.TableName = "citation";
        //        viewModelClone.TableCode = "Citation";
        //        viewModelClone.PageTitle = "Add Citation (Clone)";
        //        viewModelClone.Entity.FamilyID = 0;
        //        viewModelClone.Entity.FamilyName = String.Empty;
        //        viewModelClone.Entity.GenusID = 0;
        //        viewModelClone.Entity.GenusName = String.Empty;
        //        viewModelClone.Entity.SpeciesID = 0;
        //        viewModelClone.Entity.SpeciesName = String.Empty;
        //        return View(BASE_PATH + "/Clone.cshtml", viewModelClone);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return RedirectToAction("InternalServerError", "Error");
        //    }
        //}
        public ActionResult Clone(int entityId, string eventAction="", string eventValue="")
        {
            // Retrieve citation to be cloned.
            CitationViewModel viewModel = new CitationViewModel();
            CitationViewModel cloneViewModel = new CitationViewModel();

            viewModel.SearchEntity.ID = entityId;
            viewModel.Search();

            // Create copy of source citation, resetting taxon attributes.
            viewModel.CloneEntity = viewModel.Entity;
            viewModel.EventAction = eventAction;
            viewModel.EventValue = eventValue;
            viewModel.CloneEntity.FamilyID = 0;
            viewModel.CloneEntity.FamilyName = String.Empty;
            viewModel.CloneEntity.GenusID = 0;
            viewModel.CloneEntity.GenusName = String.Empty;
            viewModel.CloneEntity.SpeciesID = 0;
            viewModel.CloneEntity.SpeciesName = String.Empty;
            return View(BASE_PATH + "Clone.cshtml", viewModel);
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

            // If the table name is not a taxon table, load species citations.
            if ((tableName != "taxonomy_species") && 
                (tableName != "taxonomy_genus") && 
                tableName != "taxonomy_family_map")
                {
                    viewModel.SearchEntity.SpeciesID = speciesId;
                    viewModel.Search();
                }
            return PartialView(BASE_PATH + "/Modals/_Lookup.cshtml", viewModel);
        }
        public PartialViewResult RenderEditModal(int entityId)
        {
            CitationViewModel viewModel = new CitationViewModel();

            if (entityId > 0)
            {
                viewModel.Get(entityId);
            }
            return PartialView("~/Views/Taxonomy/Citation/Modals/_Edit.cshtml", viewModel);
        }

        public PartialViewResult RenderWidget(int entityId)
        {
            CitationViewModel viewModel = new CitationViewModel();

            try
            {
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                }
                return PartialView("~/Views/Taxonomy/Citation/_Widget.cshtml", viewModel);
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
                if (!String.IsNullOrEmpty(coll["TypeCode"]))
                {
                    vm.SearchEntity.TypeCode = coll["TypeCode"];
                }

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
                    vm.SearchEntity.CitationYear = Int32.Parse(coll["CitationYear"]);
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
