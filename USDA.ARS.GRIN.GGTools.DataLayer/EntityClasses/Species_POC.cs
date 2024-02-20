using System;
using System.Text;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class Species_POC
    {
        public int taxonomy_species_id { get; set; }
        public string name { get; set; }
        public string protologue { get; set; }
        public string name_authority { get; set; } 
    }
}
