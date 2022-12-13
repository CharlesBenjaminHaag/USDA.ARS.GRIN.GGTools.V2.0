using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;
using System.Linq;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class GenusViewModel : GenusViewModelBase, IViewModel<Genus>
    {
        public GenusViewModel()
        { }
        public void Delete()
        {
            try
            {
                using (GenusManager mgr = new GenusManager())
                {
                    mgr.Delete(TableName, Entity.ID);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }
        public Genus Get(int entityId)
        {
            using (GenusManager mgr = new GenusManager())
            {
                try
                {
                    SearchEntity.ID = entityId;
                    Entity = new Collection<Genus>(mgr.Search(SearchEntity))[0];
                    Entity.IsAccepted = ToBool(Entity.IsAcceptedName);
                    Entity.IsWebVisibleOption = ToBool(Entity.IsWebVisible);

                    if (Entity.Rank != "GENUS")
                    {
                        GetParentGenus();
                    }
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
            return Entity;
        }
        public void GetFolderItems()
        {
            using (GenusManager mgr = new GenusManager())
            {
                try
                {
                    DataCollection = new Collection<Genus>(mgr.GetFolderItems(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        public void GetSynonyms(int entityId)
        {
            using (GenusManager mgr = new GenusManager())
            {
                try
                {
                    DataCollectionSynonyms = new Collection<Genus>(mgr.GetSynonyms(entityId));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        public void GetSubdivisions(string genusName)
        {
            using (GenusManager mgr = new GenusManager())
            {
                try
                {
                    DataCollectionSubdivisions = new Collection<Genus>(mgr.GetSubdivisions(genusName));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        public void GetParentGenus()
        {
            using (GenusManager mgr = new GenusManager())
            {
                TopRankGenusEntity = mgr.GetGenus(Entity.Name);
                Entity.ParentID = TopRankGenusEntity.ID;
                Entity.ParentName = TopRankGenusEntity.Name;
            }
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            try
            {
                using (GenusManager mgr = new GenusManager())
                {
                    Entity.ID = mgr.Insert(Entity);
                }

                if (IsTypeGenus == "Y")
                {
                    using (FamilyMapManager familyMapManager = new FamilyMapManager())
                    {
                        FamilyMapSearch familyMapSearch = new FamilyMapSearch { ID = Entity.FamilyID };
                        FamilyMap familyMap = new FamilyMap();
                        familyMap = familyMapManager.Search(familyMapSearch).FirstOrDefault();

                        familyMap.TypeGenusID = Entity.ID;
                        familyMapManager.Update(familyMap);
                    }
                }
                return RowsAffected;
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }

        public void Search()
        {
            using (GenusManager mgr = new GenusManager())
            {
                try
                {
                    DataCollection = new Collection<Genus>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public void SearchFolderItems()
        {
            using (GenusManager mgr = new GenusManager())
            {
                try
                {
                    DataCollection = new Collection<Genus>(mgr.SearchFolderItems(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public List<CodeValue> SearchNotes()
        {
            List<CodeValue> codeValues = new List<CodeValue>();
            using (GenusManager mgr = new GenusManager())
            {
                DataCollectionNotes = new Collection<CodeValue>(mgr.SearchNotes(SearchEntity.TableName, SearchEntity.Note));
            }
            return codeValues;
        }

        public int Update()
        {
            using (GenusManager mgr = new GenusManager())
            {
                try
                {
                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }

        public string GetPageTitle()
        {
            string pageTitle = String.Empty;
            switch (Entity.Rank.ToUpper())
            {
                case "GENUS":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.Rank.ToLower()), Entity.ID, Entity.Name);
                    break;
                case "SUBGENUS":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.Rank.ToLower()), Entity.ID, Entity.SubgenusName);
                    break;
                case "SECTION":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.Rank.ToLower()), Entity.ID, Entity.SectionName);
                    break;
                case "SUBSECTION":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.Rank.ToLower()), Entity.ID, Entity.SubsectionName);
                    break;
                case "SERIES":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.Rank.ToLower()), Entity.ID, Entity.SeriesName);
                    break;
                case "SUBSERIES":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.Rank.ToLower()), Entity.ID, Entity.SubseriesName);
                    break;
                default:
                    pageTitle = String.Format("Edit Genus [{0}]: {1}", Entity.ID, Entity.Name);
                    break;
            }
            //DEBUG
            pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.Rank.ToLower()), Entity.ID, Entity.AssembledName);

            return pageTitle;
        }
    }
}
