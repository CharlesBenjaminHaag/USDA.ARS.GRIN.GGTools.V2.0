using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using System.Security.Cryptography.X509Certificates;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class OrderRequestManager : GRINGlobalDataManagerBase, Common.DataLayer.IManager<OrderRequest, OrderRequestSearch>
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
            return orderRequest;
        }

        public List<OrderRequestItem> GetItems(int orderRequestId)
        {
            List<OrderRequestItem> results = new List<OrderRequestItem>();

            SQL = " SELECT * FROM vw_GRINGlobal_Order_Request_Item ";
            SQL += " WHERE        (@OrderRequestID         IS NULL OR  OrderRequestID     = @OrderRequestID)";


            var parameters = new List<IDbDataParameter> {
                CreateParameter("@OrderRequestID", (object)orderRequestId, false)
            };

            results = GetRecords<OrderRequestItem>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }
        
        public List<OrderRequestAction> GetActions(int orderRequestId)
        {
            List<OrderRequestAction> results = new List<OrderRequestAction>();

            SQL = " SELECT * FROM vw_GRINGlobal_Order_Request_Action ";
            SQL += " WHERE        (@OrderRequestID         IS NULL OR  OrderRequestID       = @OrderRequestID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("OrderRequestID", orderRequestId > 0 ? (object)orderRequestId : DBNull.Value, true),
            };

            results = GetRecords<OrderRequestAction>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<OrderRequestAttachment> GetAttachments(int orderRequestId)
        {
            List<OrderRequestAttachment> orderRequestAttachments = new List<OrderRequestAttachment>();
            
            SQL = " SELECT order_request_attach_id AS ID, order_request_id AS OrderRequestID, ISNULL(title,'[No GroupTitle]') AS Title, content_type AS ContentType, category_code AS CategoryCode, description AS Description, virtual_path AS VirtualPath, thumbnail_virtual_path AS ThumbnailVirtualPath, attach_date AS AttachDate FROM order_request_attach ";
            SQL += " WHERE        (@OrderRequestID         IS NULL OR  order_request_id     = @OrderRequestID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("OrderRequestID", orderRequestId > 0 ? (object)orderRequestId : DBNull.Value, true),
            };

            orderRequestAttachments = GetRecords<OrderRequestAttachment>(SQL, parameters.ToArray());
            RowsAffected = orderRequestAttachments.Count;
            return orderRequestAttachments;
        }

        public List<OrderRequestPhytoLog> GetPhytoLog(int orderRequestId)
        {
            List<OrderRequestPhytoLog> orderRequestPhytoLogs = new List<OrderRequestPhytoLog>();

            SQL = " SELECT * FROM vw_GRINGlobal_Order_Request_Phyto_Log ";
            SQL += " WHERE        (@OrderRequestID         IS NULL OR  OrderRequestID     = @OrderRequestID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("OrderRequestID", orderRequestId > 0 ? (object)orderRequestId : DBNull.Value, true),
            };

            orderRequestPhytoLogs = GetRecords<OrderRequestPhytoLog>(SQL, parameters.ToArray());
            RowsAffected = orderRequestPhytoLogs.Count;
            return orderRequestPhytoLogs;
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

            SQL = "usp_GRINGlobal_Order_Request_Search";
           
            var parameters = new List<IDbDataParameter> {
                CreateParameter("order_request_id", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("web_order_request_id", searchEntity.WebOrderRequestID > 0 ? (object)searchEntity.WebOrderRequestID : DBNull.Value, true),
                CreateParameter("order_type_code", (object)searchEntity.OrderTypeCode ?? DBNull.Value, true),
                CreateParameter("most_recent_order_request_action_code", (object)searchEntity.MostRecentOrderActionCode ?? DBNull.Value, true),
                CreateParameter("requestor_name", (object)searchEntity.RequestorCooperatorName ?? DBNull.Value, true),
                CreateParameter("requestor_email", (object)searchEntity.RequestorEmailAddress ?? DBNull.Value, true),
                CreateParameter("requestor_organization", (object)searchEntity.RequestorOrganizationName ?? DBNull.Value, true),
                CreateParameter("ordered_date", searchEntity.OrderedDate == DateTime.MinValue ? DBNull.Value : (object)searchEntity.OrderedDate,
                true),
                CreateParameter("ordered_year", searchEntity.OrderedYear > 0 ? (object)searchEntity.OrderedYear : DBNull.Value, true),
                CreateParameter("time_frame", (object)searchEntity.OrderedDateTimeFrame ?? DBNull.Value, true),
                CreateParameter("intended_use_code", (object)searchEntity.IntendedUseCode ?? DBNull.Value, true),
            };

            results = GetRecords<OrderRequest>(SQL, CommandType.StoredProcedure, parameters.ToArray());
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
