using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class EconomicUsageTypeViewModel : EconomicUsageTypeViewModelBase, IViewModel<EconomicUsageType>
    {
        public EconomicUsageTypeViewModel()
        { 
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public EconomicUsageType Get(int entityId)
        {
            try
            {
                using (EconomicUsageTypeManager mgr = new EconomicUsageTypeManager())
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

        public int Insert()
        {
            using (EconomicUsageTypeManager mgr = new EconomicUsageTypeManager())
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

        public void Search()
        {
            using (EconomicUsageTypeManager mgr = new EconomicUsageTypeManager())
            {
                try
                {
                    DataCollection = new Collection<EconomicUsageType>(mgr.Search(SearchEntity));
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

        public int Update()
        {
            using (EconomicUsageTypeManager mgr = new EconomicUsageTypeManager())
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
    }
}
