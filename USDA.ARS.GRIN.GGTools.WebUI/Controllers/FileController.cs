using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class FileController : BaseController     
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        //private readonly string _uploadPath = Path.Combine(HttpRuntime.AppDomainAppPath, "uploads");
        private readonly string _uploadPath = @"C:\inetpub\wwwroot\gringlobal\documents";

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                ViewBag.Message = "No file provided.";
                return RedirectToAction("Index");
            }

            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);

            var safeName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_uploadPath, safeName);

            file.SaveAs(filePath);

            ViewBag.Message = safeName + " uploaded successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string fileName)
        {
            var safeName = Path.GetFileName(fileName);
            var filePath = Path.Combine(_uploadPath, safeName);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            var files = Directory.Exists(_uploadPath)
                ? Directory.GetFiles(_uploadPath)
                           .Select(Path.GetFileName)
                           .ToList()
                : new List<string>();

            return View(files);
        }
    }
}