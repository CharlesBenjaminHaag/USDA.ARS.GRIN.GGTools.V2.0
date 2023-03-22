using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class WebCooperatorViewModel : WebCooperatorViewModelBase, IViewModel<WebCooperator>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public WebCooperator Get(int entityId, int cooperatorId = 0)
        {
            using (WebCooperatorManager mgr = new WebCooperatorManager())
            {
                if (entityId > 0)
                {
                    Entity = mgr.Get(entityId);
                }
                else
                {
                    if (cooperatorId > 0)
                    {
                        Entity = mgr.GetByCooperatorID(cooperatorId);
                    }
                }
            }
            return Entity;
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            using (WebCooperatorManager mgr = new WebCooperatorManager())
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
            using (WebCooperatorManager mgr = new WebCooperatorManager())
            {
                try
                {
                    DataCollection = new Collection<WebCooperator>(mgr.Search(SearchEntity));
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
            throw new NotImplementedException();
        }

        // Create a web cooperator based on an existing cooperator.
        public void Copy(int cooperatorId)
        {
            using (WebCooperatorManager mgr = new WebCooperatorManager())
            {
                Entity.ID = mgr.Copy(cooperatorId);
            }
        }

        public List<WebCooperator> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public WebCooperator Get(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
