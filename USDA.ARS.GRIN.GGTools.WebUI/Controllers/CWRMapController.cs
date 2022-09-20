using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class CWRMapController : BaseController, IController<CWRMapViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/CWRMap/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        [HttpPost]
        public ActionResult Index(CWRMapViewModel vm)
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Index()
        {
            try
            {
                CWRMapViewModel viewModel = new CWRMapViewModel();
                viewModel.PageTitle = "CWR Map Search";
                viewModel.TableName = "taxonomy_cwr_map";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(BASE_PATH + "Index.cshtml", viewModel);
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
                CWRMapViewModel viewModel = new CWRMapViewModel();
                viewModel.TableName = "taxonomy_cwr_map";
                viewModel.PageTitle = String.Format("Edit CWR Map [{0}]", entityId);
                viewModel.Get(entityId);
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(CWRMapViewModel viewModel)
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
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }
                return RedirectToAction("Edit", "CWRMap", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Map(int cropForCwrId = 0)
        {
            CWRMapViewModel viewModel = new CWRMapViewModel();
            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
            viewModel.Entity.CropForCWRID = cropForCwrId;
            viewModel.TableName = "taxonomy_cwr_map";

            try 
            {
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Map(CWRMapViewModel viewModel)
        {
            try
            {
                //TODO
                return View();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }


        [HttpPost]
        public ActionResult Search(CWRMapViewModel viewModel)
        {
            try
            {
                viewModel.EventAction = "SEARCH";
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

        [HttpPost]
        public PartialViewResult Add(FormCollection formCollection)
        {
            CWRMapViewModel viewModel = new CWRMapViewModel();
            FolderViewModel folderViewModel = new FolderViewModel();
            viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

            if (!String.IsNullOrEmpty(formCollection["IDList"]))
            {
                viewModel.Entity.ItemIDList = formCollection["IDList"];
            }
            
            

            if (!String.IsNullOrEmpty(formCollection["CropForCWRID"]))
            {
                viewModel.Entity.CropForCWRID = Int32.Parse(formCollection["CropForCWRID"]);
            }
            if (!String.IsNullOrEmpty(formCollection["GenepoolCode"]))
            {
                viewModel.Entity.GenepoolCode = formCollection["GenepoolCode"];
            }
            if (!String.IsNullOrEmpty(formCollection["CropCommonName"]))
            {
                viewModel.Entity.CropCommonName = formCollection["CropCommonName"];
            }

            // FOLDER
            if (!String.IsNullOrEmpty(formCollection["FolderTitle"]))
            {
                folderViewModel.Entity.FolderName = formCollection["FolderTitle"];
            }
            if (!String.IsNullOrEmpty(formCollection["FolderCategory"]))
            {
                folderViewModel.Entity.Category = formCollection["FolderCategory"];
            }
            if (!String.IsNullOrEmpty(formCollection["FolderDescription"]))
            {
                folderViewModel.Entity.Description = formCollection["FolderDescription"];
            }
            viewModel.InsertBatch();

            // Retrieve newly-added records.
            viewModel.EventAction = "SEARCH";
            viewModel.SearchEntity.CropForCWRID = viewModel.Entity.CropForCWRID; ;
            viewModel.Search();
            ModelState.Clear();
            return PartialView("~/Views/CWRMap/_SelectList.cshtml", viewModel);
        }

        public ActionResult Add(int cropForCwrId = 0)
        {
            try
            {
                CWRMapViewModel viewModel = new CWRMapViewModel();
                viewModel.TableName = "taxonomy_cwr_map";
                viewModel.PageTitle = "Add CWR Map";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Entity.CropForCWRID = cropForCwrId;
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

//        [HttpPost]
//        public JsonResult AddCitation(FormCollection coll)
//        {
//#pragma warning disable CS0219 // The variable 'citationId' is assigned but its value is never used
//            int citationId = 0;
//#pragma warning restore CS0219 // The variable 'citationId' is assigned but its value is never used
//            string entityIdList = String.Empty;
//            CWRMapViewModel viewModel = new CWRMapViewModel();
//            CitationViewModel citationViewModel = new CitationViewModel();
//            citationViewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
//            citationViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

//            try
//            {
//                if (!String.IsNullOrEmpty(coll["IDList"]))
//                {
//                    entityIdList = coll["IDList"];
//                }

//                if (!String.IsNullOrEmpty(coll["EntityID"]))
//                {
//                    viewModel.Entity.ID = Int32.Parse(coll["EntityID"]);
//                }

//                if (!String.IsNullOrEmpty(coll["CitationAuthorName"]))
//                {
//                    citationViewModel.Entity.CitationAuthorName = coll["CitationAuthorName"];
//                }

//                if (!String.IsNullOrEmpty(coll["CitationYear"]))
//                {
//                    citationViewModel.Entity.CitationYear = Int32.Parse(coll["CitationYear"]);
//                }

//                if (!String.IsNullOrEmpty(coll["VolumePage"]))
//                {
//                    citationViewModel.Entity.VolumeOrPage = coll["VolumePage"];
//                }

//                if (!String.IsNullOrEmpty(coll["DOIReference"]))
//                {
//                    citationViewModel.Entity.DOIReference = coll["DOIReference"];
//                }

//                if (!String.IsNullOrEmpty(coll["ReferenceTitle"]))
//                {
//                    citationViewModel.Entity.ReferenceTitle = coll["ReferenceTitle"];
//                }

//                if (!String.IsNullOrEmpty(coll["Description"]))
//                {
//                    citationViewModel.Entity.Description = coll["Description"];
//                }
//                citationViewModel.Insert();
//                return Json( new { citationId = citationViewModel.Entity.ID }, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex);
//                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
//            }
//        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(int folderId)
        {
            try
            {
                CWRMapViewModel viewModel = new CWRMapViewModel();
                viewModel.EventAction = "SEARCH";
                viewModel.EventValue = "FOLDER";
                viewModel.SearchEntity.FolderID = folderId;
                viewModel.SearchFolderItems();
                ModelState.Clear();
                return PartialView("~/Views/CWRMap/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _List(int cropForCwrId)
        {
            CWRMapViewModel viewModel = new CWRMapViewModel();
            try
            {
                viewModel.EventAction = "SEARCH";
                viewModel.SearchEntity.CropForCWRID = cropForCwrId;
                viewModel.Search();
                ModelState.Clear();
                return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml", "Error");
            }
        }

        [HttpPost]
        public PartialViewResult LookupNotes(FormCollection formCollection)
        {
            string partialViewName = "~/Views/CWRMap/Modals/_NoteSelectList.cshtml";
            CWRMapViewModel viewModel = new CWRMapViewModel();

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

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
    }
}
