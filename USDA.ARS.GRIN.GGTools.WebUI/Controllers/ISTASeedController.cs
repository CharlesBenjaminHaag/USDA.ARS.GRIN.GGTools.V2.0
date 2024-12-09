using System.Web.Mvc;
using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.DataLayer.UPOV;
using NLog;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls.WebParts;
using System.Security.Policy;
using System.EnterpriseServices;
using DocumentFormat.OpenXml.Vml.Spreadsheet;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    

    [GrinGlobalAuthentication]
    public class ISTASeedController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/ISTASeed/";
        
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            ISTASeedViewModel viewModel = new ISTASeedViewModel();
            try
            {
                //TODO
                return PartialView(BASE_PATH + "_ListFolder.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
       
        public ActionResult Index(string eventAction = "", int folderId = 0, string sysTableName = "", string sysTableTitle = "")
        {
            try
            {
                ISTASeedViewModel viewModel = new ISTASeedViewModel();
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                
                viewModel.TableName = "taxonomy_ISTASeed";

                //SetPageTitle();

                string targetKey = this.ControllerContext.RouteData.Values["controller"].ToString().ToUpper() + "_SEARCH";
                if (Session[targetKey] != null)
                {
                    viewModel = Session[targetKey] as ISTASeedViewModel;
                    viewModel.Search();
                }

                if (eventAction == "RUN_SEARCH")
                {
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    appUserItemListViewModel.SearchEntity.AppUserItemFolderID = folderId;
                    appUserItemListViewModel.Search();
                    viewModel.SearchEntity = viewModel.Deserialize<ISTASeedSearch>(appUserItemListViewModel.Entity.Properties);
                    viewModel.Search();
                }
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        public ActionResult Export()
        {
            ISTASeedViewModel viewModel = new ISTASeedViewModel();

            // Create a MemoryStream to hold the document
            using (MemoryStream stream = new MemoryStream())
            {
                // Create a Word document
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, true))
                {
                    // Add a main document part
                    MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = new Body();
                    mainPart.Document.Append(body);

                    viewModel.Search();

                    // Group data by the first letter of the first column
                    var groupedData = viewModel.DataCollection.GroupBy(d => d.DisplayLetter)
                                          .OrderBy(g => g.Key);


                    foreach (var group in groupedData)
                    {
                        var heading = new Paragraph(new Run(new Text($"{group.Key}")))
                        {
                            ParagraphProperties = new ParagraphProperties
                            {
                                Justification = new Justification { Val = JustificationValues.Left },
                                SpacingBetweenLines = new SpacingBetweenLines { Before = "200", After = "200" },
                            }
                        };
                        heading.ParagraphProperties.Append(new RunProperties
                        {
                            RunFonts = new RunFonts { Ascii = "Arial" },
                            Bold = new Bold(),
                            FontSize = new FontSize { Val = "28" } // 14 pt (2x)
                        });
                        body.AppendChild(heading);

                        var table = new Table();

                        TableProperties tableProperties = new TableProperties(
                            new TableBorders(
                                new TopBorder { Val = BorderValues.None, Size = 6 },
                                new BottomBorder { Val = BorderValues.None, Size = 6 },
                                new LeftBorder { Val = BorderValues.None, Size = 6 },
                                new RightBorder { Val = BorderValues.None, Size = 6 },
                                new InsideHorizontalBorder { Val = BorderValues.None, Size = 6 },
                                new InsideVerticalBorder { Val = BorderValues.None, Size = 6 }
                            ),
                            new TableWidth { Width = "100%", Type = TableWidthUnitValues.Pct }
                        );
                        table.AppendChild(tableProperties);

                        foreach (var istaSeed in group)
                        {
                            var tableRow = new TableRow();

                            var column1Cell = new TableCell();

                            string hyperLinkUrl = "https://npgsweb.ars-grin.gov/gringlobal/taxon/taxonomydetail?id=" + istaSeed.AcceptedID;

                            column1Cell = CreateHyperlinkCell(istaSeed.DisplayName, hyperLinkUrl, mainPart);
                            tableRow.Append(column1Cell);

                            var column2Cell = new TableCell();
                            var boldParagraph = new Paragraph(
                                new Run(
                            new RunProperties(new Bold()),
                                    new Text(istaSeed.FamilyName)
                                )
                            );
                            column2Cell.Append(boldParagraph);
                            tableRow.Append(column2Cell);

                            table.AppendChild(tableRow);
                        }

                        body.Append(table);
                    }
                    
                    
                    // Add data rows
                    
                }

                // Return the Word document as a file download
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "ISTAReport_Draft.docx");
            }
        }




        /// <summary>
        /// NEWEST
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="appUserItemFolderId"></param>
        /// <returns></returns>
        private TableCell CreateHyperlinkCell(ISTASeed iSTASeed, string url, MainDocumentPart mainPart)
        {
            TableCell tableCell = new TableCell();
            string hyperlinkId = "rId" + (mainPart.HyperlinkRelationships.Count() + 1);

            //Add a hyperlink relationship
            mainPart.AddHyperlinkRelationship(new Uri(url), true, hyperlinkId);

            // Create a paragraph to hold the hyperlink
            Paragraph paragraph = new Paragraph();

            // Create a new hyperlink
            Hyperlink hyperlink = new Hyperlink();

            //string stringPart1 = ExtractItalicizedText(displayText);


            //TODO
            // Create run for each of the following
            // Genus name + species name
            // BASED ON RANK:
            //  Species: species, auth
            //  Subspecies: subsp, subsp auth
            //  Variety: var, var auth

            Run nameRun = new Run();
            RunProperties nameRunProperties = new RunProperties();

            Run speciesAuthorityRun = new Run();
            RunProperties speciesAuthorityRunProperties = new RunProperties();

            // Add the first part of the hyperlink text (italicized)
            Run italicRun = new Run();
            RunProperties italicRunProperties = new RunProperties();
            italicRunProperties.Italic = new Italic();  // Make this text italic
            italicRunProperties.Underline = new Underline { Val = UnderlineValues.Single };
            italicRunProperties.Color = new Color { Val = "0000FF" };
            italicRunProperties.Bold = new Bold();
            italicRun.Append(italicRunProperties);
            italicRun.Append(new Text(stringPart1));

            // Add the second part of the hyperlink text (non-italicized)
            Run normalRun = new Run();
            normalRun.Append(new Text(" and this part is normal"));

            // Add both runs to the hyperlink
            hyperlink.Append(italicRun);
            hyperlink.Append(normalRun);
            hyperlink.Id = hyperlinkId;

            // Add the hyperlink to the paragraph
            paragraph.Append(hyperlink);

            tableCell.Append(paragraph);

            //Create the hyperlink cell
            //return new TableCell(
            //    new Paragraph(
            //        new Hyperlink(
            //            new Run(
            //                new RunProperties(
            //                    new Underline { Val = UnderlineValues.Single },
            //                    new Color { Val = "0000FF" },
            //                    new Italic { }
            //                ),
            //                new Text(displayText)
            //            )
            //        )
            //        {
            //            Id = hyperlinkId
            //        }
            //    )
            //);
            return tableCell;
        }

        public static string ExtractItalicizedText(string input)
        {
            // Regular expression to match text between <i> and </i> tags
            Regex regex = new Regex(@"<i>(.*?)</i>", RegexOptions.IgnoreCase);

            // Find all matches in the input string
            MatchCollection matches = regex.Matches(input);

            // Initialize a variable to hold the result
            string result = string.Empty;

            // Iterate through each match and add the matched text to the result
            foreach (Match match in matches)
            {
                result += match.Groups[1].Value + " ";  // Group 1 contains the text inside <i>...</i>
            }

            // Trim any extra spaces at the end
            return result.Trim();
        }

        public ActionResult Edit(int entityId, int appUserItemFolderId = 0)
        {
            try
            {
                ISTASeedViewModel viewModel = new ISTASeedViewModel();
                viewModel.TableName = "taxonomy_ISTASeed";
                viewModel.TableCode = "ISTASeed";
                viewModel.EventValue = "Edit";
                viewModel.AppUserItemFolderID = appUserItemFolderId;

               
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        
        [HttpPost]
        public ActionResult Edit(ISTASeedViewModel viewModel)
        {
            try
            {
                if (!viewModel.Validate())
                {
                    if (viewModel.ValidationMessages.Count > 0) return View(BASE_PATH + "Edit.cshtml", viewModel);
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

               

                return RedirectToAction("Edit", "ISTASeed", new { entityId = viewModel.Entity.ID });
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        
        public ActionResult Delete(int id)
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Search(ISTASeedViewModel viewModel)
        {
            try
            {
                Session[SessionKeyName] = viewModel;
                viewModel.Search();
                ModelState.Clear();

                // Save search if attribs supplied.
                if ((viewModel.EventAction == "SEARCH") && (viewModel.EventValue == "SAVE"))
                {
                    SysFolderViewModel sysFolderViewModel = new SysFolderViewModel();
                    sysFolderViewModel.Entity.CreatedByCooperatorID = AuthenticatedUser.CooperatorID;
                    sysFolderViewModel.Entity.Title = viewModel.EventInfo;
                    sysFolderViewModel.Entity.Description = viewModel.EventNote;
                    sysFolderViewModel.Entity.TableName = viewModel.TableName;
                    sysFolderViewModel.Entity.Properties = viewModel.SerializeToXml<ISTASeedSearch>(viewModel.SearchEntity);
                    sysFolderViewModel.Entity.TypeCode = "DYN";
                    sysFolderViewModel.Insert();
                }
                viewModel.TableName = "taxonomy_ISTASeed";
                return View(BASE_PATH + "Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        
        public ActionResult Add()
        {
            try
            {
                ISTASeedViewModel viewModel = new ISTASeedViewModel();
                viewModel.TableName = "taxonomy_ISTASeed";
                return View(BASE_PATH + "Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }
        
        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public JsonResult DeleteEntity(FormCollection formCollection)
        {
            try
            {
                ISTASeedViewModel viewModel = new ISTASeedViewModel();
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
    }
}
