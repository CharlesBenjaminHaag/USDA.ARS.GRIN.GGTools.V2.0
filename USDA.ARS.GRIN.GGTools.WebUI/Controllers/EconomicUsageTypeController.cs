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
    public class EconomicUsageTypeController : BaseController, IController<EconomicUsageTypeViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/EconomicUsageType/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Search(EconomicUsageTypeViewModel viewModel)
        {
            try
            {
                Session[SessionKeyName] = viewModel;
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
        public PartialViewResult Lookup(EconomicUsageTypeViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();
                return PartialView(BASE_PATH + "/Modals/_SelectList.cshtml", viewModel);
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
                EconomicUsageTypeViewModel viewModel = new EconomicUsageTypeViewModel();
                viewModel.TableName = "taxonomy_economic_usage_type";
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
                EconomicUsageTypeViewModel viewModel = new EconomicUsageTypeViewModel();
                viewModel.EventValue = "EconomicUsageType";
                viewModel.TableName = "taxonomy_economic_usage_type";
                viewModel.PageTitle = "Add Economic Usage Type";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(BASE_PATH + "Edit.cshtml", viewModel);
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
                EconomicUsageTypeViewModel viewModel = new EconomicUsageTypeViewModel();
                viewModel.TableName = "taxonomy_use";
                viewModel.TableCode = "EconomicUsageType";
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                    viewModel.EventAction = "Edit";
                    viewModel.PageTitle = String.Format("Edit Economic Usage Type [{0}]", entityId);
                }
                else
                {
                    viewModel.EventAction = "Add";
                    viewModel.PageTitle = "Add  Economic Use";
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
        public ActionResult Edit(EconomicUsageTypeViewModel viewModel)
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
                return RedirectToAction("Edit", "EconomicUsageType", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
        public PartialViewResult RenderLookupModal()
        {
            EconomicUsageTypeViewModel viewModel = new EconomicUsageTypeViewModel();
            return PartialView(BASE_PATH + "/Modals/_Lookup.cshtml", viewModel);
        }
    }
}
