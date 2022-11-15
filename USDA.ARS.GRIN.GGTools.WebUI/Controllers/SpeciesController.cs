using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class SpeciesController : BaseController, IController<SpeciesViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Species/";
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
            SpeciesViewModel viewModel = new SpeciesViewModel();
            viewModel.PageTitle = "Species Search";
            viewModel.TableName = "taxonomy_species";
            viewModel.TableCode = "Species";
            return View(BASE_PATH + "Index.cshtml", viewModel);
        }

        public ActionResult Search(SpeciesViewModel viewModel)
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

        public PartialViewResult _List(int entityId = 0, int genusId = 0, string formatCode = "")
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            try
            {
                viewModel.SearchEntity = new SpeciesSearch { GenusID = genusId };
                viewModel.Search();
                viewModel.Entity.GenusID = genusId;

                if (formatCode == "S")
                {
                    return PartialView("~/Views/Taxonomy/Species/Modals/_SelectListSimple.cshtml", viewModel);
                }
                else
                {
                    return PartialView(BASE_PATH + "_List.cshtml", viewModel);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public PartialViewResult Lookup(FormCollection formCollection)
        {
            string partialViewName = "~/Views/Taxonomy/Species/Modals/_SelectList.cshtml";
            SpeciesViewModel viewModel = new SpeciesViewModel();

            try
            {
                if (!String.IsNullOrEmpty(formCollection["EventAction"]))
                {
                    viewModel.EventAction = formCollection["EventAction"];
                    switch (viewModel.EventAction)
                    {
                        case "species-a":
                            partialViewName = "~/Views/Taxonomy/SynonymMap/_SelectListSpeciesA.cshtml";
                            break;
                        case "species-b":
                            partialViewName = "~/Views/Taxonomy/SynonymMap/_SelectListSpeciesB.cshtml";
                            break;
                    }
                }

                if (!String.IsNullOrEmpty(formCollection["SpeciesName"]))
                {
                    viewModel.SearchEntity.SpeciesName = formCollection["SpeciesName"];
                }

                if (!String.IsNullOrEmpty(formCollection["SynonymCode"]))
                {
                    viewModel.SearchEntity.SynonymCode = formCollection["SynonymCode"];
                }

                if (!String.IsNullOrEmpty(formCollection["IsAcceptedName"]))
                {
                    viewModel.SearchEntity.IsAcceptedName = formCollection["IsAcceptedName"];
                }

                if (!String.IsNullOrEmpty(formCollection["IsMultiSelect"]))
                {
                    viewModel.IsMultiSelect = formCollection["IsMultiSelect"];
                }

                viewModel.Search();
                return PartialView(partialViewName, viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public PartialViewResult LookupProtologues(FormCollection formCollection)
        {
            string partialViewName = BASE_PATH + "/Modals/_SelectListProtologue.cshtml";
            SpeciesViewModel viewModel = new SpeciesViewModel();

            if (!String.IsNullOrEmpty(formCollection["Protologue"]))
            {
                viewModel.SearchEntity.Protologue = formCollection["Protologue"];
            }

            viewModel.SearchProtologues(viewModel.SearchEntity.Protologue);
            return PartialView(partialViewName, viewModel);
        }
        public ActionResult Add(int genusId =0, int speciesId = 0, string rank = "")
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.TableName = "taxonomy_species";
                viewModel.PageTitle = String.Format("Add {0}", viewModel.ToTitleCase(rank));
                viewModel.Entity.IsAcceptedName = "Y";
                viewModel.Entity.Rank = rank;

                if (genusId > 0)
                {
                    GenusViewModel topRankGenusViewModel = new GenusViewModel();
                    topRankGenusViewModel.SearchEntity.ID = genusId;
                    topRankGenusViewModel.Search();
                    //viewModel.TopRankGenusEntity = topRankGenusViewModel.Entity;
                    viewModel.Entity.GenusID = topRankGenusViewModel.Entity.ID;
                    viewModel.Entity.GenusName = topRankGenusViewModel.Entity.Name;
                }

                //TODO
                //Retrieve instance of parent species
                //Copy attribs of parent to new empty VM
                //Set parent species entity of VM
                if (speciesId > 0)
                {
                    SpeciesViewModel parentViewModel = new SpeciesViewModel();
                    parentViewModel.SearchEntity.ID = speciesId;
                    parentViewModel.Search();
                    viewModel.ParentEntity = parentViewModel.Entity;
                    viewModel.Entity.GenusID = parentViewModel.Entity.GenusID;
                    viewModel.Entity.GenusName = parentViewModel.Entity.GenusName;
                    viewModel.Entity.Protologue= parentViewModel.Entity.Protologue;
                }

                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _Add(int genusId)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.TableName = "taxonomy_species";
                viewModel.Entity.IsAcceptedName = "Y";
                viewModel.Entity.Rank = "SPECIES";

                if (genusId > 0)
                {
                    GenusViewModel topRankGenusViewModel = new GenusViewModel();
                    topRankGenusViewModel.SearchEntity.ID = genusId;
                    topRankGenusViewModel.Search();
                    viewModel.Entity.GenusID = topRankGenusViewModel.Entity.ID;
                    viewModel.Entity.GenusName = topRankGenusViewModel.Entity.Name;
                }
                return PartialView("~/Views/Taxonomy/Species/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _Edit(int entityId)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.TableName = "taxonomy_species";
                viewModel.TableCode = "Species";
                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit [{0}]: {1}", viewModel.Entity.ID, viewModel.Entity.FullName);
                viewModel.EventValue = viewModel.Entity.Rank.ToLower();
                viewModel.ID = entityId;
                return PartialView("~/Views/Taxonomy/Species/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _Save(SpeciesViewModel viewModel)
        {
            try
            {
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
                viewModel.Get(viewModel.Entity.ID);
                return PartialView("~/Views/Taxonomy/Species/_Edit.cshtml", viewModel);
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
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.TableName = "taxonomy_species";
                viewModel.TableCode = "Species";
                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit [{0}]: {1}", viewModel.Entity.ID, viewModel.Entity.FullName);
                viewModel.EventValue = viewModel.Entity.Rank.ToLower();
                viewModel.ID = entityId;
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(SpeciesViewModel viewModel)
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
                    if ((viewModel.EventValue == "VERIFY") || (viewModel.EventValue == "REVERIFY"))
                    {
                        viewModel.Entity.VerifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                        viewModel.Entity.NameVerifiedDate = DateTime.Now;
                    }
                    else
                    {
                        if (viewModel.EventValue == "UNVERIFY")
                        {
                            viewModel.Entity.VerifiedByCooperatorID = 0;
                            viewModel.Entity.NameVerifiedDate = DateTime.MinValue;
                        }
                    }

                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }
                return RedirectToAction("Edit", "Species", new { entityId = viewModel.Entity.ID });
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

        [HttpPost]
        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();

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

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
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

        public PartialViewResult SetVerificationStatus(FormCollection formCollection)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

                if (!String.IsNullOrEmpty(formCollection["EntityID"]))
                {
                    viewModel.Entity.ID = Int32.Parse(formCollection["EntityID"]);
                }

                if (!String.IsNullOrEmpty(formCollection["StatusCode"]))
                {
                    viewModel.SetVerificationStatus(formCollection["StatusCode"]);
                }
                return PartialView("~/Views/Taxonomy/Species/_Verification.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
    }
}