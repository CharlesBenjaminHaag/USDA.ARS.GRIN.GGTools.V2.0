using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class AccessionManager : GRINGlobalDataManagerBase
    {
        
    
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(Accession entity)
        {
            throw new NotImplementedException();
        }

        public Accession Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(Accession entity)
        {
            throw new NotImplementedException();
        }

        public List<Accession> Search(AccessionSearch searchEntity)
        {
            SQL = "usp_GRINGlobal_Accession_Search";
            List<Accession> accessions = new List<Accession>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("@CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("@SpeciesID", searchEntity.SpeciesID > 0 ? (object)searchEntity.SpeciesID : DBNull.Value, true),
            };
            accessions = GetRecords<Accession>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return accessions;
        }

        public int Update(Accession entity)
        {
            throw new NotImplementedException();
        }
    }
}
