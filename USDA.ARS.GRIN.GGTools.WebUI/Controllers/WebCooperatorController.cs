﻿using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class WebCooperatorController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public PartialViewResult _Get(int entityId, int cooperatorId = 0)
        {
            try
            {
                WebCooperatorViewModel viewModel = new WebCooperatorViewModel();
                viewModel.Get(entityId, cooperatorId);
                viewModel.PageTitle = String.Format("Edit Web Cooperator [{0}]: {1}", entityId, viewModel.Entity.AssembledName);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                viewModel.CooperatorID = cooperatorId;
                return PartialView("~/Views/WebCooperator/_Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult Copy(WebCooperatorViewModel viewModel)
        {
            viewModel.Copy(viewModel.CooperatorID);
            return _Get(viewModel.Entity.ID);
        }

        public PartialViewResult Save(WebCooperatorViewModel viewModel)
        {
            try
            {
                //if (!viewModel.Validate())
                //{
                //    if (viewModel.ValidationMessages.Count > 0) return View(viewModel);
                //}

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
                return _Get(viewModel.Entity.ID);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        [HttpPost]
        public ActionResult Edit(WebCooperatorViewModel viewModel)
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
                return RedirectToAction("Edit", "Cooperator", new { entityId = viewModel.Entity.ID });
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
                WebCooperatorViewModel viewModel = new WebCooperatorViewModel();
                viewModel.Get(entityId);
                viewModel.PageTitle = String.Format("Edit Web Cooperator [{0}]: {1}, {2}", entityId, viewModel.Entity.LastName, viewModel.Entity.FirstName);
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.AuthenticatedUser = AuthenticatedUser;
                return View("~/Views/WebCooperator/Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        // ******************************************************************************
        // BEGIN OLD CODE
        // ******************************************************************************

        public PartialViewResult _ListFolderItems(int sysFolderId)
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
        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        //public PartialViewResult _Edit(int entityId, string environment = "")
        //{
        //    try
        //    {
        //        WebCooperatorViewModel viewModel = new WebCooperatorViewModel();
        //        viewModel.View(entityId, environment);
        //        return PartialView(viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return PartialView("~/Views/Error/_InternalServerError.cshtml");
        //    }
        //}
        
        //public ActionResult Edit(int entityId)
        //{
        //    try
        //    {
        //        WebCooperatorViewModel viewModel = new WebCooperatorViewModel();
        //        viewModel.View(entityId, "");
        //        viewModel.PageTitle = String.Format("Edit Web Cooperator [{0}]: {1}", entityId, viewModel.Entity.AssembledName);
        //        viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
        //        viewModel.AuthenticatedUser = AuthenticatedUser;
        //        return View(viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex);
        //        return RedirectToAction("InternalServerError", "Error");
        //    }
        //}

        //[HttpPost]
        //public ActionResult Edit(CooperatorViewModel viewModel)
        //{
        //    throw new NotImplementedException();
        //}

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        // GET: WebCooperator
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(CooperatorViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}