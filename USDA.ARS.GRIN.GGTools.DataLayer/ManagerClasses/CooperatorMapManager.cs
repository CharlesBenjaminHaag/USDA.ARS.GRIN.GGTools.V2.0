﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Linq;
using System.Configuration;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class CooperatorMapManager : AppDataManagerBase, IManager<CooperatorMap, CooperatorMapSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(CooperatorMap entity)
        {
            throw new NotImplementedException();
        }

        public CooperatorMap Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(CooperatorMap entity)
        {
            throw new NotImplementedException();
        }

        public List<CooperatorMap> Search(CooperatorMapSearch searchEntity)
        {
            List<CooperatorMap> results = new List<CooperatorMap>();
                
            SQL = " SELECT * FROM vw_GRINGlobal_Cooperator_Map";
            SQL += " WHERE (@ID                     IS NULL     OR ID                       =       @ID)";
            SQL += " AND (@CooperatorName           IS NULL     OR CooperatorName           LIKE    '%' + @CooperatorName + '%')";
            SQL += " AND (@GroupTag                 IS NULL     OR GroupTag                 LIKE    '%' + @GroupTag + '%')";
            SQL += " AND (@CreatedByCooperatorID    IS NULL     OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND (@ModifiedByCooperatorID   IS NULL     OR ModifiedByCooperatorID   =       @ModifiedByCooperatorID)";
            SQL += " AND (@OwnedByCooperatorID      IS NULL     OR OwnedByCooperatorID      =       @OwnedByCooperatorID)";
            SQL += " ORDER BY CooperatorName, GroupTag";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CooperatorName", (object)searchEntity.CooperatorName ?? DBNull.Value, true),
                CreateParameter("GroupTag", (object)searchEntity.GroupTag ?? DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("ModifiedByCooperatorID", searchEntity.ModifiedByCooperatorID > 0 ? (object)searchEntity.ModifiedByCooperatorID : DBNull.Value, true),                
                CreateParameter("OwnedByCooperatorID", searchEntity.OwnedByCooperatorID > 0 ? (object)searchEntity.OwnedByCooperatorID : DBNull.Value, true),                
            };

            results = GetRecords<CooperatorMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public int Update(CooperatorMap entity)
        {
            throw new NotImplementedException();
        }
    }
}