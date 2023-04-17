using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.WebUI;
using NLog;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class GeographyMapController : BaseController, IController<GeographyMapViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/GeographyMap/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public PartialViewResult _ListFolderItems(int folderId)
        {
            GeographyMapViewModel viewModel = new GeographyMapViewModel();
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
        public ActionResult Index()
        {
            try
            {
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
                viewModel.PageTitle = "Distribution Search";
                viewModel.TableName = "taxonomy_geography_map";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

             
                if (TempData.ContainsKey("GEO-MAP-SEARCH"))
                {
                    viewModel.SearchEntity = TempData["GEO-MAP-SEARCH"] as GeographyMapSearch;
                    viewModel.Search();
                }

                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Search(GeographyMapViewModel viewModel)
        {
            try 
            {
                //if (viewModel.EventAction == "SAVE-SEARCH")
                //{
                //    viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                //    viewModel.SaveSearch();
                //    Session["taxonomy_geography_map_SEARCH"] = viewModel.SearchEntity;
                //    viewModel.SearchCountries(new GeographySearch());
                //    viewModel.Countries = new SelectList(viewModel.DataCollectionCountries, "CountryCode", "CountryName");
                //}
                //else
                //{
                    viewModel.SearchEntity.IsValid = viewModel.SearchEntity.IsValidOption == true ? "Y" : null;
                    viewModel.Search();
                    //viewModel.SearchCountries(new GeographySearch());
                    //viewModel.Countries = new SelectList(viewModel.DataCollectionCountries, "CountryCode", "CountryName");
                    ModelState.Clear();
                //}
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult RunSavedSearch(int id)
        {
            AppUserDynamicQueryViewModel viewModel = new AppUserDynamicQueryViewModel();
            viewModel.Get(id);
            GeographyMapSearch geographyMapSearch = viewModel.Deserialize<GeographyMapSearch>(viewModel.Entity.QuerySyntax);
            TempData["GEO-MAP-SEARCH"] = geographyMapSearch;
            return RedirectToAction("Index", "GeographyMap");
        }
        public PartialViewResult _List(string eventValue = "", int speciesId = 0)
        {
            try
            {
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
                viewModel.SearchEntity = new GeographyMapSearch { SpeciesID = speciesId };
                viewModel.Search();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        public PartialViewResult _SelectList(int speciesId = 0)
        {
            try
            {
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
                viewModel.SearchEntity = new GeographyMapSearch { SpeciesID = speciesId };
                viewModel.Search();
                return PartialView(BASE_PATH + "_SelectList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public JsonResult _Add(GeographyMapViewModel viewModel)
        {
            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Insert();
                // TODO
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public PartialViewResult Add(FormCollection formCollection)
        //{
        //    GeographyMapViewModel viewModel = new GeographyMapViewModel();
        //    viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

        //    try
        //    {
        //        viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

        //        if (!String.IsNullOrEmpty(formCollection["SpeciesID"]))
        //        {
        //            viewModel.Entity.SpeciesID = Int32.Parse(formCollection["SpeciesID"]);
        //        }
        //        if (!String.IsNullOrEmpty(formCollection["IDList"]))
        //        {
        //            viewModel.Entity.ItemIDList = formCollection["IDList"];
        //        }
        //        viewModel.Insert();
        //        return PartialView(BASE_PATH + "_EditList.cshtml", viewModel);

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}
        public ActionResult Add(int speciesId = 0)
        {
            try
            {
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
                viewModel.TableName = "taxonomy_geography_map";
                viewModel.PageTitle = "Add Distribution";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
               
                if (speciesId > 0)
                {
                    SpeciesViewModel speciesViewModel = new SpeciesViewModel();
                    speciesViewModel.SearchEntity = new SpeciesSearch { ID = speciesId };
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

        //public PartialViewResult _Add()
        //{
        //    try
        //    {
        //        GeographyMapViewModel viewModel = new GeographyMapViewModel();
        //        viewModel.TableName = "taxonomy_geography_map";
        //        viewModel.PageTitle = "Add Distribution";
        //        viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

        //        GeographyViewModel geographyViewModel = new GeographyViewModel();
        //        //viewModel.Countries = new SelectList(geographyViewModel.GetCountries(""), "CountryCode", "CountryName");

        //        return PartialView(BASE_PATH + "_Edit.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}

        public ActionResult Edit(int entityId)
        {
            try
            {
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
                viewModel.TableName = "taxonomy_geography_map";
                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit Distribution [{0}]: {1}", entityId, viewModel.Entity.GeographyDescription + " to " + viewModel.Entity.SpeciesName);
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(GeographyMapViewModel viewModel)
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
                return RedirectToAction("Edit", "GeographyMap", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public JsonResult _Save(GeographyMapViewModel viewModel)
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

                return Json(new { geographyMap = viewModel.Entity }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { geographyMap = viewModel.Entity }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Map(int entityId = 0)
        {
            try
            {
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
                viewModel.PageTitle = "Map Geographies";
                viewModel.EventValue = "species";
                viewModel.Entity.SpeciesID = entityId;
                viewModel.Entity.TableName = "taxonomy_species";
                return View(viewModel);
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

 
        public PartialViewResult FolderItems(int folderId)
        {
            try
            {
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
                viewModel.EventAction = "SEARCH";
                viewModel.EventValue = "FOLDER";
                viewModel.SearchEntity.FolderID = folderId;
                viewModel.SearchFolderItems();
                ModelState.Clear();
                return PartialView("~/Views/GeographyMap/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        // ======================================================================================
        // MODALS 
        // ======================================================================================
        public ActionResult RenderLookupModal()
        {
            GeographyMapViewModel viewModel = new GeographyMapViewModel();
            return PartialView("~/Views/GeographyMap/Modals/_Lookup.cshtml", viewModel);
        }

        public PartialViewResult RenderEditModal(int speciesId)
        {
            GeographyMapViewModel viewModel = new GeographyMapViewModel();
            viewModel.TableName = "taxonomy_geography_map";
            return PartialView(BASE_PATH + "_Edit.cshtml", viewModel);
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
        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
                GeographyMapViewModel viewModel = new GeographyMapViewModel();
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
