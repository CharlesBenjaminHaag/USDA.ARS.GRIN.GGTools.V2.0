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
    public class RegulationController : BaseController, IController<RegulationViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/Regulation/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        public ActionResult Index()
        {
            try
            {
                RegulationViewModel viewModel = new RegulationViewModel();
                viewModel.PageTitle = "Regulation Search";
                viewModel.TableName = "taxonomy_regulation";
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Add()
        {
            try
            {
                RegulationViewModel viewModel = new RegulationViewModel();
                viewModel.EventAction = "Add";
                viewModel.EventValue = "Regulation";
                viewModel.TableName = "taxonomy_regulation";
                viewModel.PageTitle = "Add Regulation";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
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
            RegulationMapViewModel viewModel = new RegulationMapViewModel();

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

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                RegulationViewModel viewModel = new RegulationViewModel();
                viewModel.TableName = "taxonomy_regulation";
                viewModel.TableCode = "Regulation";
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.EventAction = "Edit";
                    viewModel.PageTitle = String.Format("Edit Regulation [{0}]: {1}", entityId, viewModel.Entity.Description);
                }
                else
                {
                    viewModel.EventAction = "Add";
                    viewModel.PageTitle = "Add Regulation";
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
        public ActionResult Edit(RegulationViewModel viewModel)
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
                return RedirectToAction("Edit", "Regulation", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult FolderItems(int folderId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Search(RegulationViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();
                return View("~/Views/Taxonomy/Regulation/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
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
            RegulationViewModel viewModel = new RegulationViewModel();
            return PartialView("~/Views/Regulation/Modals/_Lookup.cshtml", viewModel);
        }

        [HttpPost]
        public PartialViewResult Lookup(FormCollection coll)
        {
            RegulationViewModel viewModel = new RegulationViewModel();

            if (!String.IsNullOrEmpty(coll["GeographyID"]))
            {
                viewModel.SearchEntity.GeographyID = Int32.Parse(coll["GeographyID"]);
            }

            if (!String.IsNullOrEmpty(coll["TypeCode"]))
            {
                viewModel.SearchEntity.RegulationTypeCode = coll["TypeCode"];
            }

            if (!String.IsNullOrEmpty(coll["LevelCode"]))
            {
                viewModel.SearchEntity.RegulationLevelCode = coll["LevelCode"];
            }

            if (!String.IsNullOrEmpty(coll["LevelCode"]))
            {
                viewModel.SearchEntity.RegulationLevelCode = coll["LevelCode"];
            }

            if (!String.IsNullOrEmpty(coll["IsMultiSelect"]))
            {
                viewModel.IsMultiSelectable = coll["IsMultiSelect"];
            }

            viewModel.Search();
            return PartialView("~/Views/Regulation/Modals/_SelectList.cshtml", viewModel);
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
                RegulationViewModel viewModel = new RegulationViewModel();
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
