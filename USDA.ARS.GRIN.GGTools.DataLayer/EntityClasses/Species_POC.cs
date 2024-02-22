using System;
using System.Text;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;
using System.Security;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class Species_POC
    {
        public int taxonomy_species_id { get; set; }
        public string name { get; set; }
        [AllowHtml]
        public string protologue { get; set; }
        public string name_authority { get; set; }
        public string note { get;set; }
    }
}
