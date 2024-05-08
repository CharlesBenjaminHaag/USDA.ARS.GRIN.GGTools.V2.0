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
            DataTable sourceTable = new DataTable();
            DataTable destinationTable = new DataTable();
            SysTableViewModel sysTableViewModel = new SysTableViewModel();
            SysTableField sysTableField = new SysTableField();
            SpeciesImport speciesImport = new SpeciesImport();
            
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
                    sourceTable = result.Tables[0];

                    // May need to refactor: create new DT consisting of a subset of those imported from
                    // the spreadsheet, and comparison columns to indicate the results of name searches.

                    // Create source DataTable
                    
                    //sourceTable.Columns.Add("ID", typeof(int));
                    //sourceTable.Columns.Add("Name", typeof(string));
                    //sourceTable.Columns.Add("Age", typeof(int));
                    //// Add some sample data
                    //sourceTable.Rows.Add(1, "John", 30);
                    //sourceTable.Rows.Add(2, "Alice", 25);

                    // Define the subset of columns to copy
                    string[] columnsToCopy = { "ID", "TAXONOMY_GENUS", "TAXONOMY_SPECIES", "AUTHOR" };
                    
                    // Add columns to the destination DataTable based on the subset
                    foreach (string column in columnsToCopy)
                    {
                        destinationTable.Columns.Add(column, sourceTable.Columns[column].DataType);

                        //if (column == "TAXONOMY_GENUS")
                        //{
                        //    DataColumn taxonomyGenusResultColumn = new DataColumn();
                        //    taxonomyGenusResultColumn.ColumnName = "GENUS_MATCH";
                        //    destinationTable.Columns.Add(taxonomyGenusResultColumn.ColumnName, sourceTable.Columns[column].DataType);
                        //}

                      
                    }

                    DataColumn taxonomySpeciesResultColumn = new DataColumn();
                    taxonomySpeciesResultColumn.ColumnName = "MATCH_GENUS";
                    destinationTable.Columns.Add(taxonomySpeciesResultColumn.ColumnName, sourceTable.Columns["TAXONOMY_GENUS"].DataType);

                    DataColumn taxonomySpeciesResultColumn2 = new DataColumn();
                    taxonomySpeciesResultColumn.ColumnName = "MATCH_SPECIES";
                    destinationTable.Columns.Add(taxonomySpeciesResultColumn.ColumnName, sourceTable.Columns["TAXONOMY_SPECIES"].DataType);

                    DataColumn taxonomySpeciesResultColumn3 = new DataColumn();
                    taxonomySpeciesResultColumn.ColumnName = "MATCH_AUTHOR";
                    destinationTable.Columns.Add(taxonomySpeciesResultColumn.ColumnName, sourceTable.Columns["AUTHOR"].DataType);

                    // Add columns to hold matching species record(s)

                    // Copy data from source to destination
                    foreach (DataRow sourceRow in sourceTable.Rows)
                    {
                        DataRow destRow = destinationTable.NewRow();
                        foreach (string column in columnsToCopy)
                        {
                            destRow[column] = sourceRow[column];

                            // If col is genus, species: search for match and display results in new adjacent column.
                            //if (column == "TAXONOMY_GENUS")
                            //{
                            //    string sourceGenusName = sourceRow[column].ToString();

                            //    if (!String.IsNullOrEmpty(sourceGenusName))
                            //    {
                            //        GenusViewModel genusViewModel = new GenusViewModel();
                            //        genusViewModel.SearchEntity.Name = sourceGenusName;
                            //        genusViewModel.Search();
                            //        destRow["GENUS_MATCH"] = genusViewModel.DataCollection.Count;
                            //    }
                            //}

                            if (column == "TAXONOMY_SPECIES")
                            {
                                string sourceGenusName = sourceRow["TAXONOMY_GENUS"].ToString();
                                string sourceSpeciesName = sourceRow[column].ToString();

                                if (!String.IsNullOrEmpty(sourceSpeciesName))
                                {
                                    SpeciesViewModel speciesViewModel = new SpeciesViewModel();
                                    speciesViewModel.SearchEntity.Name = sourceGenusName + " " + sourceSpeciesName;
                                    speciesViewModel.Search();

                                    if (speciesViewModel.DataCollection.Count > 0)
                                    {

                                        destRow["MATCH_GENUS"] = speciesViewModel.DataCollection[0].GenusName;
                                        destRow["MATCH_SPECIES"] = speciesViewModel.DataCollection[0].Name;
                                        destRow["MATCH_AUTHOR"] = speciesViewModel.DataCollection[0].SpeciesAuthority;
                                    }
                                }
                            }

                        }
                        destinationTable.Rows.Add(destRow);
                    }

                    viewModel.DataCollectionDataTable = destinationTable.Copy();

                    //switch (viewModel.SysTableName)
                    //{
                    //    case "Species":
                    //        SpeciesViewModel speciesViewModel = new SpeciesViewModel();

                    //        // REFACTOR once logic makes more sense (CBH, 2/2/24)

                    //        foreach (DataRow dr in dtImported.Rows)
                    //        {
                    //            //speciesImport = new SpeciesImport();

                    //            foreach (DataColumn rowCol in dtImported.Columns)
                    //            {
                    //                //sysTableField = viewModel.GetColumnInfo(viewModel.SysTableName, rowCol.ColumnName);

                    //                //switch (rowCol.ColumnName)
                    //                //{
                    //                //    case "ID":
                    //                //        primaryKeyValue = Int32.Parse(dr[rowCol.ColumnName].ToString());
                    //                //        speciesViewModel.Get(primaryKeyValue);
                    //                //        speciesImport.ID = speciesViewModel.Entity.ID;
                    //                //        break;
                    //                //    case "Name":
                    //                //    case "Epithet":
                    //                //        speciesImport.SpeciesName = dr[rowCol.ColumnName].ToString();
                    //                //        speciesImport.OriginalSpeciesName = speciesViewModel.Entity.SpeciesName;
                    //                //        break;
                    //                //    case "Authority":
                    //                //        speciesImport.SpeciesAuthority = dr[rowCol.ColumnName].ToString();
                    //                //        speciesImport.OriginalSpeciesAuthority = speciesViewModel.Entity.SpeciesAuthority;
                    //                //        break;
                    //                //    case "Protologue":
                    //                //        speciesImport.Protologue = dr[rowCol.ColumnName].ToString();
                    //                //        speciesImport.OriginalProtologue = speciesViewModel.Entity.Protologue;
                    //                //        break;

                    //                //}


                    //            }
                    //            //viewModel.DataCollectionSpeciesImport.Add(speciesImport);
                    //        }
                    //        break;
                    //    case "Citation":
                    //        CitationViewModel citationViewModel = new CitationViewModel();
                    //        break;
                    //    case "Literature":
                    //        CropForCWRViewModel cropForCWRViewModel = new CropForCWRViewModel();
                    //        break;
                    //    case "CWR Map":
                    //        CWRMapViewModel cWRMapViewModel = new CWRMapViewModel();
                    //        break;
                    //    case "CWR Trait":
                    //        CWRTraitViewModel cWRTraitViewModel = new CWRTraitViewModel();
                    //        break;
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
