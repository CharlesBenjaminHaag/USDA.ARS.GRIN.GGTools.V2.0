using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class CitationSearch: SearchEntityBase
    {
        public int FamilyID { get; set; }
        public string FamilyIDList { get; set; }
        public string TaxonName { get; set; }
        public string FamilyName { get; set; }
        public int GenusID { get; set; }
        public string GenusIDList { get; set; }
        public string GenusName { get; set; }
        public int SpeciesID { get; set; }
        public string SpeciesIDList { get; set; }
        public string SpeciesName { get; set; }
        public string CitationText { get; set; }
        public string CitationTitle { get; set; }
        public string CitationType { get; set; }
        public string URL { get; set; }
        public int LiteratureID { get; set; }
        public string LiteratureTypeCode { get; set; }
        public string ReferenceTitle { get; set; }
        public string Abbreviation { get; set; }
        public string StandardAbbreviation { get; set; }
        public string LiteratureAuthorName { get; set; }

        public string CitationAuthorName { get; set; }
        public string EditorAuthorName { get; set; }
        public string PublicationYear { get; set; }
        public string CitationYear { get; set; }
        public string VolumeOrPage { get; set; }
        public string DOIReference { get; set; }
        public string IsAcceptedName { get; set; }
    }
}
