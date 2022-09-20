using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class SpeciesSynonymMapSearch : SearchEntityBase
    {
        public int SpeciesIDSubject { get; set; }
        public string SpeciesNameSubject { get; set; }
        public string SpeciesIDPredicate { get; set; }
        public string SpeciesNamePredicate { get; set; }
        public string SynonymCode { get; set; }
        public string SynonymDescription { get; set; }
        public string ItemIDList { get; set; }
    }
}
