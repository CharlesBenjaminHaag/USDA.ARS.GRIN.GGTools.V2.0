using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.GOBS.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.GOBS.ViewModelLayer
{
    public class GOBSViewModel : GOBSViewModelBase, IViewModel<GOBSViewModel>
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

        public GOBSDataset GetDataset(int entityId)
        {
            try
            {
                using (GOBSManager mgr = new GOBSManager())
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
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
            return Entity;
        }

        public GOBSDatasetAttachment GetGOBSDatasetAttachment(int entityId)
        {
            try
            {
                using (GOBSManager mgr = new GOBSManager())
                {
                    try
                    {
                         //TODO
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
            return null;
        }

        public GOBSDatasetAttachment GetGOBSDatasetField(int entityId)
        {
            return null;
        }

        public GOBSDatasetAttachment GetGOBSDatasetInventory(int entityId)
        {
            return null;
        }

        public GOBSDatasetAttachment GetGOBSDatasetMarker(int entityId)
        {
            return null;
        }

        public GOBSDatasetAttachment GetGOBSDatasetMarkerField(int entityId)
        {
            return null;
        }

        public GOBSDatasetAttachment GetGOBSDatasetMarkerValue(int entityId)
        {
            return null;
        }

        public GOBSDatasetAttachment GetGOBSDatasetValue(int entityId)
        {
            return null;
        }

        public GOBSDatasetAttachment GetGOBSMarker(int entityId)
        {
            return null;
        }

        public GOBSDatasetAttachment GetGOBSMarkerField(int entityId)
        {
            return null;
        }

        public GOBSDatasetAttachment GetGOBSMarkerValue(int entityId)
        {
            return null;
        }

        public GOBSDatasetAttachment GetGOBSReportTrait(int entityId)
        {
            return null;
        }

        public GOBSDatasetAttachment GetGOBSReportValue(int entityId)
        {
            return null;
        }

        public GOBSDatasetAttachment GetGOBSType(int entityId)
        {
            return null;
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            using (GOBSManager mgr = new GOBSManager())
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
            using (GOBSManager mgr = new GOBSManager())
            {
                try
                {
                   

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
            using (GOBSManager mgr = new GOBSManager())
            {
                try
                {
                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
            return RowsAffected;
        }
       
     
        public override bool Validate()
        {
            bool validated = true;

            return validated;
        }

        GOBSViewModel IViewModel<GOBSViewModel>.Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<GOBSViewModel> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public void GetSysTables(int sysUserId, string databaseAreaCode)
        {
            using (SysTableManager mgr = new SysTableManager())
            {
                DataCollectionSysTables = new Collection<SysTable>(mgr.GetSysTables(sysUserId, databaseAreaCode));
            }
        }
    }
}
