using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysAppErrorSearch
    {
        public int ID { get; set; }
        public string ApplicationCode { get; set; }
        public DateTime CreateDate { get; set; }
        public string ErrorLevel { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public string Exception { get; set; }
        public string Logger { get; set; }
        public string Url { get; set; }
    }
}
