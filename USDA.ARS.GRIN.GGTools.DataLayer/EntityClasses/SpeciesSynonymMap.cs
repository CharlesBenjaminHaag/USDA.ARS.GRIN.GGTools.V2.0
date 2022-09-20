using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class SpeciesSynonymMap : AppEntityBase
    {
        public int SpeciesIDSubject { get; set; }
        public string SpeciesAssembledNameSubject { get; set; }
        public int SpeciesIDPredicate { get; set; }
        public string SpeciesAssembledNamePredicate { get; set; }
        public string SynonymCode { get; set; }
        public string SynonymDescription { get; set; }
    }
}
