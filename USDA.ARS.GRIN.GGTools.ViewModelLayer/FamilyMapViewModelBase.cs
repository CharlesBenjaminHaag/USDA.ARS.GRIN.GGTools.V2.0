using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Caching;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class FamilyMapViewModelBase : AppViewModelBase
    {
        private string _DetailPartialViewName;
        private string _EditPartialViewName;
        private string _ListPartialViewName;
        private FamilyMap _Entity = new FamilyMap();
        private FamilyMap _ParentEntity = new FamilyMap();
        private FamilyMapSearch _SearchEntity = new FamilyMapSearch();
        private Collection<FamilyMap> _DataCollection = new Collection<FamilyMap>();
        private Collection<FamilyMap> _DataCollectionSynonyms = new Collection<FamilyMap>();
        private Collection<FamilyMap> _DataCollectionSubdivisions = new Collection<FamilyMap>();
        private Collection<Genus> _DataCollectionGenera = new Collection<Genus>();

        public FamilyMapViewModelBase()
        {
            using (FamilyMapManager mgr = new FamilyMapManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("taxonomy_family_map"), "ID", "FullName");
                FamilyTypes = new SelectList(mgr.GetCodeValues("TAXONOMY_FAMILY_TYPE"), "Value", "Title");
                //FamilyMaps = new SelectList(GetFamilyMaps().Where(x => x.FamilyRank == "FAMILY").OrderBy(x => x.FamilyName), "ID", "FamilyName");
                //Subfamilies = new SelectList(GetFamilyMaps().Where(x => x.FamilyRank == "SUBFAMILY").OrderBy(x => x.SubfamilyName), "ID", "SubfamilyName");
                //Tribes = new SelectList(GetFamilyMaps().Where(x => x.FamilyRank == "TRIBE").OrderBy(x => x.TribeName), "ID", "TribeName");
                //Subtribes = new SelectList(GetFamilyMaps().Where(x => x.FamilyRank == "SUBTRIBE").OrderBy(x => x.SubtribeName), "ID", "SubtribeName");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
            }

            using (ClassificationManager classificationMgr = new ClassificationManager())
            {
                Orders = new SelectList(classificationMgr.Search(new ClassificationSearch()),"ID","OrderName");
            }
        }
        public string EditPartialViewName
        {
            get { return _EditPartialViewName; }
            set { _EditPartialViewName = value; }
        }
        public string DetailPartialViewName
        {
            get { return _DetailPartialViewName; }
            set { _DetailPartialViewName = value; }
        }
        public string ListPartialViewName
        {
            get { return _ListPartialViewName; }
            set { _ListPartialViewName = value; }
        }
        public FamilyMap Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public FamilyMap ParentEntity
        {
            get { return _ParentEntity; }
            set { _ParentEntity = value; }
        }

        public FamilyMapSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }
        public Collection<FamilyMap> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<FamilyMap> DataCollectionSynonyms
        {
            get { return _DataCollectionSynonyms; }
            set { _DataCollectionSynonyms = value; }
        }
        public Collection<FamilyMap> DataCollectionSubdivisions
        {
            get { return _DataCollectionSubdivisions; }
            set { _DataCollectionSubdivisions = value; }
        }
        public Collection<Genus> DataCollectionGenera
        {
            get { return _DataCollectionGenera; }
            set { _DataCollectionGenera = value; }
        }

        private List<FamilyMap> GetFamilyMaps()
        {
            List<FamilyMap> familyMaps = new List<FamilyMap>();

            ObjectCache cache = MemoryCache.Default;
            familyMaps = cache["DATA-LIST-FAMILY-MAPS"] as List<FamilyMap>;

            if (familyMaps == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                using (FamilyMapManager mgr = new FamilyMapManager())
                {
                    familyMaps = mgr.Search(new FamilyMapSearch());
                }
                cache.Set("DATA-LIST-FAMILY-MAPS", familyMaps, policy);
            }
            return familyMaps;
        }

        public SelectList FamilyTypes { get; set; }
        public SelectList Orders { get; set; }
        public SelectList Genera { get; set;}
        public SelectList FamilyMaps { get; set; }
        public SelectList Families { get; set; }
        public SelectList Subfamilies { get; set; }
        public SelectList Tribes { get; set; }
        public SelectList Subtribes { get; set; }

    }
}
