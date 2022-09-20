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
    public class Citation : AppEntityBase
    {
        public int TaxonID { get; set; }
        public string TaxonName { get; set; }
        public string TaxonTitle { get; set; }
        public int FamilyID { get; set; }
        public  string  FamilyName { get; set; }
        public int GenusID { get; set; }
        public string GenusName { get; set; }
        public int SpeciesID { get; set; }
        public string SpeciesName { get; set; }
        public string CitationText { get; set; }
        public string URL { get; set; }
        public int LiteratureID { get; set; }
        [AllowHtml]
        public string CitationTitle { get; set; }
        public string ReferenceTitle { get; set; }
        public string DisplayTitle { get; set; }
        public string Abbreviation { get; set; }
        public string StandardAbbreviation { get; set; }
        public string CitationAuthorName { get; set; }
        public string DisplayAuthorName { get; set; }
        public string VolumeOrPage { get; set; }
        public string DOIReference { get; set; }
        public string IsAcceptedName { get; set; }
        public string EditorAuthorName { get; set; }
        public string TypeCode { get; set; }
        public string PublicationYear { get; set; }
        public int CitationYear { get; set; }
        public string VolumePage { get; set; }
        public string Description { get; set; }
        public string PublisherName { get; set; }
        [AllowHtml]
        public string Note { get; set; }
    }
}
