using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class EconomicUseViewModel : EconomicUseViewModelBase, IViewModel<EconomicUse>
    {
        public EconomicUseViewModel()
        { 
        }
        
        public void Delete()
        {
            try
            {
                using (EconomicUseManager mgr = new EconomicUseManager())
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

        public EconomicUse Get(int entityId)
        {
            try
            {
                using (EconomicUseManager mgr = new EconomicUseManager())
                {
                    try
                    {
                        SearchEntity.ID = entityId;
                        Search();
                        if (DataCollection.Count == 1)
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
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
            return Entity;
        }

        public void GetFolderItems()
        {
            using (EconomicUseManager mgr = new EconomicUseManager())
            {
                try
                {
                    DataCollection = new Collection<EconomicUse>(mgr.GetFolderItems(SearchEntity));
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
        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            using (EconomicUseManager mgr = new EconomicUseManager())
            {
                try
                {
                    RowsAffected = mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }

        public int Update()
        {
            using (EconomicUseManager mgr = new EconomicUseManager())
            {
                try
                {
                    mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }

        public void Search()
        {
            using (EconomicUseManager mgr = new EconomicUseManager())
            {
                try
                {
                    DataCollection = new Collection<EconomicUse>(mgr.Search(SearchEntity));
                    if (DataCollection.Count == 1)
                    {
                        Entity = DataCollection[0];
                    }
                    RowsAffected = mgr.RowsAffected;
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
            using (EconomicUseManager mgr = new EconomicUseManager())
            {
                try
                {
                    DataCollection = new Collection<EconomicUse>(mgr.SearchFolderItems(SearchEntity));
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

        public List<EconomicUse> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
