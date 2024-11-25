using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GGTools.DataLayer.UPOV
{
    public class UPOVEncodedSpecies
    {
        public int ID{ get; set; }
        public int UPOVCodeID { get; set; }
        public string UPOVCode { get; set; }
        public string UPOVCodeURL { get; set; }
        public string UPOVPrincipalSpeciesName { get; set; }
        public string USDASpeciesName { get; set; }
        public string USDASpeciesURL { get; set; }
        public string UPOVOtherSpeciesName { get; set; }
        public string CommonNameText { get; set; }
        public string Note { get; set; }
    }
}
