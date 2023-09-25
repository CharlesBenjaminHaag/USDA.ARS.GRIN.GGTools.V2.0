using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Configuration;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysTableManager : AppDataManagerBase, IManager<SysTable, SysTableSearch>
    {
        public void BuildInsertUpdateParameters()
        {
         
        }

        public int Delete(SysTable entity)
        {
            throw new NotImplementedException();
        }

        public SysTable Get(int siteId)
        {
            SQL = "usp_GRINGlobal_SysTable_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("site_id", (object)siteId, false)
            };
            SysTable site = GetRecord<SysTable>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return site;
        }

        public int Insert(SysTable entity)
        {
            throw new NotImplementedException();
        }

        public List<SysTable> Search(SysTableSearch searchEntity)
        {
            List<SysTable> results = new List<SysTable>();

            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Table";
            SQL += " WHERE  (@ID                    IS NULL     OR ID                   =       @ID)";
            SQL += " AND    (@DatabaseAreaCode      IS NULL     OR DatabaseAreaCode     =       @DatabaseAreaCode)";

            // Temp (?)
            SQL += " AND TableTitle IS NOT NULL ";

            // Also temp
            SQL += " AND TableName <> 'taxonomy_family'";

            SQL += " ORDER BY TableTitle ";

             var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("DatabaseAreaCode", (object)searchEntity.DatabaseAreaCode ?? DBNull.Value, true),
            };

            results = GetRecords<SysTable>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public int Update(SysTable entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<SysTable>(entity);
            SQL = "usp_GRINGlobal_Sys_Table_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            RowsAffected = ExecuteNonQuery();

            return RowsAffected;
        }

        public virtual List<CodeValue> GetCodeValues(string groupName)
        {
            SQL = "usp_GRINGlobal_Code_Values_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("group_name", (object)groupName, false)
            };
            List<CodeValue> codeValues = GetRecords<CodeValue>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return codeValues;
        }

        protected virtual void BuildInsertUpdateParameters(SysTable entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("site_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            if (entity.ID > 0)
            {
                AddParameter("modified_by", entity.ModifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.ModifiedByCooperatorID, true);
            }
            else
            {
                AddParameter("created_by", entity.CreatedByCooperatorID == 0 ? DBNull.Value : (object)entity.CreatedByCooperatorID, true);
            }
        }
    }
}
