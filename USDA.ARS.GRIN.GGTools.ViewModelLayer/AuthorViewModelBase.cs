using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer;
using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class AuthorViewModelBase : AppViewModelBase
    {
        private Author _Entity = new Author();
        private AuthorSearch _SearchEntity = new AuthorSearch();
        private Collection<Author> _DataCollection = new Collection<Author>();
        private Collection<CodeValue> _DataCollectionNotes = new Collection<CodeValue>();
        private int _SearchResultsFormat;

        public AuthorViewModelBase()
        {
            using (AuthorManager mgr = new AuthorManager())
            {
                Cooperators = new SelectList(mgr.GetCooperators("taxonomy_author"), "ID", "FullName");
            }
        }

        public Author Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public AuthorSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<Author> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }

        public Collection<CodeValue> DataCollectionNotes
        {
            get { return _DataCollectionNotes; }
            set { _DataCollectionNotes = value; }
        }

        public int SearchResultsFormat
        {
            get { return _SearchResultsFormat; }
            set { _SearchResultsFormat = value; }

        }
    }
}
