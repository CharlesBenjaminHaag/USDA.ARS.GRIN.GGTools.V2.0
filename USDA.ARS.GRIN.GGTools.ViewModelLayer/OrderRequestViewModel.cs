using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.OrderManagement.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class OrderRequestViewModel : OrderRequestViewModelBase, IViewModel<OrderRequest>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public OrderRequest Get(int entityId)
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
            throw new NotImplementedException();
        }

        public List<OrderRequest> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
