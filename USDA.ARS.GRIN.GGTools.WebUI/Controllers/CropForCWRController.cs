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
    public class CropForCWRController : BaseController, IController<CropForCWRViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/CropForCWR/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
       
        public ActionResult Index()
        {
            try
            {
                CropForCWRViewModel viewModel = new CropForCWRViewModel();
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.PageTitle = "Crop For CWR Search";
                viewModel.TableName = "taxonomy_cwr_crop";
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public PartialViewResult _Search(FormCollection formCollection)
        {
            CropForCWRViewModel vm = new CropForCWRViewModel();

            if (!String.IsNullOrEmpty(formCollection["Name"]))
            {
                vm.SearchEntity.Name = formCollection["Name"];
                vm.EventAction = "search";
            }
            vm.HandleRequest();
            ModelState.Clear();
            return PartialView("~/Views/CropForCWR/_SelectList.cshtml", vm);
        }

        public PartialViewResult _Search()
        { return PartialView("~/Views/CropForCWR/_Search.cshtml", new CropForCWRViewModel()); }

        public ActionResult Search(CropForCWRViewModel viewModel)
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

        public ActionResult Add()
        {
            try
            {
                CropForCWRViewModel viewModel = new CropForCWRViewModel();
                viewModel.TableName = "taxonomy_cwr_crop";
                viewModel.PageTitle = "Add Crop For CWR";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(BASE_PATH + "Edit.cshtml", viewModel);
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

        public ActionResult Edit(int entityId = 0)
        {
            try
            {
                CropForCWRViewModel viewModel = new CropForCWRViewModel();
                viewModel.TableName = "taxonomy_cwr_crop";
                viewModel.TableCode = "CropForCWR";
                viewModel.PageTitle = String.Format("Edit Crop For CWR [{0}]", entityId);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                //viewModel.AuthenticatedUser = AuthenticatedUser;
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
        public ActionResult Edit(CropForCWRViewModel viewModel)
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
                return RedirectToAction("Edit", "CropForCWR", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Delete(FormCollection formCollection)
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult LookupNotes(FormCollection formCollection)
        {
            string partialViewName = "~/Views/CropForCWR/Modals/_NoteSelectList.cshtml";
            CropForCWRViewModel viewModel = new CropForCWRViewModel();

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
               
        public PartialViewResult FolderItems(int folderId)
        {
            CropForCWRViewModel viewModel = new CropForCWRViewModel();

            try
            {
                viewModel.SearchEntity.FolderID = folderId;
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
        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            CropForCWRViewModel viewModel = new CropForCWRViewModel();

            //try
            //{
            //    viewModel.SearchEntity.FolderID = folderId;
            //    viewModel.SearchFolderItems();
            //    ModelState.Clear();
            //    return PartialView(BASE_PATH + "_List.cshtml", viewModel);
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        }
    }
}
