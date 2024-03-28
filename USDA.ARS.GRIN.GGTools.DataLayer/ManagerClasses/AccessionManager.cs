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
            //Reset(CommandType.StoredProcedure);
            //Validate<Species>(entity);
            //SQL = "usp_GRINGlobal_Accession_Update";

            ////BuildInsertUpdateParameters(entity);

            //AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            //int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            //RowsAffected = ExecuteNonQuery();

            return RowsAffected;
        }

        public int UpdateBySpecies(Accession entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Accession>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Accession_By_Species_Update";

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("taxonomy_species_id", entity.SpeciesID == 0 ? DBNull.Value : (object)entity.SpeciesID, true);
            AddParameter("new_taxonomy_species_id", entity.NewSpeciesID == 0 ? DBNull.Value : (object)entity.NewSpeciesID, true);
            AddParameter("modified_by", entity.ModifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.ModifiedByCooperatorID, true);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_accession_inv_annotation_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            RowsAffected = entity.ID;
            return entity.ID;
        }
    }
}
