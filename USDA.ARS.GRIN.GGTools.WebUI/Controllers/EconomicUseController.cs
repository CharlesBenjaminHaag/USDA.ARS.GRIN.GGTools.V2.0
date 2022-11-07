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
    public class EconomicUseController : BaseController, IController<EconomicUseViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/EconomicUse/";
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
        public ActionResult Add(string eventValue = "", int entityId = 0)
        {
            try
            {
                EconomicUseViewModel viewModel = new EconomicUseViewModel();
                viewModel.EventAction = "Add";
                viewModel.EventValue = "EconomicUse";
                viewModel.TableName = "taxonomy_use";
                viewModel.PageTitle = "Add Economic Use";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

                if (entityId > 0)
                {
                    SpeciesViewModel speciesViewModel = new SpeciesViewModel();
                    speciesViewModel.SearchEntity = new SpeciesSearch { ID = entityId };
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

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                EconomicUseViewModel viewModel = new EconomicUseViewModel();
                viewModel.TableName = "taxonomy_use";
                viewModel.TableCode = "EconomicUse";
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.EventAction = "Edit";
                    viewModel.PageTitle = String.Format("Edit Economic Use [{0}]: {1}", entityId, viewModel.ToTitleCase(viewModel.Entity.ExtendedName));
                }
                else
                {
                    viewModel.EventAction = "Add";
                    viewModel.PageTitle = "Add  Economic Use";
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
        public ActionResult Edit(EconomicUseViewModel viewModel)
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
                return RedirectToAction("Edit", "EconomicUse", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            EconomicUseViewModel viewModel = new EconomicUseViewModel();

            try
            {
                if (!String.IsNullOrEmpty(formCollection["FolderID"]))
                {
                    viewModel.SearchEntity.FolderID = Int32.Parse(formCollection["FolderID"].ToString());
                }
                viewModel.SearchFolderItems();
                ModelState.Clear();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
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
                EconomicUseViewModel viewModel = new EconomicUseViewModel();
                viewModel.PageTitle = "Economic Use Search";
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Search(EconomicUseViewModel viewModel)
        {
            try
            {
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

        public PartialViewResult _List(int entityId = 0)
        {
            EconomicUseViewModel viewModel = new EconomicUseViewModel();
            viewModel.SearchEntity = new EconomicUseSearch { SpeciesID = entityId };
            viewModel.Search();
            return PartialView("~/Views/EconomicUse/_List.cshtml", viewModel);
        }

        // ======================================================================================
        // MODALS 
        // ======================================================================================
        public ActionResult RenderEditModal(int speciesId = 0)
        {
            EconomicUseViewModel viewModel = new EconomicUseViewModel();
            viewModel.Entity.SpeciesID = speciesId;
            return PartialView("~/Views/EconomicUse/Modals/_Lookup.cshtml", viewModel);
        }
        public ActionResult RenderLookupModal(int speciesId = 0)
        {
            EconomicUseViewModel viewModel = new EconomicUseViewModel();
            viewModel.Entity.SpeciesID = speciesId;
            return PartialView("~/Views/EconomicUse/Modals/_Lookup.cshtml", viewModel);
        }

        [HttpPost]
        public JsonResult Add(FormCollection coll)
        {
            return null;
        }

        [HttpPost]
        public PartialViewResult Lookup(FormCollection coll)
        {
#pragma warning disable CS0219 // The variable 'taxonName' is assigned but its value is never used
            string taxonName = "";
#pragma warning restore CS0219 // The variable 'taxonName' is assigned but its value is never used
            CitationViewModel vm = new CitationViewModel();

            //formData.append("AbbreviatedLiteratureSource", abbreviatedLiteratureSource);
            //formData.append("CitationTitle", citationTitle);
            //formData.append("EditorAuthorName", editorAuthorName);
            //formData.append("CitationYear", citationYear);

            if (!String.IsNullOrEmpty(coll["AbbreviatedLiteratureSource"]))
            {
                vm.SearchEntity.Abbreviation = coll["AbbreviatedLiteratureSource"];
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

            if (!String.IsNullOrEmpty(coll["CitationType"]))
            {
                vm.SearchEntity.CitationType = coll["CitationType"];
            }

            if (!String.IsNullOrEmpty(coll["TaxonIDList"]))
            {
                //TODO: GET TAXON ID VALUES
            }

            if (!String.IsNullOrEmpty(coll["TableName"]))
            {
                vm.SearchEntity.TableName = coll["TableName"];
                switch (vm.SearchEntity.TableName)
                {
                    case "taxonomy_family":
                        vm.SearchEntity.FamilyName = coll["TaxonName"];
                        break;
                    case "taxonomy_genus":
                        vm.SearchEntity.GenusName = coll["TaxonName"];
                        break;
                    case "taxonomy_species":
                        vm.SearchEntity.SpeciesName = coll["TaxonName"];
                        break;
                }
            }
            vm.Search();
            return PartialView("~/Views/Citation/_SelectList.cshtml", vm);
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
                EconomicUseViewModel viewModel = new EconomicUseViewModel();
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
