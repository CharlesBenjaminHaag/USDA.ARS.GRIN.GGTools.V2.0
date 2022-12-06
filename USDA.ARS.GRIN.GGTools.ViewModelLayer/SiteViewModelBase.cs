using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SiteViewModelBase: AppViewModelBase
    {
        private Site _Entity = new Site();
        private SiteSearch _SearchEntity = new SiteSearch();
        private Collection<Site> _DataCollection = new Collection<Site>();
        private Collection<Cooperator> _DataCollectionSiteCooperators = new Collection<Cooperator>();
        public SiteViewModelBase()
        {
            using (SiteManager mgr = new SiteManager())
            {
                Types = new SelectList(mgr.GetCodeValues("SITE_TYPE"), "Value", "Title");
            }
        }

        public Site Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public SiteSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<Site> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<Cooperator> DataCollectionSiteCooperators
        {
            get { return _DataCollectionSiteCooperators; }
            set { _DataCollectionSiteCooperators = value; }
        }
        public SelectList Types { get; set; }
    }
}
