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
using System.Reflection;
using USDA.ARS.GRIN.GGTools.DataLayer;
using System.Linq.Expressions;

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
            DataTable dtImported = new DataTable();
            SysTableViewModel sysTableViewModel = new SysTableViewModel();
            SysTableField sysTableField = new SysTableField();
            SpeciesImport speciesImport = new SpeciesImport();
            int primaryKeyValue = 0;

            if (!viewModel.Validate())
            {
                if (viewModel.ValidationMessages.Count > 0) return View("~/Views/Import/Index.cshtml", viewModel);
            }

            viewModel.ImportFileName = viewModel.DocumentUpload.FileName;

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
                    dtImported = result.Tables[0];
                    
                    switch (viewModel.SysTableName)
                    {
                        case "Species":
                            SpeciesViewModel speciesViewModel = new SpeciesViewModel();

                            // REFACTOR once logic makes more sense (CBH, 2/2/24)

                            foreach (DataRow dr in dtImported.Rows)
                            {
                                speciesImport = new SpeciesImport();

                                foreach (DataColumn rowCol in dtImported.Columns)
                                {
                                    sysTableField = viewModel.GetColumnInfo(viewModel.SysTableName, rowCol.ColumnName);

                                    switch (rowCol.ColumnName)
                                    {
                                        case "ID":
                                            primaryKeyValue = Int32.Parse(dr[rowCol.ColumnName].ToString());
                                            speciesViewModel.Get(primaryKeyValue);
                                            speciesImport.ID = speciesViewModel.Entity.ID;
                                            break;
                                        case "Name":
                                        case "Epithet":
                                            speciesImport.SpeciesName = dr[rowCol.ColumnName].ToString();
                                            speciesImport.OriginalSpeciesName = speciesViewModel.Entity.SpeciesName;
                                            break;
                                        case "Authority":
                                            speciesImport.SpeciesAuthority = dr[rowCol.ColumnName].ToString();
                                            speciesImport.OriginalSpeciesAuthority = speciesViewModel.Entity.SpeciesAuthority;
                                            break;
                                        case "Protologue":
                                            speciesImport.Protologue = dr[rowCol.ColumnName].ToString();
                                            speciesImport.OriginalProtologue = speciesViewModel.Entity.Protologue;
                                            break;

                                    }


                                }
                                viewModel.DataCollectionSpeciesImport.Add(speciesImport);
                            }
                            break;
                        case "Citation":
                            CitationViewModel citationViewModel = new CitationViewModel();
                            break;
                        case "Literature":
                            CropForCWRViewModel cropForCWRViewModel = new CropForCWRViewModel();
                            break;
                        case "CWR Map":
                            CWRMapViewModel cWRMapViewModel = new CWRMapViewModel();
                            break;
                        case "CWR Trait":
                            CWRTraitViewModel cWRTraitViewModel = new CWRTraitViewModel();
                            break;
                    }
                }
            }
            return View("~/Views/Import/Index.cshtml", viewModel);
        }

        [HttpPost]
        public PartialViewResult PostImport()
        {
            ImportViewModel viewModel = new ImportViewModel();

            //TODO

            return PartialView("~/Views/Import/_ListImport.cshtml");
        }
    }
}
