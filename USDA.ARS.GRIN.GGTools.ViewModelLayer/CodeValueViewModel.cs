using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class CodeValueViewModel : CodeValueViewModelBase, IViewModel<CodeValue>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Get(int entityId)
        {
            using (CodeValueManager mgr = new CodeValueManager())
            {
                try
                {
                    Entity = mgr.Get(entityId);
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
            using (CodeValueManager mgr = new CodeValueManager())
            {
                try
                {
                    Entity.ID = mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return Entity.ID;
            }
        }

        public void Search()
        {
            using (CodeValueManager mgr = new CodeValueManager())
            {
                try
                {
                    DataCollection = new Collection<CodeValue>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public List<CodeValue> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            using (CodeValueManager mgr = new CodeValueManager())
            {
                try
                {
                    Entity.ID = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return Entity.ID;
            }
        }

        CodeValue IViewModel<CodeValue>.Get(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
