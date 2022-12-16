using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class WebOrderRequestManager : AppDataManagerBase, IManager<WebOrderRequest, WebOrderRequestSearch>
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
            List<WebOrderRequest> webOrderRequests = new List<WebOrderRequest>();
            WebOrderRequestSearch webOrderSearch = new WebOrderRequestSearch();
            webOrderSearch.ID = entityId;
            webOrderRequests = Search(webOrderSearch);
            if (webOrderRequests.Count > 0)
            {
                return webOrderRequests[0];
            }
            return null;
        }

        public int Insert(WebOrderRequest entity)
        {
            throw new NotImplementedException();
        }

        public int InsertWebOrderRequestAction(WebOrderRequestAction entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<WebOrderRequestAction>(entity);

            SQL = "usp_GGTools_Orders_WebOrderRequestAction_Insert";

            AddParameter("web_order_request_id", entity.WebOrderRequestID == 0 ? DBNull.Value : (object)entity.WebOrderRequestID, true);
            AddParameter("action_code", String.IsNullOrEmpty(entity.ActionCode) ? DBNull.Value : (object)entity.ActionCode, true);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, true);
            AddParameter("created_by", (object)entity.OwnedByWebUserID ?? DBNull.Value, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            
            //TODO Get err no.
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }

        public List<WebOrderRequest> Search(WebOrderRequestSearch searchEntity)
        {
            List<WebOrderRequest> results = new List<WebOrderRequest>();

            SQL = " SELECT ID, " +
                " WebCooperatorID, " +
                " WebCooperatorTitle, " +
                " WebCooperatorFirstName, " +
                " WebCooperatorLastName, " +
                " WebCooperatorFullName, " +
                " WebCooperatorPrimaryPhone, " +
                " WebCooperatorEmail, " +
                " WebCooperatorOrganization, " +
                " WebCooperatorCreatedDate, " +
                " WebCooperatorAddress1, " +
                " WebCooperatorAddress2, " +
                " WebCooperatorAddress3, " +
                " WebCooperatorAddressCity, " +
                " WebCooperatorAddressPostalIndex, " +
                " WebCooperatorAddressState, " +
                " ShippingAddress1, " +
                " ShippingAddress2, " +
                " ShippingAddress3, " +
                " ShippingAddressCity, " +
                " ShippingAddressPostalIndex, " +
                " ShippingAddressState, " +
                " OrderDate, " +
                " IntendedUseCode, " +
                " IntendedUseNote, " +
                " StatusCode, " +
                " MostRecentOrderAction, " +
                " MostRecentWebOrderAction, " +
                " WebCooperatorVettedStatusCode, " +
                " Note, " +
                " SpecialInstruction, " +
                " TotalItems, " +
                " IsLocked, " +
                " IsPreviouslyNRRReviewed, " +
                " RelatedOrders, " +
                " OwnedByWebUserID, " + 
                " OwnedByWebCooperatorName, " +
                " OwnedByDate, " +
                " CreatedDate " +
                " FROM vw_GGTools_Orders_WebOrderRequests";
            SQL += " WHERE (@FirstName              IS NULL     OR      WebCooperatorFirstName          LIKE        '%' + @FirstName + '%')";
            SQL += " AND (@ID                       IS NULL     OR      ID                              =           @ID)";
            SQL += " AND (@OwnedByWebUserID         IS NULL     OR      OwnedByWebUserID                =           @OwnedByWebUserID)";
            SQL += " AND (@LastName                 IS NULL     OR      WebCooperatorLastName           LIKE        '%' + @LastName + '%')";
            SQL += " AND (@EmailAddress             IS NULL     OR      WebCooperatorEmail              LIKE        '%' + @EmailAddress + '%')";
            SQL += " AND (@Organization             IS NULL     OR      WebCooperatorOrganization       LIKE        '%' + @Organization + '%')";
            SQL += " AND (@IntendedUseCode          IS NULL     OR      IntendedUseCode                 =           @IntendedUseCode)";
            SQL += " AND (@StatusCode               IS NULL     OR      StatusCode                      =           @StatusCode)";
            SQL += " AND (@Year                     IS NULL     OR      YEAR(CreatedDate)               =           @Year)";

            if (searchEntity.IsLocked == "Y")
            {
                SQL += " AND (@IsLocked IS NULL OR IsLocked = 1)";
            }

            if (searchEntity.HasOrders == "Y")
            {
                SQL += " AND (RelatedOrders > 0)";
            }
            
            switch (searchEntity.TimeFrame)
            {
                case "1D":
                    SQL += " AND (CONVERT(date, OrderDate) = CONVERT(date, GETDATE()))";
                    break;
                case "3D":
                    SQL += " AND OrderDate >= DATEADD(day,-3, GETDATE())";
                    break;
                case "7D":
                    SQL += " AND OrderDate >= DATEADD(day,-7, GETDATE())";
                    break;
                case "30D":
                    SQL += " AND OrderDate >= DATEADD(day,-30, GETDATE())";
                    break;
                case "60D":
                    SQL += " AND OrderDate >= DATEADD(day,-60, GETDATE())";
                    break;
                case "90D":
                    SQL += " AND OrderDate >= DATEADD(day,-190, GETDATE())";
                    break;
            }

            if (!String.IsNullOrEmpty(searchEntity.StatusList))
            {
                searchEntity.StatusList = String.Join(",", Array.ConvertAll(searchEntity.StatusList.Split(','), z => "'" + z + "'"));
                SQL += " AND StatusCode IN (" + searchEntity.StatusList + ")";
            }

            //if (!String.IsNullOrEmpty(searchEntity.WebUserList))
            //{
            //    searchEntity.WebUserList = String.Join(",", Array.ConvertAll(searchEntity.WebUserList.Split(','), z + "'"));
            //    SQL += " AND IntendedUseCode IN (" + searchEntity.IntendedUseList + ")";
            //}

            SQL += " ORDER BY OrderDate DESC";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("OwnedByWebUserID", searchEntity.OwnedByWebUserID > 0 ? (object)searchEntity.OwnedByWebUserID : DBNull.Value, true),
                CreateParameter("FirstName", (object)searchEntity.WebCooperatorFirstName ?? DBNull.Value, true),
                CreateParameter("LastName", (object)searchEntity.WebCooperatorLastName ?? DBNull.Value, true),
                CreateParameter("Organization", (object)searchEntity.WebCooperatorOrganization ?? DBNull.Value, true),
                CreateParameter("EmailAddress", (object)searchEntity.WebCooperatorEmailAddress ?? DBNull.Value, true),
                CreateParameter("IntendedUseCode", (object)searchEntity.IntendedUseCode ?? DBNull.Value, true),
                CreateParameter("StatusCode", (object)searchEntity.StatusCode ?? DBNull.Value, true),
                CreateParameter("Year", searchEntity.Year > 0 ? (object)searchEntity.Year : DBNull.Value, true),
                CreateParameter("IsLocked", (object)searchEntity.IsLocked ?? DBNull.Value, true),
            };

            results = GetRecords<WebOrderRequest>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public List<WebOrderRequestItem> GetWebOrderRequestItems(int entityId)
        {
            SQL = "usp_GGTools_Orders_WebOrderRequestItems_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("web_order_request_id", (object)entityId, false)
            };
            List<WebOrderRequestItem> webOrderRequestItems = GetRecords<WebOrderRequestItem>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return webOrderRequestItems;
        }
        public List<WebOrderRequestAction> GetWebOrderRequestActions(int entityId)
        {
            SQL = "usp_GGTools_Orders_WebOrderRequestActions_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("web_order_request_id", (object)entityId, false)
            };
            List<WebOrderRequestAction> webOrderRequestActions = GetRecords<WebOrderRequestAction>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return webOrderRequestActions;
        }

        public CodeValue GetWebOrderRequestEmailAddresses(int entityId)
        {
            SQL = "usp_GGTools_Orders_WebOrderRequestEmailAddresses_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("web_order_request_id", (object)entityId, false)
            };
            //AddParameter("out_email_recipients", -1, true, System.Data.DbType.String, System.Data.ParameterDirection.Output);
            CodeValue siteEmailDetail = GetRecord<CodeValue>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return siteEmailDetail;
        }

        public List<ReportItem> GetReportItems(WebOrderRequestSearch searchEntity)
        {
            SQL = "usp_GRINGlobalRptStatusPercentage";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("year", (object)searchEntity.Year, false)
            };
            List<ReportItem> reportItems = GetRecords<ReportItem>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return reportItems;
        }

        //public List<ReportItem> GetCooperatorTotals(string searchText)
        //{
        //    SQL = "SELECT CommitteeMember, " +
        //        " StatusCode," +
        //        " COUNT(*) AS TotalOrders, " +
        //        " FROM vw_GGTools_Orders_Rpt_ProcessedByUser " +
        //        " WHERE CommitteeMember LIKE @Name " +
        //        " GROUP BY CommitteeMember, StatusCode ";
        //    var parameters = new List<IDbDataParameter> {
        //        CreateParameter("FirstName", (object)searchText ?? DBNull.Value, true)
        //    };
        //    results = GetRecords<ReportItem>(SQL, parameters.ToArray());
        //    RowsAffected = results.Count;
        //    return results;
        //}

        public int Update(WebOrderRequest entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<WebOrderRequest>(entity);

            SQL = "usp_GGTools_Orders_WebOrderRequest_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }

        public int UpdateLockStatus(WebOrderRequest entity)
        {
            WebOrderRequest webOrderRequest = new WebOrderRequest();
            webOrderRequest = Get(entity.ID);
            //TODO Set lock attrib
            webOrderRequest.IsLocked = true;
            Update(webOrderRequest);
            return 0;
        }

        public int UpdateWebCooperatorStatus(WebCooperator entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<WebCooperator>(entity);

            SQL = "usp_GGTools_Orders_WebCooperatorStatus_Update";

            AddParameter("web_cooperator_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            AddParameter("vetted_status_code", String.IsNullOrEmpty(entity.VettedStatusCode) ? DBNull.Value : (object)entity.VettedStatusCode, true);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, true);
            AddParameter("modified_by_web_user_id", entity.ModifiedByWebUserID == 0 ? DBNull.Value : (object)entity.ModifiedByWebUserID, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }

        public void BuildInsertUpdateParameters(WebOrderRequest entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("web_order_request_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("web_user_id", entity.WebUserID == 0 ? DBNull.Value : (object)entity.WebUserID, true);
            AddParameter("status_code", String.IsNullOrEmpty(entity.StatusCode) ? DBNull.Value : (object)entity.StatusCode, true);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, true);
        }

        public virtual int Count(int year)
        {
            // Submit SQL to count all records in a table
            return CountRecords("SELECT COUNT(*) FROM vw_gringlobal_web_order_requests WHERE YEAR(OrderDate) = " + year.ToString());
        }

        public List<CodeValue> GetTimeFrameOptions()
        {
            List<CodeValue> timeFrameOptions = new List<CodeValue>();
            timeFrameOptions.Add(new CodeValue { Value = "TDY", Title = "Today" });
            timeFrameOptions.Add(new CodeValue { Value = "3DY", Title = "Within the last 3 days" });
            timeFrameOptions.Add(new CodeValue { Value = "7DY", Title = "Within the last 7 days" });
            timeFrameOptions.Add(new CodeValue { Value = "30D", Title = "Within the last 30 days" });
            timeFrameOptions.Add(new CodeValue { Value = "60D", Title = "Within the last 60 days" });
            return timeFrameOptions;
        }

        public List<WebCooperator> GetWebCooperators()
        {
            SQL = "usp_GGTools_Orders_WebCooperators_Select";
            List<WebCooperator> webCooperators = GetRecords<WebCooperator>(SQL, CommandType.StoredProcedure);
            RowsAffected = webCooperators.Count;
            return webCooperators;
        }

        public virtual List<CodeValue> GetCodeValues(string groupName)
        {
            SQL = "usp_GGTools_GRINGlobal_CodeValuesByGroup_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("group_name", (object)groupName, false)
            };
            List<CodeValue> codeValues = GetRecords<CodeValue>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return codeValues;
        }
    }
}
