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
    public class SpeciesController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Species/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        //public ActionResult Explorer()
        //{
        //    TaxonomyExplorerViewModel viewModel = new TaxonomyExplorerViewModel();
        //    GenusViewModel viewModelGenus = new GenusViewModel();
        //    FamilyMapViewModel viewModelFamily = new FamilyMapViewModel();
        //    SpeciesViewModel viewModelSpecies = new SpeciesViewModel();

        //    try
        //    {
        //        viewModelFamily.SearchEntity.FamilyName = "Poa";
        //        viewModelFamily.Search();
        //        viewModel.DataCollectionFamily = viewModelFamily.DataCollection;

        //        viewModelGenus.SearchEntity.FullName = "Poa";
        //        viewModelGenus.Search();
        //        viewModel.DataCollectionGenus = viewModelGenus.DataCollection;
        //        viewModel.SpeciesViewModel = viewModelSpecies;
        //        return View(BASE_PATH + "Explorer/Index.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return RedirectToAction("InternalServerError", "Error");
        //    }
        //}
        public PartialViewResult _List(int entityId = 0, int genusId = 0, string formatCode = "", string speciesAuthority = "")
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            try
            {
                viewModel.SearchEntity.GenusID = genusId;
                viewModel.SearchEntity.SpeciesAuthority = speciesAuthority;
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

        public PartialViewResult _ListConspecific(int speciesId = 0)
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            try
            {
                viewModel.SearchEntity.ID = speciesId;
                viewModel.GetConspecific();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }


        public PartialViewResult _ListFolderItems(int folderId, string displayFormat = "")
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            string listPartialViewName = "~/Views/Taxonomy/Species/_List.cshtml";

            try
            {
                viewModel.EventAction = "FOLDER";
                viewModel.SearchEntity.FolderID = folderId;
                viewModel.GetFolderItems();

                if (displayFormat == "SELECT")
                {
                    listPartialViewName = "~/Views/Taxonomy/Species/Modals/_SelectList.cshtml";
                }

                return PartialView(listPartialViewName, viewModel);
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
            viewModel.SearchEntity.SQLStatement = "SELECT * FROM vw_gringlobal_" + viewModel.TableName;
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

        public PartialViewResult RenderLookupModal()
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            return PartialView(BASE_PATH + "/Modals/_Lookup.cshtml", viewModel);
        }
        public PartialViewResult RenderParentLookupModal()
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            return PartialView(BASE_PATH + "/Modals/_LookupParent.cshtml", viewModel);
        }
        public PartialViewResult RenderInfraspecificAutonymWidget(string genusName, string speciesName, string rank)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.GetInfraspecificAutonym(genusName, speciesName, rank);
                return PartialView("~/Views/Taxonomy/Shared/_InfraspecificAutonymWidget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        [HttpPost]
        public PartialViewResult Lookup(SpeciesViewModel viewModel)
        {
            string partialViewName = "~/Views/Taxonomy/Species/Modals/_SelectList.cshtml";

            try
            {
                switch (viewModel.EventAction)
                {
                    case "species-a":
                        partialViewName = "~/Views/Taxonomy/SpeciesSynonymMap/_SelectListSpeciesA.cshtml";
                        break;
                    case "species-b":
                        partialViewName = "~/Views/Taxonomy/SpeciesSynonymMap/_SelectListSpeciesB.cshtml";
                        break;
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
        public PartialViewResult LookupParent(SpeciesViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                return PartialView("~/Views/Taxonomy/Species/Modals/_SelectListParent.cshtml", viewModel);
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

        public ActionResult Add(int genusId = 0, int entityId = 0, string rank = "species", string synonymTypeCode = "")
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.TableName = "taxonomy_species";
                viewModel.EventAction = "ADD";
                viewModel.Entity.IsAcceptedName = "Y";
                viewModel.Entity.IsAccepted = true;
                viewModel.Entity.IsWebVisibleOption = true;
                viewModel.IsAutonymNeededOption = true;
                viewModel.IsBasionymNeededOption = true;
                viewModel.Entity.Rank = String.IsNullOrEmpty(rank) ? "species" : rank;
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Entity.CreatedByCooperatorName = AuthenticatedUser.FullName;
                viewModel.Entity.CreatedDate = System.DateTime.Now;

                if (genusId > 0)
                {
                    GenusViewModel topRankGenusViewModel = new GenusViewModel();
                    topRankGenusViewModel.SearchEntity.ID = genusId;
                    topRankGenusViewModel.Search();
                    //viewModel.TopRankGenusEntity = topRankGenusViewModel.Entity;
                    viewModel.Entity.GenusID = topRankGenusViewModel.Entity.ID;
                    viewModel.Entity.GenusName = topRankGenusViewModel.Entity.Name;
                }

                if (entityId > 0)
                {
                    SpeciesViewModel parentViewModel = new SpeciesViewModel();
                    parentViewModel.SearchEntity.ID = entityId;
                    parentViewModel.Search();

                    // Add link to "parent" taxon.
                    viewModel.ParentEntity = parentViewModel.Entity;
                    viewModel.Entity.ParentID = parentViewModel.ID;
                    viewModel.Entity.Name = parentViewModel.Entity.Name;
                    viewModel.Entity.SpeciesName = parentViewModel.Entity.SpeciesName;
                    viewModel.Entity.AssembledName = parentViewModel.Entity.AssembledName;
                    viewModel.Entity.SubspeciesName = parentViewModel.Entity.SubspeciesName;
                    viewModel.Entity.VarietyName = parentViewModel.Entity.VarietyName;
                    viewModel.Entity.SubvarietyName = parentViewModel.Entity.SubvarietyName;
                    viewModel.Entity.GenusID = parentViewModel.Entity.GenusID;
                    viewModel.Entity.GenusName = parentViewModel.Entity.GenusName;
                    viewModel.Entity.Protologue = parentViewModel.Entity.Protologue;
                }

                if (!String.IsNullOrEmpty(synonymTypeCode))
                {
                    viewModel.PageTitle = String.Format("Add {0} of {1}", viewModel.ToTitleCase(synonymTypeCode), viewModel.ParentEntity.AssembledName);
                    viewModel.EventAction = "ADD_SYNONYM";
                    viewModel.EventValue = synonymTypeCode;
                }
                else
                {
                    viewModel.PageTitle = String.Format("Add {0}", viewModel.ToTitleCase(rank));
                }

                viewModel.SubspeciesUrl = Url.Action("Add", "Species", new { entityId = entityId, rank = "subspecies" });
                viewModel.VarietyUrl = Url.Action("Add", "Species", new { entityId = entityId, rank = "variety" });
                viewModel.SubvarietyUrl = Url.Action("Add", "Species", new { entityId = entityId, rank = "subvariety" });
                viewModel.FormUrl = Url.Action("Add", "Species", new { entityId = entityId, rank = "form" });

                if (!String.IsNullOrEmpty(Request.QueryString["synonymCode"]))
                {
                    viewModel.SubspeciesUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                    viewModel.VarietyUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                    viewModel.SubvarietyUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                    viewModel.FormUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                }

                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        public ActionResult AddAutonym(int entityId)
        {
            //TODO
            //Get species whose ID is passed in
            //Create copy and configure as autonym
            //Add new autonym
            //Re-load species passed in; new autonym will appear in conspecific list

            SpeciesViewModel speciesViewModel = new SpeciesViewModel();
            speciesViewModel.Get(entityId);

            SpeciesViewModel autonymViewModel = new SpeciesViewModel();
            autonymViewModel.Entity.GenusID = speciesViewModel.Entity.GenusID;
            autonymViewModel.Entity.SpeciesName = speciesViewModel.Entity.SpeciesName;
            autonymViewModel.Entity.Rank = speciesViewModel.Entity.Rank;
            autonymViewModel.Entity.SynonymCode = "A";

            switch (speciesViewModel.Entity.Rank.ToLower())
            {
                case "subspecies":
                    autonymViewModel.Entity.SubspeciesName = speciesViewModel.Entity.SpeciesName;
                    break;
                case "variety":
                    autonymViewModel.Entity.VarietyName = speciesViewModel.Entity.SpeciesName;
                    break;
                case "subvariety":
                    autonymViewModel.Entity.SubvarietyName = speciesViewModel.Entity.SpeciesName;
                    break;
                case "form":
                    autonymViewModel.Entity.FormaName = speciesViewModel.Entity.SpeciesName;
                    break;
            }
            autonymViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            autonymViewModel.Insert();
            return RedirectToAction("Edit", "Species", new { @entityId = entityId });
        }

        public PartialViewResult _Add(int genusId)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.TableName = "taxonomy_species";
                viewModel.Entity.IsAcceptedName = "Y";
                viewModel.Entity.Rank = "SPECIES";
                viewModel.Entity.IsWebVisible = "Y";

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
                viewModel.PageTitle = String.Format("Edit [{0}]: {1}", viewModel.Entity.ID, viewModel.Entity.AssembledName);
                viewModel.EventValue = viewModel.Entity.Rank.ToLower();
                viewModel.ID = entityId;
                viewModel.SpeciesID = entityId;
                return PartialView("~/Views/Taxonomy/Species/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public JsonResult _Save(SpeciesViewModel viewModel)
        {
            int speciesId = 0;
            try
            {
                if (viewModel.Entity.ID == 0)
                {
                    viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Insert();
                    speciesId = viewModel.Entity.ID;
                }
                else
                {
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                    speciesId = viewModel.Entity.ID;
                }
                viewModel.Get(speciesId);
                viewModel.Entity.SpeciesID = speciesId;
                return Json(new { species = viewModel.Entity }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { speciesId = -1 }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Edit(int entityId, string rank = "")
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.TableName = "taxonomy_species";
                viewModel.TableCode = "Species";
                viewModel.EventAction = "EDIT";
                viewModel.Get(entityId);
                viewModel.Entity.Protologue = System.Web.HttpUtility.HtmlDecode(viewModel.Entity.Protologue);
                viewModel.PageTitle = String.Format("Edit {0} [{1}]: {2}", viewModel.Entity.ID, viewModel.ToTitleCase(viewModel.Entity.Rank), viewModel.Entity.AssembledName);
                viewModel.EventValue = viewModel.Entity.Rank.ToUpper();
                viewModel.ID = entityId;

                // If there is a rank specified, this is a change-rank operation; reload
                // page with newly-set rank to enable necessary fields.
                if (!String.IsNullOrEmpty(rank))
                {
                    viewModel.Entity.Rank = rank;
                }

                viewModel.SubspeciesUrl = Url.Action("Edit", "Species", new { entityId = entityId, rank = "subspecies" });
                viewModel.VarietyUrl = Url.Action("Edit", "Species", new { entityId = entityId, rank = "variety" });
                viewModel.SubvarietyUrl = Url.Action("Edit", "Species", new { entityId = entityId, rank = "subvariety" });
                viewModel.FormUrl = Url.Action("Edit", "Species", new { entityId = entityId, rank = "form" });

                if (!String.IsNullOrEmpty(Request.QueryString["synonymCode"]))
                {
                    viewModel.SubspeciesUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                    viewModel.VarietyUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                    viewModel.SubvarietyUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
                    viewModel.FormUrl += "&synonymCode=" + Request.QueryString["synonymCode"];
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
                    if (viewModel.EventAction == "VERIFY")
                    {
                        if (viewModel.EventValue == "Y")
                        {
                            viewModel.Entity.VerifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                            viewModel.Entity.NameVerifiedDate = DateTime.Now;
                        }
                        else
                        {
                            viewModel.Entity.VerifiedByCooperatorID = 0;
                            viewModel.Entity.NameVerifiedDate = DateTime.MinValue;
                        }
                    }

                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }

                // If action code indicates that the species being added is a synonym, add
                // the necessary map record.
                if (viewModel.EventAction == "ADD_SYNONYM")
                {
                    SynonymMapViewModel synonymMapViewModel = new SynonymMapViewModel();
                    // TODO
                }

                // If this save entails adding a new species, determine whether or not specified
                // related taxon records must be added as well.
                if (viewModel.EventAction == "ADD")
                {
                    //if (viewModel.IsAutonymNeededOption)
                    //{
                    //    SpeciesViewModel speciesViewModelAutonym = new SpeciesViewModel();
                    //    speciesViewModelAutonym.Entity = viewModel.Entity;
                    //    speciesViewModelAutonym.Entity.ID = 0;
                    //    speciesViewModelAutonym.Entity.NameAuthority = null;
                    //    speciesViewModelAutonym.Insert();
                    //}

                    //if (viewModel.IsBasionymNeededOption)
                    //{
                    //    SpeciesViewModel speciesViewModelBasionym = new SpeciesViewModel();
                    //    speciesViewModelBasionym.Entity = viewModel.Entity;
                    //    speciesViewModelBasionym.Entity.ID = 0;
                    //    speciesViewModelBasionym.Entity.NameAuthority = null;
                    //    speciesViewModelBasionym.Insert();
                    //}
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

        public PartialViewResult SetVerificationStatus(int speciesId, string statusCode)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.Get(speciesId);

                if (statusCode == "Y")
                {
                    viewModel.Entity.VerifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Entity.NameVerifiedDate = System.DateTime.Now;
                }
                else
                {
                    viewModel.Entity.VerifiedByCooperatorID = 0;
                    viewModel.Entity.NameVerifiedDate = DateTime.MinValue;
                }

                viewModel.Update();
                viewModel.Get(speciesId);
                return PartialView("~/Views/Taxonomy/Species/_RevisionHistory.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        #region Reports
        public PartialViewResult RenderReportsWidget()
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.GetReportList();
                return PartialView("~/Views/Taxonomy/Reports/_ListWidget.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public ViewResult ReportDetail(string reportCode)
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            viewModel.PageTitle = reportCode.Replace("_", " ");
            viewModel.GetReport(reportCode);
            //TODO Get report
            return View("~/Views/Taxonomy/Reports/Detail.cshtml", viewModel);
        }

        #endregion
    }
}