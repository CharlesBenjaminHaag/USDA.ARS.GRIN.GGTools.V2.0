using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.OrderManagement.DataLayer.ManagerClasses
{
    public class OrderRequestManager : AppDataManagerBase, IManager<WebOrderRequest, WebOrderRequestSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(WebOrderRequest entity)
        {
            throw new NotImplementedException();
        }

        public WebOrderRequest Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(WebOrderRequest entity)
        {
            throw new NotImplementedException();
        }

        public List<WebOrderRequest> Search(WebOrderRequestSearch searchEntity)
        {
            throw new NotImplementedException();
        }

        public int Update(WebOrderRequest entity)
        {
            throw new NotImplementedException();
        }
    }
}
