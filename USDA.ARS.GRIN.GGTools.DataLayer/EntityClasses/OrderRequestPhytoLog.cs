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
    public class OrderRequestPhytoLog : AppEntityBase
    {
        public int OrderRequestID { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime OpenedDate { get; set; }
        public string NumberOfPackages { get; set; }
        public int NumberOfItems { get; set; }
        public string MajorGenus { get; set; }
        public string ImportPermitIdentifier { get; set; }
        public DateTime SetupInspectionDate { get; set; }
        public DateTime InspectedDate { get; set; }
        public int InspectedByCooperatorID { get; set; }
        public string PhytoCertificateTypeCode { get; set; }
        public int CertificateCost { get; set; }
        public string PhytoCertificateIdentifier { get; set; }
        public DateTime ShippedDate { get; set; }
        public string ShippingCarrier { get; set; }
        public string TrackingIdentifier { get; set; }
        public string PassedInspection { get; set; }
        public string IsDelivered { get; set; }
        public DateTime DeliveredDate { get; set; }
    }
}
