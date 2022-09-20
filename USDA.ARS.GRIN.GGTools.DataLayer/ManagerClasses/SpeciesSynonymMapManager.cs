using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class SpeciesSynonymMapManager : AppDataManagerBase, IManager<SpeciesSynonymMap, SpeciesSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(SpeciesSynonymMap entity)
        {
            throw new NotImplementedException();
        }

        public SpeciesSynonymMap Get(int entityId)
        {
            SpeciesSynonymMap result = new SpeciesSynonymMap();
            SQL = " SELECT * FROM vw_GGTools_Taxon_SpeciesSynonymMaps ";
            SQL += " WHERE      (@ID                        IS NULL OR  ID = @ID) ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", entityId, true),
            };

            result = GetRecord<SpeciesSynonymMap>(SQL, parameters.ToArray());
            return result;
        }

        public int Insert(SpeciesSynonymMap entity)
        {
            int errorNumber = 0;

            Reset(CommandType.StoredProcedure);
            Validate<SpeciesSynonymMap>(entity);
            SQL = "usp_GGTools_Taxon_SpeciesSynonymMap_Insert";

            AddParameter("taxonomy_species_id_subject", (object)entity.SpeciesIDSubject, false);
            AddParameter("taxonomy_species_id_predicate", (object)entity.SpeciesIDPredicate, false);
            AddParameter("synonym_code", (object)entity.SynonymCode ?? DBNull.Value, false);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_species_synonym_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            entity.ID = GetParameterValue<int>("@out_taxonomy_species_synonym_map_id", -1);
            return entity.ID;
        }

        public List<SpeciesSynonymMap> Search(SpeciesSynonymMapSearch searchEntity)
        {
            List<SpeciesSynonymMap> results = new List<SpeciesSynonymMap>();

            SQL = " SELECT * FROM vw_GGTools_Taxon_SpeciesSynonymMaps ";
            SQL += " WHERE      (@ID                        IS NULL OR  ID = @ID) ";
            SQL += " AND        ((@SpeciesNameSubject   IS NULL OR  REPLACE(SpeciesNameSubject, ' x ', '')  LIKE    'X ' + @SpeciesNameSubject + '%')";
            SQL += " OR         (@SpeciesNameSubject    IS NULL OR  REPLACE(SpeciesNameSubject, ' x ', '')  LIKE    '+' + @SpeciesNameSubject + '%')";
            SQL += " OR         (@SpeciesNameSubject    IS NULL OR  REPLACE(SpeciesNameSubject, ' x ', '')  LIKE    @SpeciesNameSubject + '%'))";
            SQL += " AND        (@SynonymCode               IS NULL OR  SynonymCode = @SynonymCode)";
            SQL += " AND        ((@SpeciesNamePredicate IS NULL OR  REPLACE(SpeciesNamePredicate, ' x ', '')  LIKE    'X ' + @SpeciesNamePredicate + '%')";
            SQL += " OR         (@SpeciesNamePredicate  IS NULL OR  REPLACE(SpeciesNamePredicate, ' x ', '')  LIKE    '+' + @SpeciesNamePredicate + '%')";
            SQL += " OR         (@SpeciesNamePredicate    IS NULL OR  REPLACE(SpeciesNamePredicate, ' x ', '')  LIKE    @SpeciesNamePredicate + '%'))";

            if (!String.IsNullOrEmpty(searchEntity.IDList))
            {
                SQL += " AND (SpeciesIDSubject IN (" + searchEntity.IDList + "))";
            }

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("SpeciesNameSubject", (object)searchEntity.SpeciesNameSubject ?? DBNull.Value, true),
                CreateParameter("SynonymCode", (object)searchEntity.SynonymCode ?? DBNull.Value, true),
                CreateParameter("SpeciesNamePredicate", (object)searchEntity.SpeciesNamePredicate ?? DBNull.Value, true),
            };

            results = GetRecords<SpeciesSynonymMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<SpeciesSynonymMap> Search(SpeciesSearch searchEntity)
        {
            throw new NotImplementedException();
        }

        public int Update(SpeciesSynonymMap entity)
        {
            throw new NotImplementedException();
        }
    }
}
