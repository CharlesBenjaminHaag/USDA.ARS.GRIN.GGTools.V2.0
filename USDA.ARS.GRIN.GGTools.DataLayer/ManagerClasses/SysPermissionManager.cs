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

            SQL = " SELECT * FROM vw_GGTools_GRINGlobal_SysPermissions";
            SQL += " WHERE    (@SysGroupID        IS NULL OR  SysGroupID      = @SysGroupID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("SysGroupID", searchEntity.SysGroupID > 0 ? (object)searchEntity.SysGroupID : DBNull.Value, true)
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
