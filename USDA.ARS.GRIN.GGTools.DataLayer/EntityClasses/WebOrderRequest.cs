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
    public class WebOrderRequest : AppEntityBase
    {
        public int WebCooperatorID { get; set; }
        public int WebUserID { get; set; }
        public string WebCooperatorLastName { get; set; }
        public string WebCooperatorTitle { get; set; }
        public string WebCooperatorFirstName { get; set; }
        public string WebCooperatorFullName { get; set; }
        public string WebCooperatorPrimaryPhone { get; set; }
        public string WebCooperatorEmail { get; set; }
        public string WebCooperatorOrganization { get; set; }
        public DateTime WebCooperatorCreatedDate { get; set; }
        public string WebCooperatorVettedStatusCode { get; set; }
        public string WebCooperatorAddress1  { get; set; }
        public string WebCooperatorAddress2  { get; set; }
        public string WebCooperatorAddress3  { get; set; }
        public string WebCooperatorAddressCity  { get; set; }
        public string WebCooperatorAddressPostalIndex  { get; set; }
        public string WebCooperatorAddressState  { get; set; }
        public string ShippingAddress1  { get; set; }
        public string ShippingAddress2  { get; set; }
        public string ShippingAddress3  { get; set; }
        public string ShippingAddressCity  { get; set; }
        public string ShippingAddressPostalIndex  { get; set; }
        public string ShippingAddressState  { get; set; }

        public string WebCooperatorAddressFormatted
        {
            get 
            { 
                StringBuilder sbAddress = new StringBuilder();
                
                if (!String.IsNullOrEmpty(WebCooperatorAddress1))
                {
                    sbAddress.Append(WebCooperatorAddress1);
                    sbAddress.Append("<br>");
                }

                if (!String.IsNullOrEmpty(WebCooperatorAddress2))
                {
                    sbAddress.Append(WebCooperatorAddress2);
                    sbAddress.Append("<br>");
                }

                if (!String.IsNullOrEmpty(WebCooperatorAddress3))
                {
                    sbAddress.Append(WebCooperatorAddress3);
                    sbAddress.Append("<br>");
                }

                if (!String.IsNullOrEmpty(WebCooperatorAddressCity))
                {
                    sbAddress.Append(WebCooperatorAddressCity);
                    sbAddress.Append(", ");
                }

                if (!String.IsNullOrEmpty(WebCooperatorAddressState))
                {
                    sbAddress.Append(WebCooperatorAddressState);
                    sbAddress.Append(" ");
                }

                if (!String.IsNullOrEmpty(WebCooperatorAddressPostalIndex))
                {
                    sbAddress.Append(WebCooperatorAddressPostalIndex);
                }


                return sbAddress.ToString();
            }
        }

        public string ShippingAddressFormatted
        {
            get
            {
                StringBuilder sbAddress = new StringBuilder();

                if (!String.IsNullOrEmpty(ShippingAddress1))
                {
                    sbAddress.Append(ShippingAddress1);
                    sbAddress.Append("<br>");
                }

                if (!String.IsNullOrEmpty(ShippingAddress2))
                {
                    sbAddress.Append(ShippingAddress2);
                    sbAddress.Append("<br>");
                }

                if (!String.IsNullOrEmpty(ShippingAddress3))
                {
                    sbAddress.Append(ShippingAddress3);
                    sbAddress.Append("<br>");
                }

                if (!String.IsNullOrEmpty(ShippingAddressCity))
                {
                    sbAddress.Append(ShippingAddressCity);
                    sbAddress.Append(", ");
                }

                if (!String.IsNullOrEmpty(ShippingAddressState))
                {
                    sbAddress.Append(ShippingAddressState);
                    sbAddress.Append(" ");
                }

                if (!String.IsNullOrEmpty(ShippingAddressPostalIndex))
                {
                    sbAddress.Append(ShippingAddressPostalIndex);
                }


                return sbAddress.ToString();
            }
        }

        public DateTime OrderDate { get; set; }
        public string IntendedUseCode { get; set; }
        public string IntendedUseNote { get; set; }
        public string StatusCode { get; set; }
        public string MostRecentOrderAction { get; set; }
        public string MostRecentWebOrderAction { get; set; }
        public string IsPreviouslyNRRReviewed { get; set; }
        public string SpecialInstruction { get; set; }
        public string EmailAddressList { get; set; }
        public Cooperator WebCooperator { get; set; }
        public List<Cooperator> Cooperators { get; set; }
        public List<Address> Addresses { get; set; }
        public int TotalGenera { get; set; }
        public int TotalItems { get; set; }
        public int TotalSites { get; set; }
        public string CSSClass
        {
            get
            {
                switch (StatusCode)
                {
                    case "NRR_FLAG":
                        return "bg-red";
                    case "CANCELED":
                        return "bg-yellow";
                    case "ACCEPTED":
                        return "bg-green";
                    case "SUBMITTED":
                        return "bg-purple";
                    case "NRR_REVIEW":
                        return "bg-gray";
                    default:
                        return "";
                }
            }
        }
        public bool IsLocked { get; set; }
        public int RelatedOrders { get; set; }
        public int OwnedByWebUserID { get; set; }
        public string OwnedByWebCooperatorName { get; set; }
        public DateTime OwnedByDate { get; set; }
    }
}
