using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class Accession : AppEntityBase
    {
        public string AccessionPrefix { get; set; }
        public int AccessionNumber { get; set; }
        public string AccessionSuffix { get; set; }
        public int SpeciesID { get; set; }
        public int NewSpeciesID { get; set; }
        public string StatusCode { get; set; }
        public string LifeFormCode { get; set; }
        public string ImprovementStatusCode { get; set; }
        public string ReproductiveUniformityCode { get; set; }
        public string InitialReceivedFormCode { get; set; }
        public string LifeCycleCode { get; set; }
        public string LifeHabitCode { get; set; }
        public string LifeSexCode { get; set; }
        public DateTime InitialReceivedDate { get; set; }
        public int InventoryCount { get; set; }
    }
}
