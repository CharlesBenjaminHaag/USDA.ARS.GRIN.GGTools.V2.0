using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class EconomicUsageTypeManager : GRINGlobalDataManagerBase, IManager<EconomicUsageType, EconomicUsageTypeSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(EconomicUsageType entity)
        {
            throw new NotImplementedException();
        }

        public EconomicUsageType Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(EconomicUsageType entity)
        {
            throw new NotImplementedException();
        }

        public List<EconomicUsageType> Search(EconomicUsageTypeSearch searchEntity)
        {
            List<EconomicUsageType> results = new List<EconomicUsageType>();

            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Economic_Usage_Type ";
            SQL += " WHERE  (@CreatedByCooperatorID     IS NULL     OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND    (@ID                        IS NULL     OR ID                       =       @ID)";
            SQL += " AND    (@EconomicUsageCode         IS NULL     OR EconomicUsageCode        LIKE   '%' + @EconomicUsageCode + '%')";
            SQL += " AND    (@UsageType                 IS NULL     OR UsageType                LIKE   '%' + @UsageType + '%')";
     
            var parameters = new List<IDbDataParameter> {
            CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
            CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
            CreateParameter("EconomicUsageCode", (object)searchEntity.EconomicUsageCode ?? DBNull.Value, true),
            CreateParameter("UsageType", (object)searchEntity.UsageType ?? DBNull.Value, true),
        };

            results = GetRecords<EconomicUsageType>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public int Update(EconomicUsageType entity)
        {
            throw new NotImplementedException();
        }
    }
}
