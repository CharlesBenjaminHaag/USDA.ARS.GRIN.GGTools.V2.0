using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class SpeciesManager : GRINGlobalDataManagerBase, IManager<Species, SpeciesSearch>
    {
        public int Insert(Species entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Species>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Species_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_species_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_species_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            RowsAffected = entity.ID;
            return RowsAffected;
        }

        public int Update(Species entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Species>(entity);
            SQL = "usp_GRINGlobal_Taxonomy_Species_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            RowsAffected = ExecuteNonQuery();

            return RowsAffected;
        }

        public int Delete(Species entity)
        {
            throw new NotImplementedException();
        }

        public Species Get(int id)
        {
            SQL = "usp_TaxonomySpecies_Select";
            Species species = new Species();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("taxonomy_species_id", (object)id, false)
            };

            species = GetRecord<Species>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return species;
        }

        public List<Species> GetConspecificTaxa(int? entityId)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Species_Conspecific_Select";
            List<Species> speciesList = new List<Species>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("taxonomy_species_id", (object)entityId, false)
            };

            speciesList = GetRecords<Species>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return speciesList;
        }
        public List<Species> GetInfraspecificAutonym(string genusName, string speciesName, string rank)
        {
            SQL = "usp_GRINGlobal_Taxonomy_Infraspecific_Autonym_Select";
            List<Species> speciesList = new List<Species>();

            var parameters = new List<IDbDataParameter> {
                CreateParameter("genus_name", (object)genusName, false),
                CreateParameter("species_name", (object)speciesName, false),           
                CreateParameter("rank", (object)rank, false)
             };

            speciesList = GetRecords<Species>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return speciesList;
        }

        public List<CodeValue> GetReportList()
        {
            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Rpt_List";

            List<CodeValue> reportList = new List<CodeValue>();

            reportList = GetRecords<CodeValue>(SQL);
            return reportList;
        }

        public List<Species> GetReport(string name)
        {
            SQL = "SELECT * FROM vw_GRINGlobal_Taxonomy_Rpt_" + name.Replace("_","").Replace(" ","");
            return GetRecords<Species>(SQL);
        }
        public List<Species> Search(SpeciesSearch searchEntity)
        {
            List<Species> results = new List<Species>();

            SQL = " SELECT * FROM vw_GRINGlobal_Taxonomy_Species ";
            SQL += " WHERE      (@ID                        IS NULL OR  ID = @ID) ";
            SQL += " AND        (@CreatedByCooperatorID     IS NULL OR  CreatedByCooperatorID = @CreatedByCooperatorID)";
            //SQL += " AND (@Rank                           IS NULL OR  Rank = @Rank)";
            SQL += " AND        (@GenusID                   IS NULL OR  GenusID = @GenusID)";
            SQL += " AND        ((@SpeciesName              IS NULL OR  REPLACE(Name, ' x ', '')     LIKE    'X ' + @SpeciesName + '%')";
            SQL += " OR         (@SpeciesName               IS NULL OR  REPLACE(Name, ' x ', '')     LIKE    '+' + @SpeciesName + '%')";
            SQL += " OR         (@SpeciesName               IS NULL OR  REPLACE(Name, ' x ', '')     LIKE    @SpeciesName + '%'))";
            //SQL += " AND (@VarietyName                    IS NULL OR VarietyName LIKE '%' + @VarietyName + '%')";
            SQL += " AND        (@SynonymCode               IS NULL OR  SynonymCode = @SynonymCode)";
            SQL += " AND        (@IsAcceptedName            IS NULL OR  IsAcceptedName              =       @IsAcceptedName)";
            SQL += " AND        (@SpeciesAuthority          IS NULL OR  COALESCE(SpeciesAuthority, SubspeciesAuthority, VarietyAuthority, SubvarietyAuthority, FormaAuthority)            LIKE    '%' + @SpeciesAuthority + '%')";

            if (!String.IsNullOrEmpty(searchEntity.IDList))
            {
                SQL += " AND (ID IN (" + searchEntity.IDList + "))";
            }

            if (searchEntity.IsNameVerifiedDateOption)
            {
                SQL += "AND NameVerifiedDate IS NULL";
            }

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("GenusID", searchEntity.GenusID > 0 ? (object)searchEntity.GenusID : DBNull.Value, true),
                CreateParameter("Rank", (object)searchEntity.Rank ?? DBNull.Value, true),
                CreateParameter("SpeciesName", (object)searchEntity.SpeciesName ?? DBNull.Value, true),
                CreateParameter("SynonymCode", (object)searchEntity.SynonymCode ?? DBNull.Value, true),
                CreateParameter("IsAcceptedName", (object)searchEntity.IsAcceptedName ?? DBNull.Value, true),
                CreateParameter("SpeciesAuthority", (object)searchEntity.SpeciesAuthority ?? DBNull.Value, true),
            };

            results = GetRecords<Species>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<Species> GetFolderItems(SpeciesSearch searchEntity)
        {
            List<Species> results = new List<Species>();

            SQL = " SELECT * FROM vw_GRINGlobal_Folder_Taxonomy_Species WHERE FolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Species>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public List<CodeValue> SearchProtologues(string protologue)
        {
            List<CodeValue> results = new List<CodeValue>();

            // Replace whitespace with wildcard character(s).
            protologue = protologue.Replace(" ", "%");

            SQL = " SELECT DISTINCT LTRIM(RTRIM(protologue)) AS Value, '' AS Title " +
                    " FROM taxonomy_species " +
                    " WHERE protologue IS NOT NULL ";
            SQL += " AND    (@Protologue       IS NULL OR  Protologue     LIKE     '%' + @Protologue + '%' )";
            SQL += " ORDER BY Value";
            var parameters = new List<IDbDataParameter>
            {
                 CreateParameter("Protologue", (object)protologue ?? DBNull.Value, true)
            };

            results = GetRecords<CodeValue>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }
        public Dictionary<string, string> GetTableNames()
        {
            return new Dictionary<string, string>
            {
                { "taxonomy_family", "Family" },
                { "taxonomy_genus", "Genus" },
                { "taxonomy_species", "Species" }
            };
        }
        protected virtual void BuildInsertUpdateParameters(Species entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_species_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("current_taxonomy_species_id", entity.AcceptedID == 0 ? entity.ID : (object)entity.AcceptedID, true);
            AddParameter("is_specific_hybrid", (object)entity.IsSpecificHybrid ?? DBNull.Value, true);
            AddParameter("hybrid_parentage", (object)entity.HybridParentage ?? DBNull.Value, true);
            AddParameter("name_authority", (object)entity.NameAuthority ?? DBNull.Value, true);
            AddParameter("species_authority", (object)entity.SpeciesAuthority ?? DBNull.Value, true);
            AddParameter("is_subspecific_hybrid", entity.IsSubspecificHybrid == null ? "N" : (object)entity.IsSubspecificHybrid, false);
            AddParameter("subspecies_name", (object)entity.SubspeciesName ?? DBNull.Value, true);
            AddParameter("subspecies_authority", (object)entity.SubspeciesAuthority ?? DBNull.Value, true);
            AddParameter("is_varietal_hybrid", entity.IsVarietalHybrid == null ? "N" : (object)entity.IsVarietalHybrid, false);
            AddParameter("variety_name", (object)entity.VarietyName ?? DBNull.Value, true);
            AddParameter("variety_authority", (object)entity.VarietyAuthority ?? DBNull.Value, true);
            AddParameter("is_subvarietal_hybrid",  entity.IsSubVarietalHybrid == null ? "N" : (object)entity.IsSubVarietalHybrid, false);
            AddParameter("subvariety_name", (object)entity.SubvarietyName ?? DBNull.Value, true);
            AddParameter("subvariety_authority", (object)entity.SubvarietyAuthority ?? DBNull.Value, true);
            AddParameter("is_forma_hybrid", entity.IsFormaHybrid == null ? "N" : (object)entity.IsFormaHybrid, false);
            AddParameter("forma_rank_type", (object)entity.FormaRankType ?? DBNull.Value, true);
            AddParameter("forma_name", (object)entity.FormaName ?? DBNull.Value, true);
            AddParameter("forma_authority", (object)entity.FormaAuthority ?? DBNull.Value, true);
            AddParameter("taxonomy_genus_id", (object)entity.GenusID ?? DBNull.Value, true);
            AddParameter("synonym_code", (object)entity.SynonymCode ?? DBNull.Value, true);
            AddParameter("verifier_cooperator_id", entity.VerifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.VerifiedByCooperatorID, true);
            
            if (entity.NameVerifiedDate == DateTime.MinValue)
                AddParameter("name_verified_date", DBNull.Value, true);
            else
                AddParameter("name_verified_date", (object)entity.NameVerifiedDate, true);

            AddParameter("species_name", (object)entity.SpeciesName ?? DBNull.Value, true);
            AddParameter("protologue", (object)entity.Protologue ?? DBNull.Value, true);
            AddParameter("protologue_virtual_path", (object)entity.ProtologueVirtualPath ?? DBNull.Value, true);
            AddParameter("is_web_visible", entity.IsWebVisible == null ? "N" : (object)entity.IsWebVisible, false);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, true);

            if (entity.ID > 0)
            {
                AddParameter("modified_by", entity.ModifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.ModifiedByCooperatorID, true);
            }
            else
            {
                AddParameter("created_by", entity.CreatedByCooperatorID == 0 ? DBNull.Value : (object)entity.CreatedByCooperatorID, true);
            }
        }
        public List<CodeValue> GetRanks()
        {
            List<CodeValue> ranks = new List<CodeValue>();
            ranks.Add(new CodeValue { Value="SPECIES", Title="Species" });
            ranks.Add(new CodeValue { Value = "SUBSPECIES", Title = "Subspecies" });
            ranks.Add(new CodeValue { Value = "VARIETY", Title = "Variety" });
            ranks.Add(new CodeValue { Value = "SUBVARIETY", Title = "Subvariety" });
            ranks.Add(new CodeValue { Value = "FORMA", Title = "Forma" });
            return ranks;
        }
    }
}
