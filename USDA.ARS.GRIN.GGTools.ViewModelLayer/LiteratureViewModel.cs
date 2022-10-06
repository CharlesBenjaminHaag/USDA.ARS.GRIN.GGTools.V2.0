using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class LiteratureViewModel : LiteratureViewModelBase, IViewModel<Literature>
    {
        public void Delete()
        {
            try
            {
                using (GRINGlobalDataManagerBase mgr = new GRINGlobalDataManagerBase())
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

        public Literature Get(int entityId)
        {
            using (LiteratureManager mgr = new LiteratureManager())
            {
                try
                {
                    SearchEntity.ID = entityId;
                    DataCollection = new Collection<Literature>(mgr.Search(SearchEntity));
                    DataCollectionCitations = new Collection<Citation>();
                    Entity = DataCollection[0];
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
                return Entity;
            }
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            using (LiteratureManager mgr = new LiteratureManager())
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

        public void Search()
        {
            using (LiteratureManager mgr = new LiteratureManager())
            {
                try
                {
                    DataCollection = new Collection<Literature>(mgr.Search(SearchEntity));
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

        public List<Literature> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            using (LiteratureManager mgr = new LiteratureManager())
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
            }
            return RowsAffected;
        }
    }
}
