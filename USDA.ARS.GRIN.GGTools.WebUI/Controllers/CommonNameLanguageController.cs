using System.Web.Mvc;
using System;
using USDA.ARS.GRIN.GGTools.WebUI;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class CommonNameLanguageController : BaseController, IController<CommonNameLanguageViewModel>
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/CommonNameLanguage/";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Add()
        {
            try
            {
                CommonNameLanguageViewModel viewModel = new CommonNameLanguageViewModel();
                viewModel.TableName = "taxonomy_common_name_language";
                viewModel.PageTitle = "Add Common Name Language";
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                return View(BASE_PATH + "Edit.cshtml", viewModel);
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
        [HttpPost]
        public ActionResult Edit(CommonNameLanguageViewModel viewModel)
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
                return RedirectToAction("Edit", "CommonNameLanguage", new { entityId = viewModel.Entity.ID });
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
                CommonNameLanguageViewModel viewModel = new CommonNameLanguageViewModel();
                viewModel.TableName = "taxonomy_common_name_language";
                viewModel.Get(entityId);
                viewModel.EventAction = "Edit";
                viewModel.PageTitle = String.Format("Edit Common Name Language");
                return View(BASE_PATH + "Edit.cshtml", viewModel);
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

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Index()
        {
            try
            {
                CommonNameLanguageViewModel viewModel = new CommonNameLanguageViewModel();
                viewModel.PageTitle = "Common Name Language Search";
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Search(CommonNameLanguageViewModel viewModel)
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

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
    }
}