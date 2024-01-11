using System.Web.Mvc;
using System;
using System.Collections.Generic;
using ExcelDataReader;
using System.Data;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class ImportController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        public ActionResult Index(string eventAction = "", int folderId = 0)
        {
            try
            {
                ImportViewModel viewModel = new ImportViewModel();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Post(ImportViewModel viewModel)
        {
            // TODO:

            using (var stream = viewModel.DocumentUpload.InputStream)
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true,
                        }
                    });
                    viewModel.DataCollectionDataTable = result.Tables[0];
                };
            }

            // Get file stream

            // Use EDR to load into datatable

            // Return datatable in VM

            return View("~/Views/Import/Index.cshtml", viewModel);
        }
       
    }
}
