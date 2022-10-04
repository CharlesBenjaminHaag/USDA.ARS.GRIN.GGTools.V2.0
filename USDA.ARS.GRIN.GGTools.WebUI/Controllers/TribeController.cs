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
    public class TribeController : BaseController, IController<TribeViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Add()
        {
            try 
            { 
                TribeViewModel viewModel = new TribeViewModel();
                viewModel.EventAction = "Add";
                viewModel.TableName = "taxonomy_tribe";
                viewModel.TableCode = "Tribe";
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
                TribeViewModel viewModel = new TribeViewModel();
                viewModel.PageTitle = RouteData.Route.GetRouteData(this.HttpContext).Values["controller"] + " " + RouteData.Route.GetRouteData(this.HttpContext).Values["action"];
                viewModel.TableName = "taxonomy_tribe";
                viewModel.TableCode = "Tribe";
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.EventAction = "Edit";
                    viewModel.PageTitle = String.Format(viewModel.EventAction + " " + viewModel.TableCode + String.Format(" [{0}]: {1}", viewModel.Entity.ID, viewModel.Entity.TribeName));
                }
                else
                {
                    viewModel.EventAction = "Add";
                    viewModel.PageTitle = viewModel.EventAction + " " + viewModel.TableCode;
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        [HttpPost]
        public ActionResult Edit(TribeViewModel viewModel)
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
                return RedirectToAction("Edit", "Tribe", new { entityId = viewModel.Entity.ID });
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
                TribeViewModel viewModel = new TribeViewModel();
                viewModel.EventAction = "SEARCH";
                viewModel.EventValue = "FOLDER";
                viewModel.SearchEntity.FolderID = folderId;
                viewModel.SearchFolderItems();
                ModelState.Clear();
                return PartialView("~/Views/Tribe/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        // GET: Tribe
        public ActionResult Index()
        {
            TribeViewModel viewModel = new TribeViewModel();
            viewModel.PageTitle = "Tribe Search";
            
            return View(viewModel);
        }

        public PartialViewResult SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public ActionResult Search(TribeViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();
                Session["TRIBE_SEARCH_RESULTS"] = viewModel;
                return View("~/Views/Tribe/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult SearchNotes(FormCollection formCollection)
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