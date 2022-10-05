using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class CWRTraitViewModel: CWRTraitViewModelBase, IViewModel<CWRTrait>
    {
        public void Delete()
        {
            try
            {
                using (CWRMapManager mgr = new CWRMapManager())
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

        public CWRTrait Get(int entityId)
        {
            try
            {
                using (CWRTraitManager mgr = new CWRTraitManager())
                {
                    try
                    {
                        SearchEntity.ID = entityId;
                        Search();
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

        public void HandleRequest()
        {
        }

        public int Insert()
        {
            using (CWRTraitManager mgr = new CWRTraitManager())
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
            }
            return RowsAffected;
        }

        public int Search()
        {
            using (CWRTraitManager mgr = new CWRTraitManager())
            {
                try
                {
                    DataCollection = new Collection<CWRTrait>(mgr.Search(SearchEntity));
                    if (DataCollection.Count > 0)
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
                return 0;
            }
        }

        public void SearchFolderItems()
        {
            using (CWRTraitManager mgr = new CWRTraitManager())
            {
                try
                {
                    DataCollection = new Collection<CWRTrait>(mgr.SearchFolderItems(SearchEntity));
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
            using (CWRMapManager mgr = new CWRMapManager())
            {
                DataCollectionNotes = new Collection<CodeValue>(mgr.SearchNotes(SearchEntity.TableName, SearchEntity.Note));
            }
            return codeValues;
        }

        public int Update()
        {
            using (CWRTraitManager mgr = new CWRTraitManager())
            {
                try
                {
                    mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
            return RowsAffected;
        }

        void IViewModel<CWRTrait>.Search()
        {
            throw new NotImplementedException();
        }
    }
}
