using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    public class SubfamilyController : BaseController, IController<SubfamilyViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            return null; 
        }

        public ActionResult Add()
        {
            try 
            { 
            SubfamilyViewModel viewModel = new SubfamilyViewModel();
            viewModel.EventAction = "Add";
            viewModel.TableName = "taxonomy_subfamily";
            viewModel.TableCode = "Subfamily";
            viewModel.PageTitle = viewModel.EventAction + " " + viewModel.TableCode;
            return View("~/Views/" + viewModel.TableCode + "/Edit.cshtml", viewModel);
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
                SubfamilyViewModel viewModel = new SubfamilyViewModel();
                viewModel.TableName = "just_subfamily";
                viewModel.TableCode = "Subfamily";
                //viewModel.IsSingleSelect = true;
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.EventAction = "Edit";
                    viewModel.PageTitle = String.Format("Edit Subfamily [{0}]: {1}", entityId, viewModel.Entity.SubfamilyName);
                    if (viewModel.Entity.IsAcceptedName == "Y")
                    {
                        viewModel.PageTitle += "<span class='badge' style='margin-left:10px; margin-top:0px;'>Accepted</span>";
                    }
                }
                else
                {
                    viewModel.EventAction = "Add";
                    viewModel.PageTitle = "Add Subfamily";
                }
                return View("EditSubfamily.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(SubfamilyViewModel viewModel)
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
                    if (viewModel.Entity.SetAccepted == true)
                    {
                        viewModel.Entity.AcceptedID = viewModel.Entity.ID;
                    }
                    viewModel.Update();
                }
                return RedirectToAction("Edit", "Subfamily", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Index()
        {
            SubfamilyViewModel viewModel = new SubfamilyViewModel();
            viewModel.PageTitle = "Subfamily Search";
            return View(viewModel);
        }

        public ActionResult Search(SubfamilyViewModel viewModel)
        {
            try
            {
                viewModel.EventAction = "SEARCH";
                viewModel.SearchEntity.FamilyIDList = viewModel.ItemIDList;
                viewModel.Search();
                ModelState.Clear();
                Session["SUBFAMILY_SEARCH_RESULTS"] = viewModel;
                return View("~/Views/Subfamily/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        //[HttpPost]
        //public PartialViewResult AcceptedNameLookup(FormCollection formCollection)
        //{
        //    string partialViewName = "~/Views/Subfamily/Modals/_SelectList.cshtml";
        //    SubfamilyViewModel viewModel = new SubfamilyViewModel();

        //    if (!String.IsNullOrEmpty(formCollection["LookupAcceptedName"]))
        //    {
        //        viewModel.SearchEntity.Name = formCollection["LookupAcceptedName"];
        //    }

        //    //if (!String.IsNullOrEmpty(formCollection["IsAcceptedName"]))
        //    //{
        //    //    viewModel.SearchEntity.IsAcceptedName = formCollection["IsAcceptedName"];
        //    //}

        //    //if (!String.IsNullOrEmpty(formCollection["IsSingleSelect"]))
        //    //{
        //    //    viewModel.IsSingleSelect = viewModel.ConvertBool(formCollection["IsSingleSelect"]);
        //    //}

        //    viewModel.AcceptedNameSearch(viewModel.SearchEntity.Name);
        //    return PartialView(partialViewName, viewModel);
        //}

        //[HttpPost]
        //public PartialViewResult AddInfrafamilialTaxa(FormCollection formCollection)
        //{
        //    InfrafamilyViewModel viewModel = new InfrafamilyViewModel();

        //    if (!String.IsNullOrEmpty(formCollection["SubfamilyID"]))
        //    {
        //        viewModel.SubfamilyID = Int32.Parse(formCollection["SubfamilyID"]);
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
        //    return PartialView("~/Views/Family/Modals/_InfrafamilialTaxonEditGrid.cshtml", viewModel);
        //}

        //[HttpPost]
        //public JsonResult GenerateInfrafamilialTaxa(FormCollection formCollection)
        //{
        //    string[] keyTokens;
        //    int familyId = 0;
        //    int subfamilyId = 0;
        //    int tribeId = 0;
        //    string infraRank = String.Empty;
        //    string infraRankName = String.Empty;
        //    FamilyViewModel viewModel = new FamilyViewModel();
        //    SubfamilyViewModel subfamilyViewModel = null;
        //    TribeViewModel tribeViewModel = new TribeViewModel();
        //    SubtribeViewModel subtribeViewModel = new SubtribeViewModel();

        //    foreach (var key in formCollection.AllKeys)
        //    {
        //        if (key == "FamilyID")
        //        {
        //            familyId = Int32.Parse(formCollection[key]);
        //        }
        //        else
        //        {
        //            if (key == "SubfamilyID")
        //            {
        //                subfamilyId = Int32.Parse(formCollection[key]);
        //            }
        //            else
        //            {
        //                if (key == "TribeID")
        //                {
        //                    tribeId = Int32.Parse(formCollection[key]);
        //                }
        //                else
        //                {
        //                    keyTokens = key.Split('_');
        //                    infraRank = keyTokens[0];
        //                    infraRankName = formCollection[key];
        //                }
        //            }
        //        }

        //        switch (infraRank)
        //        {
        //            case "SUBFAMILY":
        //                subfamilyViewModel = new SubfamilyViewModel();
        //                subfamilyViewModel.Entity.FamilyID = familyId;
        //                subfamilyViewModel.Entity.SubfamilyName = infraRankName;
        //                subfamilyViewModel.Insert();
        //                break;
        //            case "TRIBE":
        //                tribeViewModel.Entity.FamilyID = familyId;
        //                tribeViewModel.Entity.SubfamilyID = subfamilyId;
        //                tribeViewModel.Entity.TribeName = infraRankName;
        //                tribeViewModel.Insert();
        //                break;
        //            case "SUBTRIBE":
        //                subtribeViewModel.Entity.FamilyID = familyId;
        //                subtribeViewModel.Entity.SubfamilyID = subfamilyId;
        //                subtribeViewModel.Entity.TribeID = tribeId;
        //                subtribeViewModel.Entity.SubtribeName = infraRankName;
        //                subtribeViewModel.Insert();
        //                break;
        //        }
        //    }
        //    return null;
        //}

        public PartialViewResult FolderItems(int folderId)
        {
            try
            {
                SubfamilyViewModel viewModel = new SubfamilyViewModel();
                viewModel.EventAction = "SEARCH";
                viewModel.EventValue = "FOLDER";
                viewModel.SearchEntity.FolderID = folderId;
                viewModel.SearchFolderItems();
                ModelState.Clear();
                return PartialView("~/Views/Subfamily/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        public PartialViewResult SearchNotes(string searchText)
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
    }
}