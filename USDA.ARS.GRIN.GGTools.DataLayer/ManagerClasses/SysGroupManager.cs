using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;


namespace USDA.ARS.GRIN.GGTools.DataLayer 
{
    public class SysGroupManager : AppDataManagerBase, IManager<SysGroup, SysGroupSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(SysGroup entity)
        {
            throw new NotImplementedException();
        }

        public SysGroup Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(SysGroup entity)
        {
            throw new NotImplementedException();
        }

        public List<SysGroup> Search(SysGroupSearch searchEntity)
        {
            List<SysGroup> sysGroups = new List<SysGroup>();

            SQL = " SELECT * FROM vw_GRINGlobal_Sys_Group";
            SQL += " WHERE  (@ID            IS NULL OR  ID              = @ID)";
            SQL += " AND    (@GroupTag      IS NULL OR  GroupTag        = @GroupTag)";
            SQL += " ORDER BY GroupTag ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("GroupTag", (object)searchEntity.GroupTag ?? DBNull.Value, true),
            };

            sysGroups = GetRecords<SysGroup>(SQL, parameters.ToArray());
            parameters.Clear();
            return sysGroups;
        }

        public int Update(SysGroup entity)
        {
            throw new NotImplementedException();
        }
    }
}
