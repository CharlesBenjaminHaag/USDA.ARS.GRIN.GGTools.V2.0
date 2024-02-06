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

        public int InsertAccessionInvAnnotation(AccessionInvAnnotation entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<AccessionInvAnnotation>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Accession_Inv_Annotation_Insert";
        
            AddParameter("taxonomy_species_id", entity.SpeciesID == 0 ? DBNull.Value : (object)entity.SpeciesID, true);
            AddParameter("current_taxonomy_species_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("modified_id", entity.ModifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.ModifiedByCooperatorID, true);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_accession_inv_annotation_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

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
    }
}
