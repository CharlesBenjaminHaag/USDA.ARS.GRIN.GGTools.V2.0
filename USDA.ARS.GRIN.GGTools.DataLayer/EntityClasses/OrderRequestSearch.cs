using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class OrderRequestSearch : SearchEntityBase
    {
        public int OriginalID { get; set; }
        public int WebOrderRequestID { get; set; }
        public string LocalNumber { get; set; }
        public string MostRecentOrderActionCode { get; set; }
        public string OrderTypeCode { get; set; }
        public string OrderTypeDescription { get; set; }
        public int OrderRequestItemCount { get; set; }
        public string OrderRequestItemCountOperator { get; set; }
        public string IntendedUseCode { get; set; }
        public DateTime OrderedDate { get; set; }
        public int OrderedYear { get; set; }
        public string OrderedDateTimeFrame { get; set; }
        public DateTime CompletedDate { get; set; }
        public int RequestorCooperatorID { get; set; }
        public string RequestorCooperatorName { get; set; }
        public string RequestorOrganizationName { get; set; }
        public string RequestorEmailAddress { get; set; }
        public int ShipToCooperatorID { get; set; }
        public string ShipToCooperatorName { get; set; }
        public int FinalRecipientCooperatorID { get; set; }
        public string FinalRecipientCooperatorName { get; set; }
        public string OrderObtainedVia { get; set; }
        public string SpecialInstruction { get; set; }
        public int SiteID { get; set; }
    }
}
