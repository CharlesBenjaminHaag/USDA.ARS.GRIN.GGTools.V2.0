using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class SysFolderManager : GRINGlobalDataManagerBase
    {
        public SysFolder Get(SysFolderSearch searchEntity)
        {
            SysFolder appUserItemFolder = new SysFolder();

            SQL = "usp_GRINGlobal_SysFolder_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("app_user_item_folder_id", (object)searchEntity.ID, false)
            };

            appUserItemFolder = GetRecord<SysFolder>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            
            return appUserItemFolder;
        }
      
        public virtual List<SysFolder> Search(SysFolderSearch searchEntity)
        {
            List<SysFolder> results = new List<SysFolder>();

            SQL = " SELECT * FROM vw_GRINGlobal_App_User_Item_Folder";
            SQL += " WHERE  (@FolderName                IS NULL OR   FolderName             LIKE    '%' + @FolderName + '%')";
            SQL += " AND    (@Category                  IS NULL OR   Category               =       @Category)";
            SQL += " AND    (@CreatedByCooperatorID     IS NULL OR   CreatedByCooperatorID  =       @CreatedByCooperatorID)";
            SQL += " AND    (@SysFolderID       IS NULL OR   ID                     =       @SysFolderID)";
            SQL += " AND    (@IsFavorite                IS NULL OR   IsFavorite             =       @IsFavorite)";
           SQL += " AND    (@FolderType                IS NULL OR   FolderType             =       @FolderType)";

            //if (searchEntity.IsShared == "Y")
            //{
            //    SQL += " AND ID IN (SELECT FolderID " +
            //            " FROM vw_GRINGlobal_App_User_Item_Folder_Cooperator_Map " +
            //            " WHERE CooperatorID = @SharedWithCooperatorID) ";
            //}

            //if (searchEntity.EntityID > 0)
            //{
            //    SQL += " AND ID IN (SELECT SysFolderID " +
            //            " FROM vw_GRINGlobal_App_User_Item_List " +
            //            " WHERE IDNumber = @EntityID) ";
            //}

            //switch (searchEntity.TimeFrame)
            //{
            //    case "1D":
            //        SQL += " AND (CONVERT(date, CreatedDate) = CONVERT(date, GETDATE()))";
            //        break;
            //    case "3D":
            //        SQL += " AND  CreatedDate >= DATEADD(day,-3, GETDATE())";
            //        break;
            //    case "7D":
            //        SQL += " AND  CreatedDate >= DATEADD(day,-7, GETDATE())";
            //        break;
            //    case "30D":
            //        SQL += " AND  CreatedDate >= DATEADD(day,-30, GETDATE())";
            //        break;
            //    case "60D":
            //        SQL += " AND  CreatedDate >= DATEADD(day,-60, GETDATE())";
            //        break;
            //    case "90D":
            //        SQL += " AND  CreatedDate >= DATEADD(day,-90, GETDATE())";
            //        break;
            //    case "YEAR":
            //        SQL += " AND  DATEPART(year, CreatedDate) = DATEPART(year, GETDATE())";
            //        break;
            //}

            var parameters = new List<IDbDataParameter> {
            //    CreateParameter("FolderName", !String.IsNullOrEmpty(searchEntity.FolderName) ? (object)searchEntity.FolderName : DBNull.Value, true),
            //    CreateParameter("Category", !String.IsNullOrEmpty(searchEntity.Category) ? (object)searchEntity.Category : DBNull.Value, true),
            //    CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
            //    CreateParameter("SharedWithCooperatorID", searchEntity.SharedWithCooperatorID > 0 ? (object)searchEntity.SharedWithCooperatorID : DBNull.Value, true),
            //    CreateParameter("IsFavorite", !String.IsNullOrEmpty(searchEntity.IsFavorite) ? (object)searchEntity.IsFavorite : DBNull.Value, true),
            //    CreateParameter("FolderType", !String.IsNullOrEmpty(searchEntity.FolderType) ? (object)searchEntity.FolderType : DBNull.Value, true),
            //    CreateParameter("SysFolderID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
            //    CreateParameter("EntityID", searchEntity.EntityID > 0 ? (object)searchEntity.EntityID : DBNull.Value, true),
            };

            results = GetRecords<SysFolder>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        
        public virtual int Insert(SysFolder entity)
        {
            int errorNumber = 0;

            Reset(CommandType.StoredProcedure);
            Validate<SysFolder>(entity);
            SQL = "usp_GRINGlobal_SysFolder_Insert";
            
            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_app_user_item_folder_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            entity.ID = GetParameterValue<int>("@out_app_user_item_folder_id", -1);
            return entity.ID;
        }
        
        public int Update(SysFolder entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<SysFolder>(entity);

            SQL = "usp_GRINGlobal_SysFolder_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }
        
        public int Delete(SysFolder entity)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_SysFolder_Delete";
            AddParameter("@app_user_item_folder_id", (object)entity.ID, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            return RowsAffected;
        }
        
        public int DeleteItem(int appUserItemListId)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_SysList_Delete";
            AddParameter("@app_user_item_list_id", (object)appUserItemListId, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            return RowsAffected;
        }
        
        public int DeleteItemByEntityID(int appUserItemFolderID, int idNumber)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_SysList_By_EntityID_Delete";
            AddParameter("@app_user_item_folder_id", (object)appUserItemFolderID, false);
            AddParameter("@id_number", (object)idNumber, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            return RowsAffected;
        }
        
        protected virtual void BuildInsertUpdateParameters(SysFolder entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("app_user_item_folder_id", (object)entity.ID, false);
            }

            AddParameter("title", (object)entity.Title, false);
            AddParameter("description", (object)entity.Description ?? DBNull.Value, true);
           
            if (entity.ID > 0)
            {
                AddParameter("modified_by", (object)entity.ModifiedByCooperatorID ?? DBNull.Value, true);
            }
            else 
            {
                AddParameter("created_by", (object)entity.CreatedByCooperatorID ?? DBNull.Value, true);
            }
        }
    }
}
