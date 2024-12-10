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
using System.Linq.Expressions;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    

    [GrinGlobalAuthentication]
    public class ISTAController : BaseController
    {
        protected static string BASE_PATH = "~/Views/Taxonomy/ISTA/";
        
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
       
        public ActionResult Index()
        {
            try
            {
                ISTASeedViewModel viewModel = new ISTASeedViewModel();
                viewModel.AuthenticatedUserCooperatorID = AuthenticatedUser.CooperatorID;
                viewModel.TableName = "taxonomy_ista_seed";
                viewModel.Search();                
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

                        // Create a table row for each accepted name.
                        // IF NOT ACCEPTED: show formatted accepted name in a second row under the accepted one.
                        foreach (var istaSeed in group)
                        {
                            var tableRow = new TableRow();
                            var speciesNameCell = new TableCell();
                            var upovCodeCell = new TableCell();

                            string speciesNameHyperlinkUrl = "https://npgsweb.ars-grin.gov/gringlobal/taxon/taxonomydetail?id=" + istaSeed.AcceptedID;
                            string upovCodeHyperlinkUrl = "https://www.upov.int/genie/details.xhtml?cropId=" + istaSeed.UPOVCodeID;

                            // Name/authority text
                            speciesNameCell = CreateNameHyperlinkCell(istaSeed, speciesNameHyperlinkUrl, mainPart);
                            tableRow.Append(speciesNameCell);

                            // Family name
                            var column2Cell = new TableCell();
                            var familyNameParagraph = new Paragraph();
                            var familyNameRun = new Run();
                            var familyNameRunProperties = new RunProperties();
                            familyNameRunProperties.Bold = new Bold();
                            familyNameRun.Append(familyNameRunProperties);
                            familyNameRun.Append(new Text("(" + istaSeed.FamilyName + ")"));

                            if (!String.IsNullOrEmpty(istaSeed.FamilyAlternateName))
                            {
                                Break lineBreak = new Break();
                                familyNameRun.Append(lineBreak);
                                familyNameRun.Append(new Text("[" + istaSeed.FamilyAlternateName + "]"));
                            }

                            familyNameParagraph.Append(familyNameRun);
                           
                            //var boldParagraph = new Paragraph(
                            //    new Run(
                            //new RunProperties(new Bold()),
                            //        new Text("(" + istaSeed.FamilyName + ")") 
                            //    )
                            //);
                            column2Cell.Append(familyNameParagraph);
                            tableRow.Append(column2Cell);

                            // TODO UPOV Code
                            if (!String.IsNullOrEmpty(istaSeed.UPOVCode))
                            {
                                upovCodeCell = CreateUPOVCodeHyperlinkCell(istaSeed.UPOVCode, upovCodeHyperlinkUrl, mainPart);
                            }
                            else
                            {
                                Paragraph upovCodeParagraph = new Paragraph();
                                Run upovCodeRun = new Run();
                                Text upovCodeText = new Text(istaSeed.UPOVCode);

                                var boldParagraph3 = new Paragraph(
                                    new Run(
                                new RunProperties(),
                                        new Text("")
                                    )
                                );

                                upovCodeRun.Append(upovCodeText);
                                upovCodeParagraph.Append(upovCodeRun);
                                upovCodeCell.Append(upovCodeParagraph);
                            }

                            tableRow.Append(upovCodeCell);
                            
                            // UPOV
                            //var column3Cell = new TableCell();
                            //Paragraph upovCodeParagraph = new Paragraph();
                            //Run upovCodeRun = new Run();
                            //Text upovCodeText = new Text(istaSeed.UPOVCode);

                            //var boldParagraph3 = new Paragraph(
                            //    new Run(
                            //new RunProperties(),
                            //        new Text(istaSeed.UPOVCode)
                            //    )
                            //);

                            //upovCodeRun.Append(upovCodeText);
                            //upovCodeParagraph.Append(upovCodeRun);   
                            //column3Cell.Append(upovCodeParagraph);
                            //tableRow.Append(column3Cell);

                            table.AppendChild(tableRow);

                            if (istaSeed.NameStatus == "synonym")
                            {
                                var tableRowAcceptedName = new TableRow();
                                var acceptedNameCell = new TableCell();

                                var acceptedNameParagraph = new Paragraph();
                                var acceptedNameRun = new Run();

                                ParagraphProperties acceptedNameParagraphProperties = new ParagraphProperties();
                                Indentation indentation = new Indentation()
                                {
                                    Left = "720" // Indentation in twentieths of a point (720 = 0.5 inches)
                                };
                                acceptedNameParagraphProperties.Append(indentation);
                                acceptedNameParagraph.Append(acceptedNameParagraphProperties);

                                acceptedNameRun.Append(new Text("        =" + istaSeed.AcceptedName));
                                acceptedNameParagraph.Append(acceptedNameRun);
                                acceptedNameCell.Append(acceptedNameParagraph);

                                tableRowAcceptedName.Append(acceptedNameCell);
                                table.AppendChild(tableRowAcceptedName);
                            }
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
        private TableCell CreateNameHyperlinkCell(ISTASeed iSTASeed, string url, MainDocumentPart mainPart)
        {
            TableCell tableCell = new TableCell();
            string hyperlinkId = "rId" + (mainPart.HyperlinkRelationships.Count() + 1);

            //Add a hyperlink relationship
            mainPart.AddHyperlinkRelationship(new Uri(url), true, hyperlinkId);

            // Create a paragraph to hold the hyperlink
            Paragraph paragraph = new Paragraph();

            // Create a new hyperlink
            Hyperlink hyperlink = new Hyperlink();

            //string stringPart1 = ExtractItalicizedText(iSTASeed.AcceptedName);
            string namePart = iSTASeed.GenusName + " " + iSTASeed.SpeciesName;

            //TODO
            // Create run for each of the following
            // Genus name + species name
            // BASED ON RANK:
            //  Species: species, auth
            //  Subspecies: subsp, subsp auth
            //  Variety: var, var auth

            Run nameRun = new Run();
            RunProperties nameRunProperties = new RunProperties();
            nameRunProperties.Italic = new Italic();  // Make this text italic
            nameRunProperties.Underline = new Underline { Val = UnderlineValues.Single };
            nameRunProperties.Color = new Color { Val = "0000FF" };
            
            if (iSTASeed.NameStatus == "accepted")
            {
                nameRunProperties.Bold = new Bold();
            }
            
            nameRun.Append(nameRunProperties);

            string nameRunText = iSTASeed.GenusName + " " + iSTASeed.SpeciesName;

            //switch (iSTASeed.Rank)
            //{
            //    case "SPECIES":
            //        nameRunText += " " + iSTASeed.SpeciesAuthority;
            //        break;
            //    case "SUBSPECIES":
            //        nameRunText += " subsp." + iSTASeed.SubspeciesName + " " + iSTASeed.SubspeciesAuthority;
            //        break;
            //    case "VARIETY":
            //        nameRunText += " var." + iSTASeed.VarietyName + " " + iSTASeed.VarietyAuthority;
            //        break;
            //    default:
            //        nameRunText += " " + iSTASeed.SpeciesAuthority;
            //        break;
            //}

            nameRun.Append(new Text(nameRunText));

            // Infra indicator
            Run infraspecificNameRun = new Run();
            RunProperties infraspecificNameRunProperties = new RunProperties();
            infraspecificNameRunProperties.Underline = new Underline { Val = UnderlineValues.Single };
            infraspecificNameRunProperties.Color = new Color { Val = "0000FF" };

            if (iSTASeed.NameStatus == "accepted")
            {
                infraspecificNameRunProperties.Bold = new Bold();
            }
            
            infraspecificNameRun.Append(infraspecificNameRunProperties);
            string infraspecificNameText = String.Empty;

            if (iSTASeed.Rank == "SUBSPECIES")
            {
                infraspecificNameText = " subsp.";
            }
            else
            {
                if (iSTASeed.Rank == "VARIETY")
                {
                    infraspecificNameText = " var. ";
                }
            }

            Text textInfraspecificName = new Text(infraspecificNameText)
            {
                Space = SpaceProcessingModeValues.Preserve
            };

            infraspecificNameRun.Append(textInfraspecificName);

            // Infra name
            Run infraspecificNameRun2 = new Run();
            RunProperties infraspecificNameRunProperties2 = new RunProperties();
            infraspecificNameRunProperties2.Underline = new Underline { Val = UnderlineValues.Single };
            infraspecificNameRunProperties2.Color = new Color { Val = "0000FF" };

            if (iSTASeed.NameStatus == "accepted")
            {
                infraspecificNameRunProperties2.Bold = new Bold();
            }
            
            infraspecificNameRunProperties2.Italic = new Italic();
            infraspecificNameRun2.Append(infraspecificNameRunProperties2);

            string infraspecificNameText2 = String.Empty; 
            

            if (iSTASeed.Rank == "SUBSPECIES")
            {
                infraspecificNameText2 = " " + iSTASeed.SubspeciesName + " ";
            }
            else
            {
                if (iSTASeed.Rank == "VARIETY")
                {
                    infraspecificNameText2 = " " + iSTASeed.VarietyName + " ";
                }
            }

            Text textInfraspecificName2 = new Text(infraspecificNameText2)
            {
                Space = SpaceProcessingModeValues.Preserve
            };

            infraspecificNameRun2.Append(new Text(infraspecificNameText2));

            Run authorityRun = new Run();
            RunProperties authorityRunProperties = new RunProperties();
            authorityRunProperties.Underline = new Underline { Val = UnderlineValues.Single };
            authorityRunProperties.Color = new Color { Val = "0000FF" };

            if (iSTASeed.NameStatus == "accepted")
            {
                authorityRunProperties.Bold = new Bold();
            }
            
            authorityRun.Append(authorityRunProperties);

            string authorityText = String.Empty;

            switch (iSTASeed.Rank)
            {
                case "SPECIES":
                    authorityText = " " + iSTASeed.SpeciesAuthority + " "; break;
                case "SUBSPECIES":
                    authorityText = " " + iSTASeed.SubspeciesAuthority + " "; break;
                case "VARIETY":
                    authorityText = " " + iSTASeed.VarietyAuthority + " "; break;
                default:
                    authorityText = " " + iSTASeed.SpeciesAuthority + " "; break;
            }

            Text textAuthority = new Text(authorityText)
            {
                Space = SpaceProcessingModeValues.Preserve
            };

            authorityRun.Append(textAuthority);

            // Add the first part of the hyperlink text (italicized)
            //Run italicRun = new Run();
            //RunProperties italicRunProperties = new RunProperties();
            //italicRunProperties.Italic = new Italic();  // Make this text italic
            //italicRunProperties.Underline = new Underline { Val = UnderlineValues.Single };
            //italicRunProperties.Color = new Color { Val = "0000FF" };
            //italicRunProperties.Bold = new Bold();
            //italicRun.Append(italicRunProperties);
            //italicRun.Append(new Text(namePart));

            // Add the second part of the hyperlink text (non-italicized)
            //Run normalRun = new Run();
            //normalRun.Append(new Text(" and this part is normal"));

            // Add both runs to the hyperlink
            hyperlink.Append(nameRun);
            hyperlink.Append(infraspecificNameRun);
            hyperlink.Append(infraspecificNameRun2);
            hyperlink.Append(authorityRun);

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

        private TableCell CreateAcceptedNameHyperlinkCell(ISTASeed iSTASeed, string url, MainDocumentPart mainPart)
        {
            TableCell tableCell = new TableCell();
            string hyperlinkId = "r2Id" + (mainPart.HyperlinkRelationships.Count() + 1);

            //Add a hyperlink relationship
            mainPart.AddHyperlinkRelationship(new Uri(url), true, hyperlinkId);

            // Create a paragraph to hold the hyperlink
            Paragraph paragraph = new Paragraph();

            // Create a new hyperlink
            Hyperlink hyperlink = new Hyperlink();

            string namePart = ExtractItalicizedText(iSTASeed.AcceptedName);

            //TODO
            // Create run for each of the following
            // Genus name + species name
            // BASED ON RANK:
            //  Species: species, auth
            //  Subspecies: subsp, subsp auth
            //  Variety: var, var auth

            Run nameRun = new Run();
            RunProperties nameRunProperties = new RunProperties();
            nameRunProperties.Italic = new Italic();  // Make this text italic
            nameRunProperties.Underline = new Underline { Val = UnderlineValues.Single };
            nameRunProperties.Color = new Color { Val = "0000FF" };

            if (iSTASeed.NameStatus == "accepted")
            {
                nameRunProperties.Bold = new Bold();
            }

            nameRun.Append(nameRunProperties);

            string nameRunText = namePart;

            nameRun.Append(new Text(nameRunText));

            // Add both runs to the hyperlink
            hyperlink.Append(nameRun);
          
            hyperlink.Id = hyperlinkId;

            // Add the hyperlink to the paragraph
            paragraph.Append(hyperlink);

            tableCell.Append(paragraph);

            return tableCell;
        }

        private TableCell CreateUPOVCodeHyperlinkCell(string upovCode, string url, MainDocumentPart mainPart)
        {
            TableCell tableCell = new TableCell();
            string hyperlinkId = "rIdUPOV" + (mainPart.HyperlinkRelationships.Count() + 1);

            //Add a hyperlink relationship
            mainPart.AddHyperlinkRelationship(new Uri(url), true, hyperlinkId);

            // Create a paragraph to hold the hyperlink
            Paragraph paragraph = new Paragraph();

            // Create a new hyperlink
            Hyperlink hyperlink = new Hyperlink();

            Run nameRun = new Run();
            RunProperties nameRunProperties = new RunProperties();
            nameRunProperties.Underline = new Underline { Val = UnderlineValues.Single };
            nameRunProperties.Color = new Color { Val = "0000FF" };
            nameRun.Append(nameRunProperties);
                        
            string nameRunText = upovCode;

            nameRun.Append(new Text(nameRunText));
            hyperlink.Append(nameRun);
            hyperlink.Id = hyperlinkId;
            paragraph.Append(hyperlink);
            tableCell.Append(paragraph);
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
