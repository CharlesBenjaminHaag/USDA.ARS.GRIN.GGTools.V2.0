using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class EconomicUse: AppEntityBase
    {
        public int SpeciesID { get; set; }
        public string SpeciesName { get; set; }
        public string Name { get; set; }
        public string ExtendedName { get; set; }
        public string EconomicUsageCode { get; set; }
        public string EconomicUsageDescription { get; set; }
        public string EconomicUsageType { get; set; }
        public string PlantPartCode { get; set; }
        public string PlantPartDescription { get; set; }
        public int CitationID { get; set; }
    }
}
