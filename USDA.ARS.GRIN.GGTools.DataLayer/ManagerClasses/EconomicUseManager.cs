using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class EconomicUseManager : GRINGlobalDataManagerBase, IManager<EconomicUse, EconomicUseSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(EconomicUse entity)
        {
            throw new NotImplementedException();
        }

        public EconomicUse Get(int entityId)
        {
            throw new NotImplementedException();
        }
        public List<EconomicUse> GetFolderItems(EconomicUseSearch searchEntity)
        {
            List<EconomicUse> results = new List<EconomicUse>();

            SQL = " SELECT * FROM vw_GRINGlobal_Folder_Taxonomy_Economic_Use WHERE FolderID = @FolderID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<EconomicUse>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public virtual int Insert(EconomicUse entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<EconomicUse>(entity);
            SQL = "usp_GGTools_Taxon_EconomicUse_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_use_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddStandardParameters();

            RowsAffected = ExecuteNonQuery(false, "@out_taxonomy_use_id");
            entity.ID = GetParameterValue<int>("@out_taxonomy_use_id", -1);

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception("SQL Error " + errorNumber.ToString());
            }

            RowsAffected = entity.ID;
            return RowsAffected;
        }

        public virtual int Update(EconomicUse entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<EconomicUse>(entity);

            SQL = "usp_GGTools_Taxon_EconomicUse_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception("SQL Error " + errorNumber.ToString());
            }

            return RowsAffected;
        }

        public List<EconomicUse> Search(EconomicUseSearch searchEntity)
        {
            List<EconomicUse> results = new List<EconomicUse>();

            SQL = "SELECT * FROM vw_GGTools_Taxon_EconomicUses ";
            SQL += " WHERE  (@CreatedByCooperatorID     IS NULL     OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND    (@ID                        IS NULL     OR ID                       =       @ID)";
            SQL += " AND    (@SpeciesID                 IS NULL     OR SpeciesID                =       @SpeciesID)";
            SQL += " AND    (@SpeciesName               IS NULL     OR SpeciesName              LIKE    '%' +  @SpeciesName + '%')";
            SQL += " AND    (@EconomicUsageCode         IS NULL     OR EconomicUsageCode        =       @EconomicUsageCode)";
            SQL += " AND    (@EconomicUsageType         IS NULL     OR EconomicUsageType        =       @EconomicUsageType)";
            SQL += " AND    (@PlantPartCode             IS NULL     OR PlantPartCode            =       @PlantPartCode)";

            if (!String.IsNullOrEmpty(searchEntity.IDList))
            {
                SQL += " AND    SpeciesID IN (" + searchEntity.IDList + ")";
            }

            var parameters = new List<IDbDataParameter> {
            CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
            CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
            CreateParameter("SpeciesID", searchEntity.SpeciesID > 0 ? (object)searchEntity.SpeciesID : DBNull.Value, true),
            CreateParameter("SpeciesName", (object)searchEntity.SpeciesName ?? DBNull.Value, true),
            CreateParameter("EconomicUsageCode", (object)searchEntity.EconomicUsageCode ?? DBNull.Value, true),
            CreateParameter("EconomicUsageType", (object)searchEntity.EconomicUsageTypeCode ?? DBNull.Value, true),
            CreateParameter("PlantPartCode", (object)searchEntity.PlantPartCode ?? DBNull.Value, true),
        };

        results = GetRecords<EconomicUse>(SQL, parameters.ToArray());
        RowsAffected = results.Count;

        return results;
    }

        public List<EconomicUse> SearchFolderItems(EconomicUseSearch searchEntity)
        {
            List<EconomicUse> results = new List<EconomicUse>();

            SQL = " SELECT auil.app_user_item_list_id AS ListID, " +
                " auil.list_name AS ListName, " +
                " auil.app_user_item_folder_id AS FolderID, " +
                " vgtf.* " +
                " FROM vw_GGTools_Taxon_Families vgtf " +
                " JOIN app_user_item_list auil " +
                " ON vgtf.ID = auil.id_number " +
                " WHERE auil.id_type = 'taxonomy_family' ";
            SQL += "AND  (@FolderID                          IS NULL OR  auil.app_user_item_folder_id       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<EconomicUse>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public virtual List<CodeValue> GetUsageTypes()
        {
            SQL = "SELECT Value, Title FROM vw_GGTools_Taxon_EconomicUsageTypes ORDER BY Title";
            List<CodeValue> usageTypes = GetRecords<CodeValue>(SQL);
            return usageTypes;
        }

        #region Taxonomy Common

        public virtual List<Cooperator> GetCooperators(string tableName)
        {
            SQL = "usp_GGTools_GRINGlobal_CreatedByCooperators_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("table_name", (object)tableName, false)
            };
            List<Cooperator> cooperators = GetRecords<Cooperator>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            RowsAffected = cooperators.Count;
            return cooperators;
        }
        public virtual List<CodeValue> GetCodeValues(string groupName)
        {
            SQL = "usp_GGTools_GRINGlobal_CodeValuesByGroup_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("group_name", (object)groupName, false)
            };
            List<CodeValue> codeValues = GetRecords<CodeValue>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return codeValues;
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
        //public List<Citation> GetAvailableCitations(int speciesId)
        //{
        //    SQL = "usp_TaxonomySpeciesCitationsWithSynonyms_Select";
        //    var parameters = new List<IDbDataParameter> {
        //        CreateParameter("taxonomy_species_id", (object)speciesId, false)
        //    };
        //    List<Citation> citations = GetRecords<Citation>(SQL, CommandType.StoredProcedure, parameters.ToArray());
        //    return citations;
        //}

        #endregion

        protected virtual void BuildInsertUpdateParameters(EconomicUse entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_use_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("taxonomy_species_id", entity.SpeciesID == 0 ? DBNull.Value : (object)entity.SpeciesID, true);
            AddParameter("economic_usage_code", (object)entity.EconomicUsageCode ?? DBNull.Value, false);
            AddParameter("usage_type", (object)entity.EconomicUsageType ?? DBNull.Value, true);
            AddParameter("plant_part_code", (object)entity.PlantPartCode ?? DBNull.Value, false);
            AddParameter("citation_id", entity.CitationID == 0 ? DBNull.Value : (object)entity.CitationID, true); ;
            AddParameter("note", (object)entity.Note ?? DBNull.Value, false);

            if (entity.ID > 0)
            {
                AddParameter("modified_by", entity.ModifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.ModifiedByCooperatorID, true);
            }
            else
            {
                AddParameter("created_by", entity.CreatedByCooperatorID == 0 ? DBNull.Value : (object)entity.CreatedByCooperatorID, true);
            }
        }
    }
}
