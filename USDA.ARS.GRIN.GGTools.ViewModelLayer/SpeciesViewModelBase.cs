using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class SpeciesViewModelBase: AppViewModelBase
    {
        private string _PageTitle = String.Empty;
        private string _IsMultiSelect;
        private int _SpeciesID;
        private Species _Entity = new Species();
        private Species _ParentEntity = new Species();
        private SpeciesSearch _SearchEntity = new SpeciesSearch();
        private Collection<Species> _DataCollection = new Collection<Species>();
        private Collection<CodeValue> _DataCollectionProtologues = new Collection<CodeValue>();
        public SpeciesViewModelBase()
        {
            TableName = "taxonomy_species";
            using (SpeciesManager mgr = new SpeciesManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators(TableName), "ID", "FullName");
                TableNames = new SelectList(mgr.GetTableNames(), "Key", "Value");
                YesNoOptions = new SelectList(mgr.GetYesNoOptions(), "Key", "Value");
                TimeFrameOptions = new SelectList(mgr.GetCodeValues("TAXONOMY_SEARCH_TIME_FRAME"), "Value", "Title");
                SynonymCodes = new SelectList(mgr.GetCodeValues("TAXONOMY_SPECIES_QUALIFIER"), "Value", "Title");
                Ranks = new SelectList(mgr.GetRanks(),"Value","Title");
                FormaRankTypes = new SelectList(mgr.GetCodeValues("TAXONOMY_FORMA_RANK_TYPE"), "Value", "Title");
            }
        }
        public int SpeciesID
        {
            get { return _SpeciesID; }
            set { _SpeciesID = value; }
        }
        public string IsMultiSelect
        {
            get { return _IsMultiSelect; }
            set { _IsMultiSelect = value; }
        }
        public Species Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public Species ParentEntity
        {
            get { return _ParentEntity; }
            set { _ParentEntity = value; }
        }

        public SpeciesSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<Species> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<CodeValue> DataCollectionProtologues
        {
            get { return _DataCollectionProtologues; }
            set { _DataCollectionProtologues = value; }
        }

        #region Select Lists
        public SelectList TableNames { get; set; }
        public SelectList SynonymCodes { get; set; }
        public SelectList Folders { get; set; }
        public SelectList FilterOptions { get; set; }
        public SelectList Tags { get; set; }
        public SelectList Ranks { get; set; }
        public SelectList FormaRankTypes { get; set; }
                
        #endregion

        public string GetPageTitle()
        {
            if (Entity.ID > 0)
            {
                _PageTitle = String.Format("Edit Species [{0}]", Entity.ID);
            }
            else
            {
                if (String.IsNullOrEmpty(Entity.SynonymCode))
                {
                    _PageTitle = "Add Species";
                }
                else
                {
                    _PageTitle = "Add SubTaxon";
                }
            }
            return _PageTitle;
        }
    }
}
