using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;


namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class WebCooperatorManager : AppDataManagerBase, IManager<WebCooperator, WebCooperatorSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(WebCooperator entity)
        {
            throw new NotImplementedException();
        }

        public WebCooperator Get(int entityId)
        {
            WebCooperatorSearch webCooperatorSearch = new WebCooperatorSearch();
            webCooperatorSearch.ID = entityId;
            return Search(webCooperatorSearch)[0];
        }

        public int Insert(WebCooperator entity)
        {
            throw new NotImplementedException();
        }

        public List<WebCooperator> Search(WebCooperatorSearch searchEntity)
        {
            List<WebCooperator> results = new List<WebCooperator>();

            SQL = " SELECT * FROM vw_GGTools_GRINGlobal_WebCooperators ";
            SQL += " WHERE (@FirstName      IS NULL     OR      FirstName          LIKE        '%' + @FirstName + '%')";
            SQL += " AND (@LastName         IS NULL     OR      LastName           LIKE        '%' + @LastName + '%')";
            SQL += " AND (@Organization     IS NULL     OR      Organization       LIKE        '%' + @Organization + '%')";
            SQL += " AND (@ID               IS NULL     OR      WebCooperatorID    =            @ID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FirstName", (object)searchEntity.FirstName ?? DBNull.Value, true),
                CreateParameter("LastName", (object)searchEntity.LastName ?? DBNull.Value, true),
                CreateParameter("Organization", (object)searchEntity.Organization ?? DBNull.Value, true),
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true)
            };
            
            results = GetRecords<WebCooperator>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        
        public int Update(WebCooperator entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<WebCooperator>(entity);

            SQL = "usp_GGTools_Orders_WebCooperatorStatus_Update";

            AddParameter("web_cooperator_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            AddParameter("vetted_status_code", String.IsNullOrEmpty(entity.VettedStatusCode) ? DBNull.Value : (object)entity.VettedStatusCode, true);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, true);
            AddParameter("modified_by_web_user_id", entity.ModifiedByWebUserID == 0 ? DBNull.Value : (object)entity.ModifiedByWebUserID, true);
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
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
