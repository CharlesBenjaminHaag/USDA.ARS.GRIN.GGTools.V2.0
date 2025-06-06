﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;


namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class OrderRequest : AppEntityBase
    {
        public int OriginalID { get; set; }
        public int WebOrderRequestID { get; set; }
        public string LocalNumber { get; set; }
        public string OrderTypeCode { get; set; }
        public int OrderRequestItemCount { get; set; }
        public string OrderTypeDescription { get; set; }
        public string MostRecentOrderRequestActionCode { get; set; }
        public DateTime OrderedDate { get; set; }
        public string IntendedUseCode { get; set; }
        public string IntendedUseNote { get; set; }
        public DateTime CompletedDate { get; set; }
        public int RequestorCooperatorID { get; set; }
        public string RequestorCooperatorName { get; set; }
        public string RequestorEmailAddress { get; set; }
        public string RequestorOrganization { get; set; }
        public int ShipToCooperatorID { get; set; }
        public string ShipToCooperatorName { get; set; }
        public int FinalRecipientCooperatorID { get; set; }
        public string FinalRecipientCooperatorName { get; set; }
        public string OrderObtainedVIA { get; set; }
        public int FeedbackID { get; set; }
        public string SpecialInstruction { get; set; }

        public List<OrderRequestItem> OrderRequestItems { get; set; }
        public List<OrderRequestAction> OrderRequestActions { get; set; }

        public OrderRequest()
        {
            OrderRequestItems = new List<OrderRequestItem>();
            OrderRequestActions = new List<OrderRequestAction>();
        }
    }
}
