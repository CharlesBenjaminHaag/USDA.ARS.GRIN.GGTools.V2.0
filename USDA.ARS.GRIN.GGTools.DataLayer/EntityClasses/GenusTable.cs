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
    public class GenusTable
    {
        public int taxonomy_genus_id { get; set; }
        public string genus_name { get; set; }
    }
}
