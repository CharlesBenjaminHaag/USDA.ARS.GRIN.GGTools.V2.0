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
    public class CodeValueController : BaseController, IController<CodeValueViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        public PartialViewResult _ListFolderItems(int appUserItemFolderId)
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
            CodeValueViewModel viewModel = new CodeValueViewModel();
            viewModel.EventAction = "Add";
            viewModel.TableName = "code_value";
            viewModel.TableCode = "CodeValue";
            viewModel.PageTitle = viewModel.EventAction + " " + viewModel.TableCode;
            viewModel.Entity.SysLangID = 1;
            return View("~/Views/" + viewModel.TableCode + "/Edit.cshtml", viewModel);
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult _Edit(int entityId)
        {
            try
            {
                CodeValueViewModel viewModel = new CodeValueViewModel();
                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit Code Value [{0}]", viewModel.Entity.ID);
                return PartialView("~/Views/CodeValue/Modals/_Detail.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public JsonResult Save(CodeValueViewModel viewModel)
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
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        public ActionResult Edit(int entityId)
        {
            try
            {
                CodeValueViewModel viewModel = new CodeValueViewModel();
                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit Code Value [{0}]", viewModel.Entity.ID);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        
        [HttpPost]
        public ActionResult Edit(CodeValueViewModel viewModel)
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
                return RedirectToAction("Edit", "CodeValue", new { entityId = viewModel.Entity.ID });
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
            CodeValueViewModel viewModel = new CodeValueViewModel();
            viewModel.PageTitle = "Code Value Search";
            return View(viewModel);
        }
        
        public ActionResult Explorer()
        {
            CodeValueViewModel viewModel = new CodeValueViewModel();
            return View("~/Views/CodeValue/Explorer/Index.cshtml", viewModel);
        }
        [HttpPost]
        public ActionResult Index(CodeValueViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Search(CodeValueViewModel viewModel)
        {
            try
            {
                viewModel.Search();
                ModelState.Clear();
                return View("~/Views/CodeValue/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public PartialViewResult _List(string groupName)
        {
            CodeValueViewModel viewModel = new CodeValueViewModel();
            viewModel.SearchEntity.GroupName = groupName;
            viewModel.Search();
            return PartialView("~/Views/CodeValue/_List.cshtml", viewModel);
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        #region Components

        public PartialViewResult Component_Editor(int codeValueId)
        {
            CodeValueViewModel viewModel = new CodeValueViewModel();
            
            try
            {
                viewModel.Get(codeValueId);
                return PartialView("~/Views/CodeValue/Components/_Editor.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult Component_MainSidebar()
        {
            CodeValueViewModel viewModel = new CodeValueViewModel();
            try 
            {
            
                return PartialView("~/Views/CodeValue/Components/_MainSidebar.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult Component_SelectList(string groupName)
        {
            CodeValueViewModel viewModel = new CodeValueViewModel();
            try
            {
                viewModel.SearchEntity.GroupName = groupName;
                viewModel.Search();
                //TODO
                return PartialView("~/Views/CodeValue/Components/_SelectList.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        #endregion
    }
}