using System.Web.Mvc;
using System;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.WebUI;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using NLog;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using USDA.ARS.GRIN.GGTools.DataLayer.UPOV;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System.IO;
using System.Linq;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.WebUI.Controllers
{
    public class ApiResponse
    {
        public List<upovCodeItem> upovCodeList { get; set; }
    }

    [GrinGlobalAuthentication]
    public class UPOVController : BaseController
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly HttpClient _httpClient;

        public UPOVController()
        {
            _httpClient = new HttpClient();
        }

        public ActionResult Index()
        {
            UPOVViewModel model = new UPOVViewModel();
            model.Search();
            return View(model);
        }

        public async Task<ActionResult> Import()
        {
            string apiUrl = "https://www.upov.int/geniews/upovcode/UpovCodeList"; // Replace with the actual API URL
            UPOVViewModel viewModel = new UPOVViewModel();

            try
            {
                var response = await _httpClient.GetStringAsync(apiUrl);
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);
                viewModel.DataCollectionImport = new System.Collections.ObjectModel.Collection<upovCodeItem>(apiResponse.upovCodeList);

                foreach (var result in viewModel.DataCollectionImport)
                {
                    viewModel.Entity = result;
                    viewModel.Insert();
                }

                // Pass data to the view
                return View("~/Views/UPOV/Import.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., logging, showing error messages)
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        public ActionResult ExportToWord()
        {
            // Sample data for the table
            var data = new[]
            {
            new { Name = "Google", Url = "https://www.google.com" },
            new { Name = "Stack Overflow", Url = "https://stackoverflow.com" },
            new { Name = "GitHub", Url = "https://github.com" }
        };

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

                    // Add a table
                    Table table = new Table();

                    // Add table properties (optional styling)
                    TableProperties tableProperties = new TableProperties(
                        new TableBorders(
                            new TopBorder { Val = BorderValues.Single, Size = 6 },
                            new BottomBorder { Val = BorderValues.Single, Size = 6 },
                            new LeftBorder { Val = BorderValues.Single, Size = 6 },
                            new RightBorder { Val = BorderValues.Single, Size = 6 },
                            new InsideHorizontalBorder { Val = BorderValues.Single, Size = 6 },
                            new InsideVerticalBorder { Val = BorderValues.Single, Size = 6 }
                        )
                    );
                    table.AppendChild(tableProperties);

                    // Add a header row
                    TableRow headerRow = new TableRow();
                    headerRow.Append(
                        CreateTableCell("Name"),
                        CreateTableCell("Link")
                    );
                    table.Append(headerRow);

                    // Add data rows
                    foreach (var item in data)
                    {
                        TableRow dataRow = new TableRow();
                        dataRow.Append(
                            CreateTableCell(item.Name),
                            CreateHyperlinkCell(item.Url, mainPart)
                        );
                        table.Append(dataRow);
                    }

                    body.Append(table);
                }

                // Return the Word document as a file download
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "TableWithLinks.docx");
            }
        }

        private TableCell CreateTableCell(string text)
        {
            return new TableCell(
                new Paragraph(
                    new Run(
                        new Text(text)
                    )
                )
            );
        }

        private TableCell CreateHyperlinkCell(string url, MainDocumentPart mainPart)
        {
            string hyperlinkId = "rId" + (mainPart.HyperlinkRelationships.Count() + 1);

            // Add a hyperlink relationship
            mainPart.AddHyperlinkRelationship(new Uri(url), true, hyperlinkId);

            // Create the hyperlink cell
            return new TableCell(
                new Paragraph(
                    new Hyperlink(
                        new Run(
                            new RunProperties(
                                new Underline { Val = UnderlineValues.Single },
                                new Color { Val = "0000FF" }
                            ),
                            new Text(url)
                        )
                    )
                    {
                        Id = hyperlinkId
                    }
                )
            );
        }
    }


}

