using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class WebCooperatorViewModelBase : AuthenticatedViewModelBase
    {
        private WebCooperator _Entity = new WebCooperator();
        private WebCooperatorSearch _SearchEntity = new WebCooperatorSearch();
        private Collection<WebCooperator> _DataCollection = new Collection<WebCooperator>();
        private Collection<WebCooperator> _DataCollectionDashboard = new Collection<WebCooperator>();

        public WebCooperatorViewModelBase()
        {
            using (WebCooperatorManager mgr = new WebCooperatorManager())
            {
                Salutations = new SelectList(mgr.GetCodeValues("COOPERATOR_TITLE"), "Value", "Title");
            }
        }
        public WebCooperator Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public WebCooperatorSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<WebCooperator> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<WebCooperator> DataCollectionDashboard
        {
            get { return _DataCollectionDashboard; }
            set { _DataCollectionDashboard = value; }
        }

        #region Select Lists

        public SelectList Salutations { get; set; }

        #endregion
    }
}
