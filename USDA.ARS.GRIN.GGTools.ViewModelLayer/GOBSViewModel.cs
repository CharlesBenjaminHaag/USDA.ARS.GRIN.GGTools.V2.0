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
using USDA.ARS.GRIN.GRINGlobal.DTO;

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
                   // mgr.Delete(TableName, Entity.ID);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }

        public Dataset GetDataset(int cooperaorId, int entityId)
        {
            try
            {
                using (GOBSManager mgr = new GOBSManager())
                {
                    try
                    {
                        Entity = mgr.GetDataset(cooperaorId, entityId);

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

        public void GetDatasets(int cooperatorId)
        {
            try
            {
                using (GOBSManager mgr = new GOBSManager())
                {
                    try
                    {
                       DataCollectionDatasets = new Collection<Dataset>(mgr.GetDatasets(cooperatorId));
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
        }

        public DatasetAttach GetGOBSDatasetAttachment(int entityId)
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

   
        public int Insert()
        {
            using (GOBSManager mgr = new GOBSManager())
            {
                try
                {
                    //RowsAffected = mgr.Insert(Entity);
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
                    //RowsAffected = mgr.Update(Entity);
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
 
    }
}
