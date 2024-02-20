using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.OrderManagement.DataLayer.ManagerClasses
{
    public class OrderRequestManager : GRINGlobalDataManagerBase, IManager<OrderRequest, OrderRequestSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(WebOrderRequest entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(OrderRequest entity)
        {
            throw new NotImplementedException();
        }

        public OrderRequest Get(int entityId)
        {
            SQL = "usp_GRINGlobal_Order_Request_Select";
            OrderRequest orderRequest = new OrderRequest();

            var parameters = new List<IDbDataParameter> {
            CreateParameter("order_request_id", (object)entityId, false)
            };

            orderRequest = GetRecord<OrderRequest>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            orderRequest.OrderRequestItems = GetItems(orderRequest.ID);
            orderRequest.OrderRequestActions = GetActions(orderRequest.ID);
            return orderRequest;
        }

        public List<OrderRequestItem> GetItems(int entityId)
        {
            List<OrderRequestItem> orderRequestItems = new List<OrderRequestItem>();
            //TODO
            return orderRequestItems; 
        }
        
        public List<OrderRequestAction> GetActions(int entityId)
        {
            List<OrderRequestAction> orderRequestActions = new List<OrderRequestAction>();
            //TODO
            return orderRequestActions;
        }

        public int Insert(WebOrderRequest entity)
        {
            throw new NotImplementedException();
        }

        public int Insert(OrderRequest entity)
        {
            throw new NotImplementedException();
        }

        public List<OrderRequest> Search(OrderRequestSearch searchEntity)
        {
            List<OrderRequest> results = new List<OrderRequest>();

            SQL = " SELECT * FROM vw_GRINGlobal_Order_Request ";
            SQL += " WHERE      (@ID                        IS NULL OR  ID = @ID) ";
            SQL += " AND        (@CreatedByCooperatorID     IS NULL OR  CreatedByCooperatorID   = @CreatedByCooperatorID)";
            SQL += " AND        (@WebOrderRequestID         IS NULL OR  WebOrderRequestID       = @WebOrderRequestID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("WebOrderRequestID", searchEntity.WebOrderRequestID > 0 ? (object)searchEntity.WebOrderRequestID : DBNull.Value, true),
            };

            results = GetRecords<OrderRequest>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public int Update(WebOrderRequest entity)
        {
            throw new NotImplementedException();
        }

        public int Update(OrderRequest entity)
        {
            throw new NotImplementedException();
        }
    }
}
