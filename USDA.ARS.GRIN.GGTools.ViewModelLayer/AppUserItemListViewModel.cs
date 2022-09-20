using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class AppUserItemListViewModel : AppUserItemListViewModelBase, IViewModel<AppUserItemList>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public AppUserItemList Get(int entityId)
        {
            throw new NotImplementedException();
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
            using (AppUserItemListManager mgr = new AppUserItemListManager())
            {
                try
                {
                    DataCollection = new Collection<AppUserItemList>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
                }
            }
        }

        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
