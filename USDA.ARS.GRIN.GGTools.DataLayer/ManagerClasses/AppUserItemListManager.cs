using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer 
{
    public class AppUserItemListManager : AppDataManagerBase, IManager<AppUserItemList, AppUserItemListSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(AppUserItemList entity)
        {
            throw new NotImplementedException();
        }

        public AppUserItemList Get(int entityId)
        {
            throw new NotImplementedException();
        }
        public List<AppUserItemList> GetTabList(int cooperatorId)
        {
            SQL = "usp_GRINGlobal_AppUserItemListTabs_Select";
            List<AppUserItemList> appUserItemLists = new List<AppUserItemList>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false)
            };

            appUserItemLists = GetRecords<AppUserItemList>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return appUserItemLists;
        }

        public List<AppEntityRecord> GetListsByTab(int cooperatorId, string tabName)
        {
            SQL = "usp_GRINGlobal_AppUserItemListsByTab_Select";
            List<AppEntityRecord> appUserItemLists = new List<AppEntityRecord>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false),
                CreateParameter("tab_name", (object)tabName, false)
            };

            appUserItemLists = GetRecords<AppEntityRecord>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return appUserItemLists;
        }
        public List<AppUserItemList> GetItemsByList(int cooperatorId, string listName)
        {
            SQL = "usp_GRINGlobal_AppUserItemListItemsByList_Select";
            List<AppUserItemList> appUserItemLists = new List<AppUserItemList>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false),
                CreateParameter("list_name", (object)listName, false)
            };

            appUserItemLists = GetRecords<AppUserItemList>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return appUserItemLists;
        }
        public List<AppUserItemList> Search(AppUserItemListSearch search)
        {
            SQL = "SELECT * FROM  vw_GRINGlobal_App_User_Item_List ";
            SQL += " WHERE  (@CreatedByCooperatorID     IS NULL OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND    (@AppUserItemFolderID       IS NULL OR AppUserItemFolderID      =       @AppUserItemFolderID)";
            SQL += " AND    (@ID                        IS NULL OR ID                       =       @ID)";
            SQL += " AND    (@ListName                  IS NULL OR ListName                 LIKE    '%' + @ListName + '%')";
    
            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", search.CreatedByCooperatorID > 0 ? (object)search.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("AppUserItemFolderID", search.AppUserItemFolderID > 0 ? (object)search.AppUserItemFolderID : DBNull.Value, true),
                CreateParameter("ID", search.ID > 0 ? (object)search.ID : DBNull.Value, true),
                CreateParameter("ListName", (object)search.ListName ?? DBNull.Value, true),
            };
            List<AppUserItemList> appUserItemLists = GetRecords<AppUserItemList>(SQL, parameters.ToArray());
            RowsAffected = appUserItemLists.Count;
            return appUserItemLists;
        }

        public int Update(AppUserItemList entity)
        {
            throw new NotImplementedException();
        }

        public int Insert(AppUserItemList entity)
        {
             
                int rowsAffected = 0;

                Reset(CommandType.StoredProcedure);

                SQL = "usp_GRINGlobal_App_User_Item_List_Insert";

                AddParameter("app_user_item_folder_id", (object)entity.AppUserItemFolderID, false);
                AddParameter("cooperator_id", (object)entity.CreatedByCooperatorID, false);
                AddParameter("tab_name", (object)entity.TabName, false);
                AddParameter("list_name", (object)entity.ListName, false);
                AddParameter("id_number", (object)entity.IDNumber, false);
                AddParameter("id_type", (object)entity.IDType.Replace("_ID", ""), false);
                AddParameter("sort_order", (object)entity.IDNumber, false);
                AddParameter("title", (object)entity.ListName, false);
                AddParameter("description", (object)entity.Description, false);
                AddParameter("created_by", (object)entity.CreatedByCooperatorID ?? DBNull.Value, true);

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
         
    }
}
