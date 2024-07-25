using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetMenu(string eventAction, string eventValue, int entityId = 0)
        {
            MenuViewModel viewModel = new MenuViewModel();
            viewModel.EntityID = entityId;
            viewModel.EventAction = eventAction;
            viewModel.EventValue = eventValue;

            eventAction = eventAction.ToLower();
            eventValue = eventValue.ToLower();

            if ((eventAction == "home") && (eventValue == "index"))
            {
                return PartialView("~/Views/Components/_DefaultMenu.cshtml");
            }

            if ((eventAction == "taxonomy") && (eventValue == "index"))
            {
                return PartialView("~/Views/Components/_DefaultMenu.cshtml");
            }

            if (eventValue == "index")
            {
                return PartialView("~/Views/Components/_DefaultSearchMenu.cshtml", viewModel);
            }

            if ((eventValue == "add") || (eventValue == "edit"))
            {
                if ((eventAction == "Family") || (eventAction == "Genus") || (eventAction == "Species"))
                {
                    // Load context-based menu for the current taxon type.
                    return PartialView("~/Views/Taxonomy/" + eventAction + "/Components/_EditMenu.cshtml", viewModel);
                }
                else
                {
                    if (eventAction == "Classification")
                    {
                        return PartialView("~/Views/Taxonomy/Order/Components/_EditMenu.cshtml", viewModel);
                    }
                    else 
                    {
                        if (eventAction == "SysFolder")
                        {
                            return PartialView("~/Views/Components/_DefaultMenu.cshtml");
                        }
                        else
                        {
                            return PartialView("~/Views/Components/_DefaultEditMenu.cshtml", viewModel);
                        }
                    }
                }
            }
            return null;
        }
    }
}