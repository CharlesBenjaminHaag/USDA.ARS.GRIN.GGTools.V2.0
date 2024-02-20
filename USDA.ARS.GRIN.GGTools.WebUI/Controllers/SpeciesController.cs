using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;
using System.Security.Permissions;
using DataTables;
using System.Linq.Expressions;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class SpeciesController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Species/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Map()
        {
            SpeciesBatchEditViewModel viewModel = new SpeciesBatchEditViewModel();
            return View("~/Views/Taxonomy/Species/Map/Index.cshtml", viewModel);
        }

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


        public PartialViewResult _ListFolderItems(int appUserItemFolderId, string displayFormat = "")
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
     
            try
            {
                viewModel.EventAction = "FOLDER";
                viewModel.SearchEntity.FolderID = appUserItemFolderId;
                viewModel.GetFolderItems();

                return PartialView("~/Views/Taxonomy/Species/_ListFolder.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _ListDynamicFolderItems(int folderId)
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();

            try
            {
                viewModel.RunSearch(folderId);
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public ActionResult Index(string eventAction = "", int folderId = 0)
        {
            SpeciesViewModel viewModel = new SpeciesViewModel();
            viewModel.PageTitle = "Species Search";
            viewModel.TableName = "taxonomy_species";
            viewModel.TableCode = "Species";
            viewModel.SearchEntity.SQLStatement = "SELECT * FROM vw_gringlobal_" + viewModel.TableName;

            string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
            if (Session[targetKey] != null)
            {
                viewModel = Session[targetKey] as SpeciesViewModel;
            }

            if (eventAction == "RUN_SEARCH")
            {
                AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                appUserItemListViewModel.Search();
                viewModel.SearchEntity = viewModel.Deserialize<SpeciesSearch>(appUserItemListViewModel.Entity.Properties);
                viewModel.Search();
            }

            return View(BASE_PATH + "Index.cshtml", viewModel);
        }

        public ActionResult Search(SpeciesViewModel viewModel)
        {
            try
            {
                Session[SessionKeyName] = viewModel;
                viewModel.EventAction = "Search";
                viewModel.EventValue = "Species";
                viewModel.Search();
                ModelState.Clear();

                // Save search if attribs supplied.
                if ((viewModel.EventAction == "SEARCH") && (viewModel.EventValue == "SAVE"))
                {
                    viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.SaveSearch();
                }

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

        /// <summary>
        /// Called when adding a synonym. Assumes a UI element that allows the user to 
        /// specify:
        /// 1) Synonym type
        /// 2) Rank (species or infraspecific)
        /// 3) A set of Boolean variables indicating which fields to copy from
        /// the "parent" species of which the synonym is being created.
        /// </summary>
        /// <param name="viewModel">An instance of the Species view model.</param>
        /// <returns>Sends the user to the Species Edit page, configured according to
        /// the synonym type and rank specified.</returns>
        [HttpPost]
        
        public ActionResult Add(SpeciesViewModel viewModel)
        {
            viewModel.TableName = "taxonomy_species";
            viewModel.EventAction = "ADD";
            
            viewModel.Entity.IsAcceptedName = "Y";
            viewModel.Entity.IsAccepted = true;
            viewModel.Entity.IsWebVisibleOption = true;

            // Obtain reference to parent species.
            if (viewModel.Entity.ParentID > 0)
            {
                SpeciesViewModel parentViewModel = new SpeciesViewModel();
                parentViewModel.SearchEntity.ID = viewModel.Entity.ParentID;
                parentViewModel.Search();

                // Add link to "parent" taxon.
                viewModel.ParentEntity = parentViewModel.Entity;
                viewModel.Entity.ParentID = parentViewModel.Entity.ID;
                viewModel.Entity.ParentName = parentViewModel.Entity.AssembledName;
                viewModel.Entity.Name = parentViewModel.Entity.Name;

                if (viewModel.IsCopyGenusRequired == true)
                {
                    viewModel.Entity.GenusID = parentViewModel.Entity.GenusID;
                    viewModel.Entity.GenusName = parentViewModel.Entity.GenusName;
                }

                if (viewModel.IsCopySpeciesRequired == true)
                {
                    viewModel.Entity.SpeciesName = parentViewModel.Entity.SpeciesName;
                }
                
                viewModel.Entity.AssembledName = parentViewModel.Entity.AssembledName;
                viewModel.Entity.SubspeciesName = parentViewModel.Entity.SubspeciesName;
                viewModel.Entity.VarietyName = parentViewModel.Entity.VarietyName;
                viewModel.Entity.SubvarietyName = parentViewModel.Entity.SubvarietyName;
                //viewModel.Entity.Protologue = parentViewModel.Entity.Protologue;

                if (!String.IsNullOrEmpty(viewModel.Entity.SynonymCode))
                {
                    // TODO: Refactor; obtain actual syn code based on human-readable
                    //       string passed in querystring.
                    switch (viewModel.Entity.SynonymCode)
                    {
                        case "=":
                            viewModel.Entity.SynonymDescription = "Homotypic Synonym";
                            break;
                        case "A":
                            viewModel.Entity.SynonymDescription = "Autonym";
                            break;
                        case "B":
                            viewModel.Entity.SynonymDescription = "Basionym";
                            break;
                        case "S":
                            viewModel.Entity.SynonymDescription = "Heterotypic Synonym";
                            break;
                        case "I":
                            viewModel.Entity.SynonymDescription = "Invalid Synonym";
                            break;
                    }
                }
            }

            return View(BASE_PATH + "Edit.cshtml", viewModel);
        }

        public ActionResult Add(int genusId = 0, int entityId = 0, string rank = "species", string synonymTypeCode = "")
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.TableName = "taxonomy_species";
                viewModel.EventAction = "ADD";
                viewModel.EventValue = rank;
                viewModel.Entity.IsAcceptedName = "Y";
                viewModel.Entity.IsAccepted = true;
                viewModel.Entity.IsWebVisibleOption = true;
                //viewModel.IsAutonymNeededOption = true;
                //viewModel.IsBasionymNeededOption = true;
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

                // If an entity ID is passed in, this represents a parent species.
                if (entityId > 0)
                {
                    SpeciesViewModel parentViewModel = new SpeciesViewModel();
                    parentViewModel.SearchEntity.ID = entityId;
                    parentViewModel.Search();

                    // Add link to "parent" taxon.
                    viewModel.ParentEntity = parentViewModel.Entity;
                    viewModel.Entity.ParentID = parentViewModel.Entity.ID;
                    viewModel.Entity.ParentName = parentViewModel.Entity.AssembledName;
                    viewModel.Entity.Name = parentViewModel.Entity.Name;
                    viewModel.Entity.SpeciesName = parentViewModel.Entity.SpeciesName;
                    viewModel.Entity.AssembledName = parentViewModel.Entity.AssembledName;
                    viewModel.Entity.SubspeciesName = parentViewModel.Entity.SubspeciesName;
                    viewModel.Entity.VarietyName = parentViewModel.Entity.VarietyName;
                    viewModel.Entity.SubvarietyName = parentViewModel.Entity.SubvarietyName;
                    viewModel.Entity.GenusID = parentViewModel.Entity.GenusID;
                    viewModel.Entity.GenusName = parentViewModel.Entity.GenusName;
                    //viewModel.Entity.Protologue = parentViewModel.Entity.Protologue;

                    // Store parent entity in session.
                    Session["PARENT-SPECIES"] = viewModel.ParentEntity;
                }

                if (!String.IsNullOrEmpty(synonymTypeCode))
                {
                    // TODO: Refactor; obtain actual syn code based on human-readable
                    //       string passed in querystring.
                    switch (synonymTypeCode)
                    {
                        case "homotypic":
                            viewModel.Entity.SynonymCode = "=";
                            viewModel.Entity.SynonymDescription = "Homotypic Synonym";
                            break;
                        case "autonym":
                            viewModel.Entity.SynonymCode = "A";
                            viewModel.Entity.SynonymDescription = "Autonym";
                            break;
                        case "basionym":
                            viewModel.Entity.SynonymCode = "B";
                            viewModel.Entity.SynonymDescription = "Basionym";
                            break;
                        case "heterotypic":
                            viewModel.Entity.SynonymCode = "S";
                            viewModel.Entity.SynonymDescription = "Heterotypic Synonym";
                            break;
                        case "invalid":
                            viewModel.Entity.SynonymDescription = "Invalid Synonym";
                            viewModel.Entity.SynonymCode = "I";
                            break;
                    }
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
                    viewModel.Entity.SynonymCode = Request.QueryString["synonymCode"];
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
                viewModel.Entity.IsWebVisibleOption = viewModel.ToBool(viewModel.Entity.IsWebVisible);
                viewModel.Entity.IsSpecificHybrid = "N";
                viewModel.Entity.IsSpecificHybridOption = viewModel.ToBool(viewModel.Entity.IsSpecificHybrid);
                
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

        /// <summary>
        /// Retrieves a single record and returns a batch-edit-formatted partial.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult _Get(int speciesId)
        {
            try
            {
                SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.TableName = "taxonomy_species";
                viewModel.TableCode = "Species";
                viewModel.Get(speciesId);
                viewModel.PageTitle = String.Format("Edit [{0}]: {1}", viewModel.Entity.ID, viewModel.Entity.AssembledName);
                viewModel.EventValue = viewModel.Entity.Rank.ToLower();
                viewModel.ID = speciesId;
                viewModel.SpeciesID = speciesId;
                return PartialView("~/Views/Taxonomy/Species/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        /// <summary>
        /// Submits changes to a species record.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="parentId"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult _Post(SpeciesViewModel viewModel)
        {
            SpeciesBatchEditViewModel batchEditViewModel = new SpeciesBatchEditViewModel();

            try
            {
                batchEditViewModel.Get(viewModel.Entity.ID);
                batchEditViewModel.Entity.GenusID = viewModel.Entity.GenusID;
                batchEditViewModel.Entity.SpeciesName = viewModel.Entity.SpeciesName;
                batchEditViewModel.Entity.SpeciesAuthority = viewModel.Entity.SpeciesAuthority;
                batchEditViewModel.Entity.Protologue = viewModel.Entity.Protologue;
                batchEditViewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                batchEditViewModel.Update();
                batchEditViewModel.Get(viewModel.Entity.ID);
                //TODO
                return PartialView("~/Views/Taxonomy/Species/_Edit.cshtml", batchEditViewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult Edit(int entityId, int parentId = 0, string rank = "")
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

                // If there is a parent ID specified, retrieve its pertinent data.
                if(parentId > 0)
                {
                    SpeciesViewModel parentViewModel = new SpeciesViewModel();
                    parentViewModel.Get(parentId);
                    viewModel.Entity.ParentID = parentViewModel.Entity.ID;
                    viewModel.Entity.ParentName = parentViewModel.Entity.Name;
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

                // If the action indicates "ADD" and a synonym code has been supplied, add
                // a map record to link the parent species, and the newly-created one.
                if (viewModel.EventAction.ToUpper() == "ADD")
                {
                    if (!String.IsNullOrEmpty(viewModel.Entity.SynonymCode))
                    {
                        SynonymMapViewModel synonymMapViewModel = new SynonymMapViewModel();
                        synonymMapViewModel.Entity.SpeciesAID = viewModel.Entity.ID;
                        synonymMapViewModel.Entity.SynonymCode = viewModel.Entity.SynonymCode;
                        synonymMapViewModel.Entity.SpeciesBID = viewModel.Entity.ParentID;
                        synonymMapViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                        synonymMapViewModel.Insert();
                    }
                }

                // If parent ID is present, display its data in a navigable URL.
                if (viewModel.Entity.ParentID > 0)
                { 
                
                }

                // If the species is being saved with a non-accepted name, add a
                // synonym map record. The stored proc. will ensure that the synonym
                // does not already exist.
                if ((viewModel.Entity.ID != viewModel.Entity.AcceptedID) && (viewModel.Entity.AcceptedID > 0))
                {
                    SynonymMapViewModel synonymMapViewModel = new SynonymMapViewModel();
                    synonymMapViewModel.Entity.SpeciesAID = viewModel.Entity.ID;
                    synonymMapViewModel.Entity.SynonymCode = viewModel.Entity.SynonymCode;
                    synonymMapViewModel.Entity.SpeciesBID = viewModel.Entity.AcceptedID;
                    synonymMapViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    synonymMapViewModel.Insert();
                }

                return RedirectToAction("Edit", "Species", new { entityId = viewModel.Entity.ID, parentId = viewModel.Entity.ParentID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }


        public JsonResult EditMultiple()
        {
            var request = System.Web.HttpContext.Current.Request;
            //var settings = Properties.Settings.Default;

            try {

                using (var db = new Database("sqlserver", "Data Source=localhost;Initial Catalog=gringlobal;User Id=gg_user;Password=Savory*survive8ammonia?;Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=100;Pooling=true;"))
                {
                    var response = new Editor(db, "taxonomy_species", "taxonomy_species_id")
                        .Model<Species_POC>()
                        .Field(new Field("taxonomy_species_id")
                            .Validator(Validation.NotEmpty())
                        )
                        .Field(new Field("name"))
                        .Field(new Field("protologue")
                            
                        )
                        .Field(new Field("name_authority")
                            
                        )
                        .Process(request)
                        .Data();

                    JsonResult jsonResult = new JsonResult();
                    jsonResult = Json(response);
                    jsonResult.MaxJsonLength = 2147483644;
                    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    return jsonResult;
                }
                
            }
            catch(Exception ex) 
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditBatch(string idList = "")
        {
            try
            {   SpeciesViewModel viewModel = new SpeciesViewModel();
                viewModel.PageTitle = "Species Batch Edit";
                viewModel.SearchEntity.IDList = idList;
                viewModel.Search();

                //DEBUG
                return View("~/Views/Taxonomy/Species/EditMultiple_POC.cshtml", viewModel);
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
        //public PartialViewResult RenderReportsWidget()
        //{
        //    try
        //    {
        //        SpeciesViewModel viewModel = new SpeciesViewModel();
        //        viewModel.GetReportList();
        //        return PartialView("~/Views/Taxonomy/Reports/_ListWidget.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}
        //public ViewResult ReportDetail(string reportCode)
        //{
        //    SpeciesViewModel viewModel = new SpeciesViewModel();
        //    viewModel.PageTitle = reportCode.Replace("_", " ");
        //    viewModel.GetReport(reportCode);
        //    //TODO Get report
        //    return View("~/Views/Taxonomy/Reports/Detail.cshtml", viewModel);
        //}

        #endregion
    }
}