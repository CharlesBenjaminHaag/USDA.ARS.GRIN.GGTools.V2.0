using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class AppUserItemFolder : AppEntityBase
    {
        public string FolderName { get; set; }
        public string FolderType { get; set; }
        public string FolderTypeDescription { get; set; }
        public string Description { get; set; }
        public int TotalItems { get; set; }
        public string Category { get; set; }
        public string AlternateCategory { get; set; }
        public bool IsFavorite { get; set; }
        public string IsShared { get; set; }
    }
}