using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.WebUI;
using NLog;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class GeographyController : BaseController, IController<GeographyViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Geography/";
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
            try
            {
                GeographyViewModel viewModel = new GeographyViewModel();
                viewModel.PageTitle = "Geography Search";
                viewModel.TableName = "geography";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Search()
        {
            try
            {
                GeographyViewModel viewModel = new GeographyViewModel();
                viewModel.PageTitle = "Geography Search";
                viewModel.Entity.TableName = "geography";
                //viewModel.DataCollectionRegions = new System.Collections.ObjectModel.Collection<Region>(viewModel.GetRegions());
                viewModel.DataCollectionCountries = new System.Collections.ObjectModel.Collection<Country>(viewModel.GetCountries());
                viewModel.DataCollection = new System.Collections.ObjectModel.Collection<Geography>(viewModel.GetGeographies());
                viewModel.Search();
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Search(GeographyViewModel viewModel)
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

        public ActionResult Edit(int entityId)
        {
            try
            {
                GeographyViewModel viewModel = new GeographyViewModel();
                viewModel.TableName = "geography";
                viewModel.TableCode = "Geography";
                if (entityId > 0)
                { 
                    viewModel.Get(entityId);
                    viewModel.EventAction = "Edit";
                    viewModel.PageTitle = String.Format("Edit Geography [{0}]: {1}", entityId, viewModel.Entity.GeographyText);
                    viewModel.DataCollectionCountries = new System.Collections.ObjectModel.Collection<Country>(viewModel.GetCountries());
                    viewModel.Countries = new SelectList(viewModel.GetCountries(),"CountryCode","CountryName");
                }
                else
                {
                    viewModel.EventAction = "Add";
                    viewModel.PageTitle = "Add Geography";
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
        public ActionResult Edit(GeographyViewModel viewModel)
        {
            try
            {
                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return View(viewModel);
                }

                viewModel.Entity.IsValid = viewModel.FromBool(viewModel.Entity.IsValidOption);

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
                return RedirectToAction("Edit", "Geography", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        public ActionResult Add()
        {
            try
            {
                GeographyViewModel viewModel = new GeographyViewModel();
                viewModel.TableName = "geography";
                viewModel.PageTitle = "Add Geography";
                viewModel.DataCollectionCountries = new System.Collections.ObjectModel.Collection<Country>(viewModel.GetCountries());
                viewModel.Countries = new SelectList(viewModel.GetCountries(),"CountryCode", "CountryName");
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public JsonResult Map(FormCollection formCollection)
        {
            GeographyMapViewModel viewModel = new GeographyMapViewModel();

            try
            {
                if (!String.IsNullOrEmpty(formCollection["ID"]))
                {
                    viewModel.Entity.ID = Int32.Parse(formCollection["ID"]);
                }

                if (!String.IsNullOrEmpty(formCollection["IDList"]))
                {
                    viewModel.ItemIDList = formCollection["IDList"];
                }
                viewModel.Map();

                //TODO
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult FolderItems(int folderId)
        {
            try
            {
                GeographyViewModel viewModel = new GeographyViewModel();
                viewModel.EventAction = "SEARCH";
                viewModel.EventValue = "FOLDER";
                viewModel.SearchEntity.FolderID = folderId;
                viewModel.SearchFolderItems();
                ModelState.Clear();
                return PartialView("~/Views/Geography/_List.cshtml", viewModel);
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

        //public JsonResult LoadSubContinents(string continentName)
        //{
        //    return null;
        //}

        //public JsonResult LoadCountries(string subContinentName)
        //{
        //    return null;
        //}

        //public JsonResult LoadStates(string countryCode)
        //{
        //    return null;
        //}

        //[HttpPost]
        //public PartialViewResult SearchSubcontinents(FormCollection formCollection)
        //{
        //    string partialViewName = "~/Views/Geography/_SubContinentList.cshtml";
        //    GeographyViewModel viewModel = new GeographyViewModel();

        //    if (!String.IsNullOrEmpty(formCollection["ContinentNameList"]))
        //    {
        //        viewModel.SearchEntity.ContinentNameList = formCollection["ContinentNameList"];
        //    }
        //    viewModel.SearchSubContinents(viewModel.SearchEntity);
        //    return PartialView(partialViewName, viewModel);
        //}
        
        [HttpPost]
        public PartialViewResult SearchCountries(FormCollection formCollection)
        {
            string partialViewName = "~/Views/Geography/_CountryList.cshtml";
            GeographyViewModel viewModel = new GeographyViewModel();

            if (!String.IsNullOrEmpty(formCollection["RegionIDList"]))
            {
                viewModel.SearchEntity.SubContinentIDList = formCollection["RegionIDList"];
            }

            if (!String.IsNullOrEmpty(formCollection["IncludeNonRegions"]))
            {
                viewModel.SearchEntity.IncludeNonRegions = formCollection["IncludeNonRegions"];
            }

            viewModel.SearchCountries(viewModel.SearchEntity);
            return PartialView(partialViewName, viewModel);
        }

        [HttpPost]
        public PartialViewResult SearchGeographies(FormCollection formCollection)
        {
            string partialViewName = "~/Views/Geography/_List.cshtml";
            GeographyViewModel viewModel = new GeographyViewModel();
            
            if (!String.IsNullOrEmpty(formCollection["ContinentRegionIDList"]))
            {
                viewModel.SearchEntity.ContinentIDList = formCollection["ContinentRegionIDList"];
            }

            if (!String.IsNullOrEmpty(formCollection["SubContinentRegionIDList"]))
            {
                viewModel.SearchEntity.SubContinentIDList = formCollection["SubContinentRegionIDList"];
            }

            if (!String.IsNullOrEmpty(formCollection["CountryCodeList"]))
            {
                viewModel.SearchEntity.CountryCodeList = formCollection["CountryCodeList"];
            }

            viewModel.Search();
            return PartialView(partialViewName, viewModel);
        }

        public ActionResult RenderLookupModal()
        {
            GeographyViewModel viewModel = new GeographyViewModel();
            return PartialView(BASE_PATH + "Modals/_Lookup.cshtml", viewModel);
        }

        [HttpPost]
        public PartialViewResult Lookup(FormCollection coll)
        {
            GeographyViewModel viewModel = new GeographyViewModel();

            if (!String.IsNullOrEmpty(coll["ContinentRegionID"]))
            {
                viewModel.SearchEntity.ContinentRegionID = Int32.Parse(coll["ContinentRegionID"]);
            }

            if (!String.IsNullOrEmpty(coll["SubContinentRegionID"]))
            {
                viewModel.SearchEntity.SubContinentRegionID = Int32.Parse(coll["SubContinentRegionID"]);
            }

            if (!String.IsNullOrEmpty(coll["CountryCode"]))
            {
                viewModel.SearchEntity.CountryCode = coll["CountryCode"];
            }

            if (!String.IsNullOrEmpty(coll["IsValid"]))
            {
                viewModel.SearchEntity.IsValid = coll["IsValid"];
            }
            viewModel.Search();
            return PartialView(BASE_PATH + "Modals/_SelectList.cshtml", viewModel);
        }

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
                GeographyViewModel viewModel = new GeographyViewModel();
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
