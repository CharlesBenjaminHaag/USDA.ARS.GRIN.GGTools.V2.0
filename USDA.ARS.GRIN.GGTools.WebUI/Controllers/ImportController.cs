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
            SysTableViewModel sysTableViewModel = new SysTableViewModel();
            SysTableField sysTableField = new SysTableField();
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
                    viewModel.DataCollectionDataTable = result.Tables[0];

                    // TODO
                    // Iterate through columns and look for PK field
                    // Look for value in field
                    // If present -- update
                    // If not -- insert

                    /// MAYBE NOT...,
                    // Based on the table indicated by the user, instantiate the appropriate VM.
                    switch (viewModel.SysTableName)
                    {
                        case "taxonomy_species":
                            SpeciesViewModel speciesViewModel = new SpeciesViewModel();

                            // REFACTOR once logic makes more sense (CBH, 2/2/24)

                            foreach (DataRow dr in viewModel.DataCollectionDataTable.Rows)
                            {
                                string SQL = "UPDATE " + viewModel.SysTableName + " SET ";

                                foreach (DataColumn rowCol in viewModel.DataCollectionDataTable.Columns)
                                {
                                    sysTableField = viewModel.GetColumnInfo(viewModel.SysTableName, rowCol.ColumnName);

                                    // When we find pri. key, instantiate new Species object to use in update.
                                    if (sysTableField.FieldPurpose == "PRIMARY_KEY")
                                    {
                                        primaryKeyValue = Int32.Parse(dr[rowCol.ColumnName].ToString());
                                        speciesViewModel.Entity.ID = primaryKeyValue;
                                    }
                                    else
                                    {
                                        switch(rowCol.ColumnName)
                                        {
                                            case "protologue":
                                                speciesViewModel.Entity.Protologue = dr[rowCol.ColumnName].ToString();
                                                break;
                                        }

                                    }
                                }
                            }




                            break;
                        case "citation":
                            CitationViewModel citationViewModel = new CitationViewModel();
                            break;
                        case "taxonomy_cwr_crop":
                            CropForCWRViewModel cropForCWRViewModel = new CropForCWRViewModel();
                            break;
                        case "taxonomy_cwr_map":
                            CWRMapViewModel cWRMapViewModel = new CWRMapViewModel();
                            break;
                        case "taxonomy_cwr_trait":
                            CWRTraitViewModel cWRTraitViewModel = new CWRTraitViewModel();
                            break;
                    }

                    // ALTERNATIVE: Generate SQL per table. May not be needed -- import generally restricted to a
                    //              select subset of taxon tables.
                    //
                    // For each imported row, generate an INSERT or UPDATE statement based on the existence
                    // of a value for the PK of the table.
                    // foreach (DataRow dr in viewModel.DataCollectionDataTable.Rows)
                    // {
                    //    string SQL = "UPDATE " + viewModel.SysTableName + " SET ";

                    //    foreach (DataColumn rowCol in viewModel.DataCollectionDataTable.Columns)
                    //    {
                    //        sysTableField = viewModel.GetColumnInfo(viewModel.SysTableName, rowCol.ColumnName);

                    //        if (sysTableField.FieldPurpose == "PRIMARY_KEY")
                    //        {
                    //            primaryKeyValue = Int32.Parse(dr[rowCol.ColumnName].ToString());
                    //        }
                    //        else
                    //        {
                    //            SQL += sysTableField.FieldName + "= '" + dr[rowCol.ColumnName] + "'";
                    //        }
                    //    }
                    //}
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
