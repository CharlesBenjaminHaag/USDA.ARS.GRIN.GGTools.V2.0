using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public partial class FolderManager : AppDataManagerBase, IManager<AppUserItemFolder, FolderSearch>
    {
        public AppUserItemFolder Get(FolderSearch searchEntity)
        {
            AppUserItemFolder appUserItemFolder = new AppUserItemFolder();

            SQL = "usp_GRINGlobal_AppUserItemFolder_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("app_user_item_folder_id", (object)searchEntity.ID, false)
            };

            appUserItemFolder = GetRecord<AppUserItemFolder>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            
            return appUserItemFolder;
        }

        public virtual List<AppUserItemFolder> Search(FolderSearch searchEntity)
        {
            List<AppUserItemFolder> results = new List<AppUserItemFolder>();

            SQL = " SELECT * FROM vw_GRINGlobal_App_User_Item_Folder"; 
            SQL += " WHERE  (@FolderType                IS NULL OR   FolderTypeDescription      =   @FolderType)";
            SQL += " AND    (@Category                  IS NULL OR   Category                   =   @Category)";
            SQL += " AND    (@CreatedByCooperatorID     IS NULL OR   CreatedByCooperatorID      =   @CreatedByCooperatorID)";
            SQL += " AND    (@AppUserItemFolderID       IS NULL OR   ID                         =   @AppUserItemFolderID)";
            SQL += " AND    (@IsFavorite                IS NULL OR   IsFavorite                 =   @IsFavorite)";

            if (searchEntity.IsShared == true)
            {
                SQL += " AND ID IN (SELECT FolderID " +
                        " FROM vw_GRINGlobal_App_User_Item_Folder_Cooperator_Map " +
                        " WHERE CooperatorID = @SharedWithCooperatorID) ";
            }

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderType", !String.IsNullOrEmpty(searchEntity.FolderTypeDescription) ? (object)searchEntity.FolderTypeDescription : DBNull.Value, true),
                CreateParameter("Category", !String.IsNullOrEmpty(searchEntity.Category) ? (object)searchEntity.Category : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("SharedWithCooperatorID", searchEntity.SharedWithCooperatorID > 0 ? (object)searchEntity.SharedWithCooperatorID : DBNull.Value, true),
                CreateParameter("IsFavorite", !String.IsNullOrEmpty(searchEntity.IsFavorite) ? (object)searchEntity.IsFavorite : DBNull.Value, true),
                CreateParameter("AppUserItemFolderID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
            };

            results = GetRecords<AppUserItemFolder>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public virtual List<AppUserItemFolder> GetMySharedFolders(FolderSearch searchEntity)
        {
            List<AppUserItemFolder> results = new List<AppUserItemFolder>();

            SQL = " SELECT * FROM vw_GRINGlobal_App_User_Item_Folder";
            SQL += " WHERE  (@FolderType                IS NULL OR   FolderTypeDescription      =   @FolderType)";
            SQL += " AND    (@Category                  IS NULL OR   Category                   =   @Category)";
            SQL += " AND    (@CreatedByCooperatorID     IS NULL OR   CreatedByCooperatorID      =   @CreatedByCooperatorID)";
            SQL += " AND    (@AppUserItemFolderID       IS NULL OR   ID                         =   @AppUserItemFolderID)";
            SQL += " AND    (@IsFavoriteOption          IS NULL OR   IsFavoriteOption           =   @IsFavoriteOption)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderType", !String.IsNullOrEmpty(searchEntity.FolderTypeDescription) ? (object)searchEntity.FolderTypeDescription : DBNull.Value, true),
                CreateParameter("Category", !String.IsNullOrEmpty(searchEntity.Category) ? (object)searchEntity.Category : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("IsFavorite", searchEntity.IsFavorite == "Y" ? (object)searchEntity.IsFavorite : DBNull.Value, true),
                CreateParameter("AppUserItemFolderID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
            };

            results = GetRecords<AppUserItemFolder>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public virtual List<AppUserItemFolder> GetRelatedFolders(FolderSearch searchEntity)
        {
            List<AppUserItemFolder> results = new List<AppUserItemFolder>();

            SQL =   " SELECT DISTINCT FolderID, FolderName " +
                    " FROM vw_GRINGlobal_Folder_" + searchEntity.TableName + " WHERE CreatedByCooperatorID = @CreatedByCooperatorID";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID, true)
            };
            results = GetRecords<AppUserItemFolder>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public virtual List<AppUserItemFolder> GetAvailableFolders(int cooperatorId, string tableName)
        {
            List<AppUserItemFolder> results = new List<AppUserItemFolder>();

            SQL = " SELECT * FROM vw_GRINGlobal_App_User_Item_Folder";
            SQL += " WHERE CreatedByCooperatorID = @CreatedByCooperatorID ";
            SQL += " AND FolderType = @FolderType";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", cooperatorId, true),
                CreateParameter("FolderType", (object)tableName ?? DBNull.Value, true),
            };
            results = GetRecords<AppUserItemFolder>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public virtual List<CodeValue> GetFolderTypes(int cooperatorId = 0)
        {
            List<CodeValue> codeValues = new List<CodeValue>();
            FolderSearch searchEntity = new FolderSearch { CreatedByCooperatorID = cooperatorId };

            SQL = " SELECT DISTINCT FolderType AS Value, " +
                "   FolderTypeDescription AS Title " +
                "   FROM vw_GRINGlobal_App_User_Item_Folder ";
            SQL += " WHERE (@CreatedByCooperatorID IS NULL OR CreatedByCooperatorID =  @CreatedByCooperatorID)";
            SQL += " GROUP BY FolderType, FolderTypeDescription  ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true)
            };

            codeValues = GetRecords<CodeValue>(SQL, parameters.ToArray());
            return codeValues;
        }

        //public virtual List<CodeValue> GetFolderLists(int cooperatorId)
        //{
        //    List<CodeValue> codeValues = new List<CodeValue>();
        //    FolderSearch searchEntity = new FolderSearch { CreatedByCooperatorID = cooperatorId };

        //    SQL =   " SELECT ListName AS Value, " +
        //            " CONVERT(NVARCHAR, COUNT(*)) AS Title " + 
        //            " FROM vw_GGTools_GRINGlobal_AppUserItemLists " + 
        //            " WHERE TableName <> 'FOLDER' " +
        //            " AND CooperatorID = @CooperatorID " +
        //            " GROUP BY ListName ";
        //    var parameters = new List<IDbDataParameter> {
        //        CreateParameter("CooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true)
        //    };

        //    codeValues = GetRecords<CodeValue>(SQL, parameters.ToArray());
        //    return codeValues;
        //}

        public virtual List<CodeValue> GetFolderCategories(int cooperatorId = 0)
        {
            List<CodeValue> codeValues = new List<CodeValue>();
            FolderSearch searchEntity = new FolderSearch { CreatedByCooperatorID = cooperatorId };

            SQL = " SELECT DISTINCT Category AS Value, " +
                  " Category AS Title " +
                  " FROM vw_GRINGlobal_App_User_Item_Folder ";
            SQL += " WHERE (@CreatedByCooperatorID IS NULL OR CreatedByCooperatorID =  @CreatedByCooperatorID)";
            SQL += " GROUP BY Category  ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true)
            };

            codeValues = GetRecords<CodeValue>(SQL, parameters.ToArray());
           
            return codeValues;
        }

        public virtual List<AppUserItemList> GetFolderItems(int folderId)
        {
            List<AppUserItemList> appUserFolderItems = new List<AppUserItemList>();

            SQL = "usp_GGTools_Taxon_AppUserItemList_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("app_user_item_folder_id", (object)folderId, false)
            };

            appUserFolderItems = GetRecords<AppUserItemList>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return appUserFolderItems;
        }

        public virtual int Insert(AppUserItemFolder entity)
        {
            int newFolderId = 0;
            int errorNumber = 0;

            Reset(CommandType.StoredProcedure);
            Validate<AppUserItemFolder>(entity);
            SQL = "usp_GRINGlobal_AppUserItemFolder_Insert";
            
            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_app_user_item_folder_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            newFolderId = GetParameterValue<int>("@out_app_user_item_folder_id", -1);

            entity.ID = newFolderId;

            if (!String.IsNullOrEmpty(entity.ItemIDList))
            {
                InsertItems(entity);
            }
            return RowsAffected;
        }

        public int InsertItems(AppUserItemFolder entity)
        {
            string[] idCollection;
            idCollection = entity.ItemIDList.Split(',');
            foreach (var id in idCollection) 
            {
                AppUserItemList appUserItemList = new AppUserItemList();
                appUserItemList.AppUserItemFolderID = entity.ID;
                appUserItemList.CreatedByCooperatorID = entity.CreatedByCooperatorID;
                appUserItemList.IDNumber = Int32.Parse(id);
                appUserItemList.IDType = entity.FolderType;
                //appUserItemList.ListName = entity.FolderName;
                InsertItem(appUserItemList);
            }
            return 0;
        }

        public int ImportItems(AppUserItemFolder entity)
        {
            string[] idCollection;
            idCollection = entity.ItemIDList.Split(',');
            foreach (var id in idCollection)
            {
                AppUserItemList appUserItemList = new AppUserItemList();
                appUserItemList.ID = Int32.Parse(id);
                appUserItemList.AppUserItemFolderID = entity.ID;
                appUserItemList.CreatedByCooperatorID = entity.CreatedByCooperatorID;
                appUserItemList.IDNumber = Int32.Parse(id);
                appUserItemList.IDType = entity.FolderType;
                //appUserItemList.ListName = entity.FolderName;
                ImportItem(appUserItemList);
            }
            return 0;
        }

        private int InsertItem(AppUserItemList appUserItemList)
        {
            int rowsAffected = 0;

            Reset(CommandType.StoredProcedure);

            SQL = "usp_GGTools_GRINGlobal_AppUserItemList_Insert ";

            AddParameter("app_user_item_folder_id", (object)appUserItemList.AppUserItemFolderID, false);
            AddParameter("cooperator_id", (object)appUserItemList.CreatedByCooperatorID, false);
            //AddParameter("folder_name", (object)appUserItemList.ListName, false);
            AddParameter("item_id", (object)appUserItemList.IDNumber, false);
            AddParameter("folder_type", (object)appUserItemList.IDType.Replace("_ID",""), false);
            AddParameter("created_by", (object)appUserItemList.CreatedByCooperatorID ?? DBNull.Value, true);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_app_user_item_list_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            rowsAffected = ExecuteNonQuery();

            RowsAffected = GetParameterValue<int>("@out_app_user_item_list_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            return rowsAffected;
        }
        private int ImportItem(AppUserItemList appUserItemList)
        {
            int rowsAffected = 0;

            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_AppUserItemFolderListMap_Insert";

            AddParameter("app_user_item_folder_id", (object)appUserItemList.AppUserItemFolderID, false);
            AddParameter("app_user_item_list_id", (object)appUserItemList.ID, false);
            AddParameter("cooperator_id", (object)appUserItemList.CreatedByCooperatorID, false);
            AddParameter("created_by", (object)appUserItemList.CreatedByCooperatorID ?? DBNull.Value, true);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_app_user_item_folder_list_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            rowsAffected = ExecuteNonQuery();

            RowsAffected = GetParameterValue<int>("@out_app_user_item_folder_list_map_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            return rowsAffected;
        }

        public int Import(AppUserItemFolder entity)
        {
            int newFolderId = 0;
            int errorNumber = 0;

            Reset(CommandType.StoredProcedure);
            Validate<AppUserItemFolder>(entity);
            SQL = "usp_GRINGlobal_AppUserItemFolder_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_app_user_item_folder_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            newFolderId = GetParameterValue<int>("@out_app_user_item_folder_id", -1);

            entity.ID = newFolderId;

            if (!String.IsNullOrEmpty(entity.ItemIDList))
            {
                ImportItems(entity);
            }
            return RowsAffected;
        }

        protected virtual void BuildInsertUpdateParameters(AppUserItemFolder entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("app_user_item_folder_id", (object)entity.ID, false);
            }

            AddParameter("folder_name", (object)entity.FolderName, false);
            AddParameter("folder_type", (object)entity.FolderType, false);
            AddParameter("category", (object)entity.Category ?? DBNull.Value, true);
            AddParameter("description", (object)entity.Description ?? DBNull.Value, true);
            AddParameter("is_favorite", (object)entity.IsFavorite ?? DBNull.Value, true);

            if (entity.ID > 0)
            {
                AddParameter("modified_by", (object)entity.ModifiedByCooperatorID ?? DBNull.Value, true);
            }
            else 
            {
                AddParameter("created_by", (object)entity.CreatedByCooperatorID ?? DBNull.Value, true);
            }
        }

        public int Update(AppUserItemFolder entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<AppUserItemFolder>(entity);

            SQL = "usp_GRINGlobal_AppUserItemFolder_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }

        public int Delete(AppUserItemFolder entity)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GRINGlobal_AppUserItemFolder_Delete";
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

            SQL = "usp_GRINGlobal_AppUserItemList_Delete";
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
        public int DeleteCollaborator(int cooperatorId, int folderId)
        {
            Reset(CommandType.StoredProcedure);

            SQL = "usp_GGTools_GRINGlobal_AppUserItemFolderCooperatorMap_Delete";
            AddParameter("cooperator_id", (object)cooperatorId, false);
            AddParameter("app_user_item_folder_id", (object)folderId, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            return RowsAffected;
        }


        public Folder Get(int entityId)
        {
            SQL = "SELECT * FROM vw_GGTools_Taxon_Folders " +
                " WHERE ID = @app_user_item_folder_id";
            Folder folder = new Folder();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("@app_user_item_folder_id", (object)entityId, false)
            };
            folder = GetRecord<Folder>(SQL, parameters.ToArray());
            return folder;
        }

        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }
        public List<Cooperator> GetAvailableCooperators(int folderId)
        {
            List<Cooperator> cooperators = new List<Cooperator>();
            SQL = "usp_GRINGlobal_AppUserItemFolderAvailableCooperators_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("app_user_item_folder_id", (object)folderId ?? DBNull.Value, true)
            };
            cooperators = GetRecords<Cooperator>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return cooperators;
        }

        public List<Cooperator> GetCurrentCollaborators(int folderId)
        {
            List<Cooperator> cooperators = new List<Cooperator>();
            SQL = "usp_GRINGlobal_AppUserItemFolderCurrentCooperators_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("app_user_item_folder_id", (object)folderId ?? DBNull.Value, true)
            };
            cooperators = GetRecords<Cooperator>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return cooperators;
        }
        
        public virtual List<Cooperator> GetCooperators(string tableName)
        {
            SQL = "usp_GGTools_GRINGlobal_CreatedByCooperators_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("table_name", (object)tableName, false)
            };
            List<Cooperator> cooperators = GetRecords<Cooperator>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            RowsAffected = cooperators.Count;
            return cooperators;
        }

        public int InsertCollaborator(int cooperatorId, int appUserItemFolderId)
        {
            int rowsAffected = 0;

            Reset(CommandType.StoredProcedure);

            SQL = "usp_GGTools_GRINGlobal_AppUserItemFolderCooperatorMap_Insert";

            AddParameter("app_user_item_folder_id", (object)appUserItemFolderId, false);
            AddParameter("cooperator_id", (object)cooperatorId, false);
            AddParameter("created_by", (object)48 ?? DBNull.Value, true);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_app_user_item_folder_cooperator_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            rowsAffected = ExecuteNonQuery();
            return rowsAffected;
        }
      
        AppUserItemFolder IManager<AppUserItemFolder, FolderSearch>.Get(int entityId)
        {
            throw new NotImplementedException();
        }

        List<AppUserItemFolder> IManager<AppUserItemFolder, FolderSearch>.Search(FolderSearch searchEntity)
        {
            throw new NotImplementedException();
        }
    }
}
