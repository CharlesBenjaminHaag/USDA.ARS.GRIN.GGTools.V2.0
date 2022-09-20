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

        public WebCooperator Get(int entityId)
        {
            using (WebCooperatorManager mgr = new WebCooperatorManager())
            {
                Entity = mgr.Get(entityId);
            }
            return Entity;
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            throw new NotImplementedException();
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

        public List<WebCooperator> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
