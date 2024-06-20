using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class OrderRequestController : BaseController, IController<OrderRequestViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(OrderRequestViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public ActionResult Index()
        {
            throw new NotImplementedException();
        }

        public ActionResult Search(OrderRequestViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult _Get(int entityId = 0, int webOrderRequestId = 0)
        {
            OrderRequestViewModel viewModel = new OrderRequestViewModel();

            try
            {
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                }
                else 
                { 
                    if (webOrderRequestId > 0)
                    {
                        viewModel.SearchEntity.WebOrderRequestID = webOrderRequestId;
                        viewModel.Search();
                    }
                }
                return PartialView("~/Views/OrderRequest/_Detail.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        public PartialViewResult _ListItems(int orderRequestId, int webOrderRequestId)
        {
            try
            {
                OrderRequestViewModel viewModel = new OrderRequestViewModel();
                if (webOrderRequestId > 0)
                { 
                    viewModel.SearchEntity.WebOrderRequestID = webOrderRequestId;
                }
                viewModel.Search();
                return PartialView("~/Views/WebOrder/_ListItems.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

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
        
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult View(int entityId)
        {
            return View();
        }
    }
}