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
    public class SubtribeController : BaseController, IController<SubtribeViewModel>
    {
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
        [HttpPost]
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Add()
        {
            try 
            { 
                SubtribeViewModel viewModel = new SubtribeViewModel();
                viewModel.EventAction = "Add";
                viewModel.TableName = "taxonomy_subtribe";
                viewModel.TableCode = "Subtribe";
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
                SubtribeViewModel viewModel = new SubtribeViewModel();
                viewModel.PageTitle = RouteData.Route.GetRouteData(this.HttpContext).Values["controller"] + " " + RouteData.Route.GetRouteData(this.HttpContext).Values["action"];
                viewModel.TableName = "taxonomy_subtribe";
                viewModel.TableCode = "Subtribe";
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.EventAction = "Edit";
                    viewModel.PageTitle = String.Format(viewModel.EventAction + " " + viewModel.TableCode + String.Format(" [{0}]: {1}", viewModel.Entity.ID, viewModel.Entity.SubtribeName));
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
        public ActionResult Edit(SubtribeViewModel viewModel)
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
                return RedirectToAction("Edit", "Subtribe", new { entityId = viewModel.Entity.ID });
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

        public ActionResult Index()
        {
            SubtribeViewModel viewModel = new SubtribeViewModel();
            viewModel.PageTitle = "Subtribe Search";
            return View(viewModel);
        }

        public PartialViewResult SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public ActionResult Search(SubtribeViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();
                Session["SUBTRIBE_SEARCH_RESULTS"] = viewModel;
                return View("~/Views/Subtribe/Index.cshtml", viewModel);
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