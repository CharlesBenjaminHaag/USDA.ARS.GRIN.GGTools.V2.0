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
    public class SpeciesSynonymMapController : BaseController, IController<SynonymMapViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/SpeciesSynonymMap/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public PartialViewResult _List(int speciesId = 0)
        {
            SynonymMapViewModel viewModel = new SynonymMapViewModel();
            viewModel.SearchEntity = new SpeciesSynonymMapSearch { SpeciesBID = speciesId };
            viewModel.Search();
            return PartialView(BASE_PATH + "_List.cshtml", viewModel);
        }
        public PartialViewResult _ListFolderItems(int folderId)
        {
            SynonymMapViewModel viewModel = new SynonymMapViewModel();
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
        public ActionResult Add(int speciesId = 0, string synonymType = "")
        {
            SynonymMapViewModel viewModel = new SynonymMapViewModel();
            SpeciesViewModel speciesViewModel = new SpeciesViewModel();

            try
            {
                viewModel.TableName = "taxonomy_species_synonym_map";

                // Clear the session of any previously-created batches.
                if (Session["SPECIES-SYNONYM-MAPS"] != null)
                {
                    Session.Remove("SPECIES-SYNONYM-MAPS");
                }

                // If an entity (species) ID is sent via param, retrieve the species and set the corresponding 
                // VM attributes.
                if (speciesId > 0)
                {
                    speciesViewModel.SearchEntity.ID = speciesId;
                    speciesViewModel.Search();
                    viewModel.SearchEntity.SpeciesAID = speciesId;
                    viewModel.SearchEntity.SpeciesAName = speciesViewModel.DataCollection[0].Name;
                }
                else
                {
                }
                return View(BASE_PATH + "Add.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public PartialViewResult Add(FormCollection formCollection)
        {
            SynonymMapViewModel viewModel = new SynonymMapViewModel();
            List<SpeciesSynonymMap> batchedSpeciesSynonymMaps = new List<SpeciesSynonymMap>();
            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

            if (!String.IsNullOrEmpty(formCollection["SynonymCode"]))
            {
                viewModel.Entity.SynonymCode = formCollection["SynonymCode"];
            }

            if (!String.IsNullOrEmpty(formCollection["SpeciesIDListSubject"]))
            {
                viewModel.SpeciesIDListSubject = formCollection["SpeciesIDListSubject"];
            }

            if (!String.IsNullOrEmpty(formCollection["SpeciesIDListPredicate"]))
            {
                viewModel.SpeciesIDListPredicate = formCollection["SpeciesIDListPredicate"];
            }

            viewModel.InsertMultiple();

            // Add each generated batch to the session-stored list.
            if (Session["SPECIES-SYNONYM-MAPS"] != null)
            {
                batchedSpeciesSynonymMaps = Session["SPECIES-SYNONYM-MAPS"] as List<SpeciesSynonymMap>;
            }
            batchedSpeciesSynonymMaps.AddRange(viewModel.DataCollection);
            Session["SPECIES-SYNONYM-MAPS"] = batchedSpeciesSynonymMaps;
            viewModel.DataCollectionBatch = batchedSpeciesSynonymMaps;

            return PartialView("~/Views/Taxonomy/SpeciesSynonymMap/_BatchList.cshtml", viewModel);
        }

        public PartialViewResult Clear(FormCollection formCollection)
        {
            if (Session["SPECIES-SYNONYM-MAPS"] != null)
            {
                Session.Remove("SPECIES-SYNONYM-MAPS");
                SynonymMapViewModel viewModel = new SynonymMapViewModel();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            return PartialView();
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public ActionResult Edit(SynonymMapViewModel viewModel)
        {
            throw new NotImplementedException();
        }
        public ActionResult Edit(int entityId)
        {
            try
            {
                SynonymMapViewModel viewModel = new SynonymMapViewModel();
                viewModel.TableName = "taxonomy_species_synonym_map";
                viewModel.TableCode = "SpeciesSynonymMap";
                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit [{0}]", viewModel.Entity.ID);
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Index()
        {
            SynonymMapViewModel viewModel = new SynonymMapViewModel();
            try 
            {
                viewModel.TableName = "taxonomy_species_synonym_map";
                viewModel.PageTitle = "Synonym Map Search";
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Search(SynonymMapViewModel viewModel)
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

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
                SynonymMapViewModel viewModel = new SynonymMapViewModel();
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