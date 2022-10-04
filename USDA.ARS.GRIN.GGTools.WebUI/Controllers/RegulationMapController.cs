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
    public class RegulationMapController : BaseController, IController<RegulationMapViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/RegulationMap/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        [HttpPost]
        public ActionResult Index(RegulationMapViewModel vm)
        {
            vm.Search();
            ModelState.Clear();
            return View(vm);
        }

        public ActionResult Index()
        {
            try
            {
                RegulationMapViewModel viewModel = new RegulationMapViewModel();
                viewModel.PageTitle = "Regulation Map Search";
                viewModel.TableName = "taxonomy_regulation_map";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _List(string eventValue = "", int speciesId = 0, int regulationId = 0)
        {
            RegulationMapViewModel viewModel = new RegulationMapViewModel();
            viewModel.SearchEntity = new RegulationMapSearch { SpeciesID = speciesId, RegulationID = regulationId };
            viewModel.Search();
            return PartialView(BASE_PATH + "_List.cshtml", viewModel);
        }

        public PartialViewResult _FolderItemList(int folderId = 0)
        {
            RegulationMapViewModel viewModel = new RegulationMapViewModel();
           
            
            //viewModel.SearchEntity = new RegulationMapSearch { SpeciesID = speciesId, RegulationID = regulationId };
            viewModel.Search();
            return PartialView(BASE_PATH + "_List.cshtml", viewModel);
        }

        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            RegulationMapViewModel viewModel = new RegulationMapViewModel();
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

                if (!String.IsNullOrEmpty(formCollection["EntityID"]))
                {
                    viewModel.Entity.SpeciesID = Int32.Parse(formCollection["EntityID"]);
                }
                if (!String.IsNullOrEmpty(formCollection["IDList"]))
                {
                    viewModel.Entity.ItemIDList = formCollection["IDList"];
                }
                viewModel.Insert();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Add(int regulationId = 0)
        {
            try
            {
                RegulationMapViewModel viewModel = new RegulationMapViewModel();
                viewModel.Entity.RegulationID = regulationId;
                viewModel.TableName = "taxonomy_regulation_map";
                viewModel.PageTitle = "Add Regulation Map";
                return View( BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        
        public ActionResult Edit(int entityId = 0)
        {
            try
            {
                RegulationMapViewModel viewModel = new RegulationMapViewModel();
                viewModel.TableName = "taxonomy_regulation_map";
                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit Regulation Map [{0}]: {1}", entityId, viewModel.Entity.AssembledName);
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        
        [HttpPost]
        public ActionResult Edit(RegulationMapViewModel viewModel)
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
                return RedirectToAction("Edit", "RegulationMap", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult _Search(int id = 0)
        {
            RegulationMapViewModel viewModel = new RegulationMapViewModel();
            return PartialView(BASE_PATH + "_Search.cshtml", viewModel);
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Search(RegulationMapViewModel viewModel)
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

        public PartialViewResult FolderItems(int folderId)
        {
            RegulationMapViewModel viewModel = new RegulationMapViewModel();

            try
            {
                viewModel.SearchEntity.FolderID = folderId;
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

        public PartialViewResult SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        // ======================================================================================
        // MODALS 
        // ======================================================================================
        public ActionResult RenderLookupModal()
        {
            RegulationMapViewModel viewModel = new RegulationMapViewModel();
            return PartialView("~/Views/RegulationMap/Modals/_Lookup.cshtml", viewModel);
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
        [HttpPost]
        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
    }
}
