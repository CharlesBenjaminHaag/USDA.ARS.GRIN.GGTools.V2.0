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
    public class GenusController : BaseController, IController<GenusViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Genus/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public PartialViewResult _ListFolderItems(int folderId)
        {
            GenusViewModel viewModel = new GenusViewModel();
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
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Add(int familyId = 0, int genusId = 0, string isType="", string rank = "")
        {
            GenusViewModel viewModel = new GenusViewModel();
            viewModel.TableName = "taxonomy_genus";
            viewModel.TableCode = "Genus";
            viewModel.PageTitle = String.Format("Add {0}", viewModel.ToTitleCase(rank));
            viewModel.Entity.IsAcceptedName = "Y";
            viewModel.Entity.Rank = rank;
            viewModel.Entity.IsAccepted = true;
            viewModel.Entity.IsAcceptedName = "Y";
            viewModel.IsTypeGenus = isType;

            if (genusId > 0)
            {
                GenusViewModel topRankGenusViewModel = new GenusViewModel();
                topRankGenusViewModel.SearchEntity.ID = genusId;
                topRankGenusViewModel.Search();
                viewModel.TopRankGenusEntity = topRankGenusViewModel.Entity;
                viewModel.Entity.ParentID = topRankGenusViewModel.Entity.ID;
                viewModel.Entity.ParentName = topRankGenusViewModel.Entity.Name;
            }

            if (familyId > 0)
            {
                viewModel.Entity.FamilyID = familyId;
                FamilyMapViewModel familyMapViewModel = new FamilyMapViewModel();
                familyMapViewModel.SearchEntity.ID = familyId;
                familyMapViewModel.Search();

                viewModel.FamilyMapEntity = familyMapViewModel.Entity;
                viewModel.Entity.FamilyID = viewModel.FamilyMapEntity.ID;
                viewModel.Entity.FamilyName = viewModel.FamilyMapEntity.FamilyName;
            }

            return View(BASE_PATH + "Edit.cshtml", viewModel);
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                GenusViewModel viewModel = new GenusViewModel();
                viewModel.Get(entityId);
                viewModel.PageTitle = viewModel.GetPageTitle();
                viewModel.TableName = "taxonomy_genus";
                viewModel.TableCode = "Genus";
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(GenusViewModel viewModel)
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

                if (viewModel.IsTypeGenus == "Y")
                {
                    FamilyMapViewModel familyMapViewModel = new FamilyMapViewModel();
                    familyMapViewModel.Get(viewModel.Entity.FamilyID);
                    familyMapViewModel.Entity.TypeGenusID = viewModel.Entity.ID;
                    familyMapViewModel.Update();
                }

                return RedirectToAction("Edit", "Genus", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                
                  Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public virtual ActionResult Index()
        {
            GenusViewModel viewModel = new GenusViewModel();

            try
            {
                viewModel.PageTitle = "Genus Search";
                viewModel.TableName = "taxonomy_genus";
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
        public ActionResult Search(GenusViewModel viewModel)
        {
            try
            {
                viewModel.PageTitle = "Genus Home";
                ViewBag.Title = viewModel.PageTitle;
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

        public PartialViewResult _List(string eventValue = "", int familyId = 0, string genusName = "")
        {
            GenusViewModel viewModel = new GenusViewModel();
            try
            {
                viewModel.SearchEntity = new GenusSearch { FamilyID = familyId, Name = genusName };
                viewModel.Search();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        
        public PartialViewResult _ListSpecies(int genusId)
        {
            FamilyMapViewModel viewModel = new FamilyMapViewModel();

            try
            {
                //viewModel.GetGenera(genusId);
                return PartialView("~/Views/Taxonomy/Genus/_ListSpecies.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListSynonyms(int genusId)
        {
            GenusViewModel viewModel = new GenusViewModel();

            try
            {
                viewModel.GetSynonyms(genusId);
                return PartialView(BASE_PATH + "_ListSynonyms.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListSubdivisions(string genusName)
        {
            GenusViewModel viewModel = new GenusViewModel();

            try
            {
                viewModel.GetSubdivisions(genusName);
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
            GenusViewModel viewModel = new GenusViewModel();

            try
            {
                if (!String.IsNullOrEmpty(formCollection["GenusName"]))
                {
                    viewModel.SearchEntity.FullName = formCollection["GenusName"];
                }

                if (!String.IsNullOrEmpty(formCollection["IsAcceptedName"]))
                {
                    viewModel.SearchEntity.IsAcceptedName = formCollection["IsAcceptedName"];
                }

                viewModel.Search();
                return PartialView(BASE_PATH + "/Modals/_SelectList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }

        }

        public ActionResult Search(SpeciesViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            GenusViewModel viewModel = new GenusViewModel();

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
        [HttpPost]
        public PartialViewResult LookupNotes(FormCollection formCollection)
        {
            string partialViewName = "~/Views/Genus/Modals/_NoteSelectList.cshtml";
            GenusViewModel viewModel = new GenusViewModel();

            if (!String.IsNullOrEmpty(formCollection["TableName"]))
            {
                viewModel.SearchEntity.TableName = formCollection["TableName"];
            }

            if (!String.IsNullOrEmpty(formCollection["Note"]))
            {
                viewModel.SearchEntity.Note = formCollection["Note"];
            }

            viewModel.SearchNotes();
            return PartialView(partialViewName, viewModel);
        }

        private string GetPartialViewName(string rank)
        {
            //NOTE Refactor somehow. (CBH 12/14/21)
            switch (rank.ToUpper())
            {
                case "GENUS":
                    return "_GenusRankDetail.cshtml";
                case "SUBGENUS":
                    return "_SubgenusRankDetail.cshtml";
                case "SECTION":
                    return "_SectionRankDetail.cshtml";
                case "SUBSECTION":
                    return "_SubsectionRankDetail.cshtml";
                case "SERIES":
                    return "_SeriesRankDetail.cshtml";
                case "SUBSERIES":
                    return "_SubseriesRankDetail.cshtml";
                default:
                    return "";
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
                GenusViewModel viewModel = new GenusViewModel();
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
