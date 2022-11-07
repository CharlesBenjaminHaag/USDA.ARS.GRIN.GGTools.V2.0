using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer 
{
    public class SysPermissionManager : AppDataManagerBase, IManager<SysPermission, SysPermissionSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(SysPermission entity)
        {
            throw new NotImplementedException();
        }

        public SysPermission Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(SysPermission entity)
        {
            throw new NotImplementedException();
        }

        public List<SysPermission> Search(SysPermissionSearch searchEntity)
        {
            List<SysPermission> sysPermissions = new List<SysPermission>();

            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Permission";
            SQL += " WHERE    (@TableName        IS NULL OR  TableName      = @TableName)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("TableName", !String.IsNullOrEmpty(searchEntity.TableName) ? (object)searchEntity.TableName : DBNull.Value, true)
            };

            sysPermissions = GetRecords<SysPermission>(SQL, parameters.ToArray());
            parameters.Clear();
            return sysPermissions;
        }

        public int Update(SysPermission entity)
        {
            throw new NotImplementedException();
        }
    }
}
