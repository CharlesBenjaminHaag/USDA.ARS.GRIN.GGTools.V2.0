using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class SysFolderController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                return View(viewModel);
                SetPageTitle();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Search(SysFolderViewModel viewModel)
        {
            try
            {
                Session[SessionKeyName] = viewModel;
                viewModel.EventAction = "SEARCH";
                viewModel.Search();
                ModelState.Clear();
                viewModel.TableName = "taxonomy_author";
                return View("~/Views/SysFolder/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public PartialViewResult Edit(SysFolderViewModel viewModel)
        {
            try
            {
                if (viewModel.Entity.ID == 0)
                {
                    viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Insert();
                
                
                    //TODO
                    //if type is "DYN", look for session object with folder table name
                
                }
                else
                {
                    viewModel.Entity.ModifiedByCooperatorID = AuthenticatedUser.CooperatorID;
                    viewModel.Update();
                }

                // If there are tag attributes, add specified tag.
                //if (viewModel.IsFavoriteSelector == true)
                //{
                //    SysTagViewModel sysTagViewModel = new SysTagViewModel();
                //    sysTagViewModel.Entity.TagText = "Favorites";
                //    sysTagViewModel.Entity.TagFormatString = "";
                //    sysTagViewModel.Entity.TableName = "sys_folder";
                //    sysTagViewModel.Entity.IDNumber = viewModel.Entity.ID;
                //    sysTagViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                //    sysTagViewModel.Insert();
                //}

                // Re-retrieve new folder to verify existence.
                viewModel.SearchEntity.ID = viewModel.Entity.ID;
                viewModel.Get(viewModel.Entity.ID);
                return PartialView("~/Views/SysFolder/Components/_Confirmation.cshtml", viewModel);

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                viewModel.TableCode = "SysFolder";
                viewModel.TableName = "sys_folder";
                viewModel.Get(entityId);
                viewModel.GetItems(entityId);
                viewModel.GetCooperators(entityId);
                viewModel.GetSysTags("sys_folder", entityId);
                viewModel.GetSysTables(entityId);
                SetPageTitle();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
               SysFolderViewModel viewModel = new SysFolderViewModel();
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

        [HttpPost]
        public JsonResult DeleteItems(FormCollection coll)
        {
            SysFolderViewModel viewModel = new SysFolderViewModel();
            viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;

            try
            {
                viewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;

                if (!String.IsNullOrEmpty(coll["FolderID"]))
                {
                    viewModel.SearchEntity.ID = Int32.Parse(coll["FolderID"]);
                }

                if (!String.IsNullOrEmpty(coll["ItemIDList"]))
                {
                    viewModel.ItemIDList = coll["ItemIDList"].ToString();
                }

                //viewModel.Get();
                viewModel.DeleteItems();

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult GetEditModal()
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                return PartialView("~/Views/SysFolder/Modals/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        public PartialViewResult GetDynamicEditModal()
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                return PartialView("~/Views/SysFolder/Modals/_EditDynamic.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        #region Components

        /// <summary>
        /// Retrieves an icon-formatted list of folders. Defaults to folders owned by the logged-in user.
        /// </summary>
        /// <returns></returns>

        public PartialViewResult ComponentListWithIcons()
        {
            try
            {
                SysFolderViewModel viewModel = new SysFolderViewModel();
                viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.Search();
                return PartialView("~/Views/SysFolder/Components/_ListWithIcons.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult Component_Widget()
        {
            SysFolderViewModel viewModel = new SysFolderViewModel();
            string sysFolderId = Request.QueryString["sysFolderId"];
            try
            {
                if (!String.IsNullOrEmpty(sysFolderId))
                {
                    viewModel.Get(Int32.Parse(sysFolderId));
                    return PartialView("~/Views/SysFolder/Components/_Widget.cshtml", viewModel);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
               

        //public PartialViewResult Component_SysFolderCooperatorMapEditor()
        //{
        //    try
        //    {
        //        SysFolderCooperatorMapViewModel viewModel = new SysFolderCooperatorMapViewModel();
        //        viewModel.SearchEntity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
        //        viewModel.Search();
        //        return PartialView("~/Views/SysFolder/Components/_ListWithIcons.cshtml", viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}

        #endregion
    }
}