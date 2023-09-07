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

        public int Insert(AppUserItemList entity)
        {
            throw new NotImplementedException();
        }

        public List<AppUserItemList> Search(AppUserItemListSearch search)
        {
            SQL = "SELECT * FROM  vw_GRINGlobal_App_User_Item_List ";
            SQL += " WHERE  (@CreatedByCooperatorID     IS NULL OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND    (@ID                        IS NULL OR ID                       =       @ID)";
            SQL += " AND    (@ListName                  IS NULL OR ListName                 LIKE    '%' + @ListName + '%')";
    
            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", search.CreatedByCooperatorID > 0 ? (object)search.CreatedByCooperatorID : DBNull.Value, true),
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
    }
}
