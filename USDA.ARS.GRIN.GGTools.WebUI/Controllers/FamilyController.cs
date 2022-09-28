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
    public class FamilyController : BaseController, IController<FamilyMapViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Family/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Add(string rank, int orderId = 0, int familyMapId = 0, int familyId = 0) 
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();
            try 
            {
                viewModel.Entity.FamilyRank = viewModel.ToTitleCase(rank);
                viewModel.PageTitle = "Add " + viewModel.Entity.FamilyRank;
                viewModel.TableName = "taxonomy_family_map";
                viewModel.Entity.OrderID = orderId;
                viewModel.Entity.ParentID = familyMapId;
                viewModel.Entity.FamilyID = familyId;
                viewModel.Entity.IsAccepted = true;
                viewModel.Entity.IsAcceptedName = "Y";
                viewModel.EditPartialViewName = BASE_PATH + "~/Views/Family/_" + viewModel.Entity.FamilyRank + "RankEdit.cshtml";

                // If the record being added is infrafamilial:
                // 1) Set rank based on eventValue parameter
                // 2) Obtain the family ID from the record matching the familyMapId parameter.
                //if (!String.IsNullOrEmpty(eventValue) && (eventValue != "family"))
                //{
                //    FamilyMapViewModel familyMapViewModel = new FamilyMapViewModel();
                //    familyMapViewModel.SearchEntity.ID = familyMapId;
                //    familyMapViewModel.Search();
                //    viewModel.Entity.FamilyID = familyMapViewModel.Entity.FamilyID;
                //    viewModel.Entity.FamilyName = familyMapViewModel.Entity.FamilyName;
                //    viewModel.Entity.SubfamilyID = familyMapViewModel.Entity.SubfamilyID;
                //    viewModel.Entity.TribeID = familyMapViewModel.Entity.TribeID;
                //    viewModel.Entity.SubtribeID = familyMapViewModel.Entity.SubtribeID;
                //}
                
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult AddSubfamily(int familyMapId)
        {
            try 
            {
                FamilyMapViewModel viewModel = new FamilyMapViewModel();
                try
                {
                    viewModel.PageTitle = "Add Subfamily";
                    viewModel.TableName = "taxonomy_family_map";
                    viewModel.Entity.ParentID = familyMapId;
                    viewModel.Entity.FamilyID = familyMapId;
                    viewModel.Entity.IsAccepted = true;
                    viewModel.Entity.IsAcceptedName = "Y";

                    if (familyMapId > 0)
                    {
                        viewModel.Entity.FamilyID = familyMapId;
                        FamilyMapViewModel familyParentViewModel = new FamilyMapViewModel();
                        familyParentViewModel.SearchEntity.ID = familyMapId;
                        familyParentViewModel.Search();

                        viewModel.Entity.FamilyID = familyParentViewModel.Entity.FamilyID;
                        viewModel.Entity.FamilyName = familyParentViewModel.Entity.FamilyName;
                    }

                    return View(BASE_PATH + "EditSubfamily.cshtml", viewModel);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    return RedirectToAction("InternalServerError", "Error");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult AddTribe(int familyMapId, int subFamilyId = 0)
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();
            try
            {
                viewModel.PageTitle = "Add Tribe";
                viewModel.TableName = "taxonomy_family_map";
                viewModel.Entity.ParentID = familyMapId;
                viewModel.Entity.FamilyID = familyMapId;
                viewModel.Entity.IsAccepted = true;
                viewModel.Entity.IsAcceptedName = "Y";
                return View(BASE_PATH + "EditTribe.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult AddSubtribe(int familyMapId, int subFamilyId = 0, int tribeId = 0)
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();
            try
            {
                viewModel.PageTitle = "Add Subtribe";
                viewModel.TableName = "taxonomy_family_map";
                viewModel.Entity.ParentID = familyMapId;
                viewModel.Entity.FamilyID = familyMapId;
                viewModel.Entity.IsAccepted = true;
                viewModel.Entity.IsAcceptedName = "Y";
                return View(BASE_PATH + "EditSubtribe.cshtml", viewModel);
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
                FamilyMapViewModel viewModel = new FamilyMapViewModel();

                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.TableName = "taxonomy_family_map";
                    viewModel.PageTitle = viewModel.GetPageTitle();
                }
                else
                {
                    viewModel.PageTitle = String.Format("Add Species");
                }
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult EditSubfamily(int entityId)
        {
            try
            {
                FamilyMapViewModel viewModel = new FamilyMapViewModel();

                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.TableName = "taxonomy_family_map";
                    viewModel.PageTitle = viewModel.GetPageTitle();
                }
                else
                {
                    viewModel.PageTitle = String.Format("Add Species");
                }
                return View(BASE_PATH + "EditSubfamily.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult EditTribe(int entityId)
        {
            try
            {
                FamilyMapViewModel viewModel = new FamilyMapViewModel();

                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.TableName = "taxonomy_family_map";
                    viewModel.PageTitle = viewModel.GetPageTitle();
                }
                else
                {
                    viewModel.PageTitle = String.Format("Add Species");
                }
                return View(BASE_PATH + "EditTribe.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult EditSubtribe(int entityId)
        {
            try
            {
                FamilyMapViewModel viewModel = new FamilyMapViewModel();

                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.TableName = "taxonomy_family_map";
                    viewModel.PageTitle = viewModel.GetPageTitle();
                }
                else
                {
                    viewModel.PageTitle = String.Format("Add Species");
                }
                return View(BASE_PATH + "EditSubtribe.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(FamilyMapViewModel viewModel)
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
                return RedirectToAction("Edit", "Family", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult EditSubfamily(FamilyMapViewModel viewModel)
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
                    viewModel.InsertSubfamily();
                }
                else
                {
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }
                return RedirectToAction("EditSubFamily", "Family", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult EditTribe(FamilyMapViewModel viewModel)
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
                return RedirectToAction("Edit", "Family", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult EditSubTribe(FamilyMapViewModel viewModel)
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
                return RedirectToAction("Edit", "Family", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult FolderItems(int folderId)
        {
            try
            {
                FamilyMapViewModel viewModel = new FamilyMapViewModel();
                viewModel.EventAction = "SEARCH";
                viewModel.EventValue = "FOLDER";
                viewModel.SearchEntity.FolderID = folderId;
                viewModel.SearchFolderItems();
                ModelState.Clear();
                return PartialView(BASE_PATH + "List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        public ActionResult Index()
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

            try
            {
                viewModel.PageTitle = "Family Search";
                viewModel.TableName = "taxonomy_family_Map";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Search(FamilyMapViewModel viewModel)
        {
            try
            {
                viewModel.SearchEntity.FamilyRank = viewModel.EventValue;
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

        public PartialViewResult _List(string eventValue = "", int orderId=0)
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

            try
            {
                viewModel.SearchEntity.OrderID = orderId;
                viewModel.Search();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListSynonyms(int familyId)
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

            try
            {
                viewModel.GetSynonyms(familyId);
                return PartialView(BASE_PATH + "_ListSynonyms.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListSubdivisions(int familyId)
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

            try
            {
                viewModel.GetSubdivisions(familyId);
                return PartialView(BASE_PATH + "_ListSubdivisions.cshtml", viewModel);
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
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

            if (!String.IsNullOrEmpty(formCollection["LookupFamilyName"]))
            {
                viewModel.SearchEntity.FamilyName = formCollection["LookupFamilyName"];
            }
            viewModel.Search();
            return PartialView(BASE_PATH + "Modals/_SelectList.cshtml", viewModel);
        }

        private string GetPartialViewName(string rank)
        {
            switch (rank.ToUpper())
            {
                case "FAMILY":
                    return "_Detail.cshtml";
                case "SUBFAMILY":
                    return "_SubfamilyRankDetail.cshtml";
                case "TRIBE":
                    return "_TribeRankDetail.cshtml";
                case "SUBTRIBE":
                    return "_SubtribeRankDetail.cshtml";
                default:
                    return "";
            }
        }

        [HttpPost]
        //public PartialViewResult AddInfrafamilialTaxa(FormCollection formCollection)
        //{
        //    InfrafamilyViewModel viewModel = new InfrafamilyViewModel();

        //    if (!String.IsNullOrEmpty(formCollection["FamilyID"]))
        //    {
        //        viewModel.FamilyID = Int32.Parse(formCollection["FamilyID"]);
        //    }

        //    if (!String.IsNullOrEmpty(formCollection["NumSubfamilies"]))
        //    {
        //        viewModel.SubfamilyCount = Int32.Parse(formCollection["NumSubfamilies"]);
        //    }

        //    if (!String.IsNullOrEmpty(formCollection["NumTribes"]))
        //    {
        //        viewModel.TribeCount = Int32.Parse(formCollection["NumTribes"]);
        //    }

        //    if (!String.IsNullOrEmpty(formCollection["NumSubtribes"]))
        //    {
        //        viewModel.SubtribeCount = Int32.Parse(formCollection["NumSubtribes"]);
        //    }
        //    viewModel.Get();
        //    return PartialView("~/Views/FamilyMap/Modals/_InfrafamilialTaxonEditGrid.cshtml", viewModel);
        //}

        public JsonResult GenerateInfrafamilialTaxa(FormCollection formCollection)
        {
            string[] keyTokens;
            int familyId = 0;
            string infraRank = String.Empty;
            string infraRankName = String.Empty;
            FamilyMapViewModel viewModel = new FamilyMapViewModel();
            //SubfamilyViewModel subfamilyViewModel = null;
            //TribeViewModel tribeViewModel = new TribeViewModel();
            //SubtribeViewModel subtribeViewModel = new SubtribeViewModel();

            foreach (var key in formCollection.AllKeys)
            {
                if (key == "FamilyID")
                {
                    familyId = Int32.Parse(formCollection[key]);
                }
                else
                {
                    keyTokens = key.Split('_');
                    infraRank = keyTokens[0];
                    infraRankName = formCollection[key];
                }

                //switch(infraRank)
                //{
                //    case "SUBFAMILY":
                //        subfamilyViewModel = new SubfamilyViewModel();
                //        subfamilyViewModel.Entity.FamilyID = familyId;
                //        subfamilyViewModel.Entity.SubfamilyName = infraRankName;
                //        subfamilyViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                //        subfamilyViewModel.Insert();
                //        break;
                //    case "TRIBE":
                //        tribeViewModel.Entity.FamilyID = familyId;
                //        tribeViewModel.Entity.TribeName = infraRankName;
                //        tribeViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                //        tribeViewModel.Insert();
                //        break;
                //    case "SUBTRIBE":
                //        subtribeViewModel.Entity.FamilyID = familyId;
                //        subtribeViewModel.Entity.SubtribeName = infraRankName;
                //        subtribeViewModel.Insert();
                //        break;
                //}
            }
            return null;
        }
       

        [HttpPost]
        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        //public JsonResult AddCitations(FormCollection formCollection)
        //{
        //    try
        //    {
        //        CitationViewModel viewModel = new CitationViewModel();

        //        if (!String.IsNullOrEmpty(formCollection["FamilyID"]))
        //        {
        //            viewModel.Entity.FamilyID = Int32.Parse(formCollection["FamilyID"]);
        //        }

        //        if (!String.IsNullOrEmpty(formCollection["IDList"]))
        //        {
        //            viewModel.Entity.ItemIDList = formCollection["IDList"];
        //        }
        //        viewModel.InsertByFamily();
        //        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}