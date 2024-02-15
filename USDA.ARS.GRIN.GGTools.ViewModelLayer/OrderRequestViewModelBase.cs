using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class OrderRequestViewModelBase : AppViewModelBase
    {
        private OrderRequest _Entity = new OrderRequest();
        private OrderRequestSearch _SearchEntity = new OrderRequestSearch();
        private Collection<OrderRequest> _DataCollection = new Collection<OrderRequest>();

        public OrderRequest Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }
        public OrderRequestSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<OrderRequest> DataCollection
        { get { return _DataCollection; } 
        set { _DataCollection = value; } 
        }

    }
}
