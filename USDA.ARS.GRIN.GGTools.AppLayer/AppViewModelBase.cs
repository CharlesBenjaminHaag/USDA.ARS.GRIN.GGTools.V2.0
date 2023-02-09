using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Configuration;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using USDA.ARS.GRIN.Common.Library;

namespace USDA.ARS.GRIN.GGTools.AppLayer
{
  /// <summary>
  /// This class is the base class for all view models for this specific application
  /// </summary>
  public class AppViewModelBase : ViewModelBase
  {
        private string _PageTitle;
        private string _ResultText;
        private string _UserMessage;
        private string _IsMultiSelectable;
        
        public int ID { get; set; }
        [AllowHtml]
        public string PublicWebsiteBaseURL
        {
            get
            {
                string publicWebsiteUrl = String.Empty;
                publicWebsiteUrl = ConfigurationManager.AppSettings["PublicWebsiteBaseUrl"];
                return publicWebsiteUrl;
            }
        }

        public string PageTitle
        {
            get
            {
                return _PageTitle;
            }
            set
            {
                _PageTitle = value;
            }
        }
        public string ResultText
        {
            get 
            { 
                return _ResultText; 
            }
            set 
            {
                _ResultText = value;
                RaisePropertyChanged("ResultText");
            }
        }
        public string UserMessage
        {
            get 
            { 
                return _UserMessage; 
            }
            set
            {
                _UserMessage = value;
            }
        }
        public string EventAction { get; set; }
        public string EventValue { get; set; }
        public string EventNote { get; set; }
        public string TableName { get; set; }
        public string TableCode { get; set; }
        public string TableKeyFieldName { get; set; }
        public string SessionKeyName 
        { 
            get 
            { 
                return "SEARCH_" + TableCode.ToUpper(); 
            } 
        }
        public int AuthenticatedUserCooperatorID { get; set; }
        public string ItemIDList { get; set; }
        public string EntityIDList { get; set; }
        public string IsMultiSelectable 
        { 
            get { return _IsMultiSelectable; }
            set { _IsMultiSelectable = value; }
        }
     
        #region Init Method
        public override void Init()
            {
            base.Init();

            ResultText = string.Empty;
            }
            #endregion

        public string SerializeToXml<T>(T value)
        {
            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(writer, value);
            return writer.ToString();
        }

        public T Deserialize<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        #region Utilities

        public string FormatBoolean(string value)
        {
            if (String.IsNullOrEmpty(value))
                return "";

            if ((value.ToUpper() == "Y") || (value.ToUpper() == "TRUE") || (value.ToUpper() == "YES"))
                return "<span class='badge badge-success'>Yes</span>";
            else
            {
                if ((value.ToUpper() == "N") || (value.ToUpper() == "FALSE") || (value.ToUpper() == "NO"))
                    return "<span class='badge badge-danger'>No</span>";
                else
                {
                    if (value.ToUpper() == "X")
                        return "<span class='label label-default'>Not Verified</span>";
                    else
                        return value;
                }
            }
        }
        public bool ToBool(string value)
        {
            bool convertedValue = false;
            if (value == "Y") convertedValue = true;
            return convertedValue;
        }

        public string FromBool(bool value)
        {
            string convertedValue = "N";
            if (value) 
                convertedValue = "Y";
            return convertedValue;
        }

        public string ToTitleCase(string value)
        {
            System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(value.ToLower());
        }

        #endregion

        #region Select Lists

        public SelectList TimeFrameOptions { get; set; }
        public SelectList Cooperators { get; set; }
        public SelectList YesNoOptions { get; set; }

        #endregion
    }
}
