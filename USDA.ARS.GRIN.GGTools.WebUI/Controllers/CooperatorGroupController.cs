using NLog;
using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class CooperatorGroupController : BaseController, IController<CooperatorGroupViewModel>
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

        public ActionResult Edit(CooperatorGroupViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        // GET: CooperatorGroup
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(CooperatorGroupViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}