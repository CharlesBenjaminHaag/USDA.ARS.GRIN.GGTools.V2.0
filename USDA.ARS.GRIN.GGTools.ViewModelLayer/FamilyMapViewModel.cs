using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class FamilyMapViewModel : FamilyMapViewModelBase, IViewModel<FamilyMap>
    {
        public void Delete()
        {
            try
            {
                using (FamilyMapManager mgr = new FamilyMapManager())
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
        public FamilyMap Get(int entityId)
        {
            using (FamilyMapManager mgr = new FamilyMapManager())
            {
                try
                {
                    SearchEntity.ID = entityId;
                    Entity = new Collection<FamilyMap>(mgr.Search(SearchEntity))[0];
                    Entity.IsAccepted = ToBool(Entity.IsAcceptedName);
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
        public void GetSynonyms(int entityId)
        {
            using (FamilyMapManager mgr = new FamilyMapManager())
            {
                try
                {
                    DataCollectionSynonyms = new Collection<FamilyMap>(mgr.GetSynonyms(entityId));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        public void GetSubdivisions(int entityId)
        {
            using (FamilyMapManager mgr = new FamilyMapManager())
            {
                try
                {
                    DataCollectionSubdivisions = new Collection<FamilyMap>(mgr.GetSubdivisions(entityId));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public void GetGenera(int entityId)
        {
            using (FamilyMapManager mgr = new FamilyMapManager())
            {
                try
                {
                    DataCollectionGenera = new Collection<Genus>(mgr.GetGenera(entityId));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
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
                switch(Entity.FamilyRank.ToUpper())
                {
                    case "FAMILY":
                        using (FamilyMapManager mgr = new FamilyMapManager())
                        {
                            Entity.ID = mgr.Insert(Entity);
                        }
                        break;
                    case "SUBFAMILY":
                        using (FamilyMapManager mgr = new FamilyMapManager())
                        {
                            Entity.ID = mgr.InsertSubfamily(Entity);
                        }
                        break;
                    case "TRIBE":
                        using (FamilyMapManager mgr = new FamilyMapManager())
                        {
                            Entity.ID = mgr.InsertTribe(Entity);
                        }
                        break;
                    case "SUBTRIBE":
                        using (FamilyMapManager mgr = new FamilyMapManager())
                        {
                            Entity.ID = mgr.InsertSubtribe(Entity);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
            return Entity.ID;
        }
        public int InsertSubfamily()
        {
            try
            {
                using (FamilyMapManager mgr = new FamilyMapManager())
                {
                    Entity.ID = mgr.InsertSubfamily(Entity);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
            return Entity.ID;
        }
        public int InsertTribe()
        {
            try
            {
                using (FamilyMapManager mgr = new FamilyMapManager())
                {
                    Entity.ID = mgr.InsertTribe(Entity);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
            return Entity.ID;
        }
        public int InsertSubtribe()
        {
            try
            {
                using (FamilyMapManager mgr = new FamilyMapManager())
                {
                    Entity.ID = mgr.InsertSubtribe(Entity);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
            return Entity.ID;
        }
        public void Search()
        {
            using (FamilyMapManager mgr = new FamilyMapManager())
            {
                try
                {
                    DataCollection = new Collection<FamilyMap>(mgr.Search(SearchEntity));
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
            using (FamilyMapManager mgr = new FamilyMapManager())
            {
                try
                {
                    DataCollection = new Collection<FamilyMap>(mgr.SearchFolderItems(SearchEntity));
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
        public List<FamilyMap> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }
        public int Update()
        {
            try
            {
                if (Entity.IsAcceptedName == "Y")
                {
                    Entity.AcceptedID = Entity.ID;
                }

                using (FamilyMapManager mgr = new FamilyMapManager())
                {
                    switch (Entity.FamilyRank.ToUpper())
                    {
                        case "FAMILY":
                            Entity.ID = mgr.Update(Entity);
                            break;
                        case "SUBFAMILY":
                            Entity.ID = mgr.UpdateSubfamily(Entity);
                            break;
                        case "TRIBE":
                            Entity.ID = mgr.UpdateTribe(Entity);
                            break;
                        case "SUBTRIBE":
                            Entity.ID = mgr.UpdateSubtribe(Entity);
                            break;
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
        public int UpdateSubfamily()
        {
            try
            {
                using (FamilyMapManager mgr = new FamilyMapManager())
                {
                    Entity.ID = mgr.UpdateSubfamily(Entity);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
            return Entity.ID;
        }
        public int UpdateTribe()
        {
            try
            {
                using (FamilyMapManager mgr = new FamilyMapManager())
                {
                    Entity.ID = mgr.UpdateTribe(Entity);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
            return Entity.ID;
        }
        public int UpdateSubtribe()
        {
            try
            {
                using (FamilyMapManager mgr = new FamilyMapManager())
                {
                    Entity.ID = mgr.UpdateSubtribe(Entity);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
            return Entity.ID;
        }
        public string GetPageTitle()
        {
            string pageTitle = String.Empty;
            switch (Entity.FamilyRank.ToUpper())
            {
                case "FAMILY":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.FamilyRank.ToLower()), Entity.ID, Entity.FamilyName);
                    break;
                case "SUBFAMILY":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.FamilyRank.ToLower()), Entity.ID, Entity.SubfamilyName);
                    break;
                case "TRIBE":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.FamilyRank.ToLower()), Entity.ID, Entity.TribeName);
                    break;
                case "SUBTRIBE":
                    pageTitle = String.Format("Edit {0} [{1}]: {2}", ToTitleCase(Entity.FamilyRank.ToLower()), Entity.ID, Entity.SubtribeName);
                    break;
            }
            return pageTitle;
        }
    }
}
