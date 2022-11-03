using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class CitationManager : AppDataManagerBase, IManager<Citation, CitationSearch>
    {
        public int Insert(Citation entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Citation>(entity);
            SQL = "usp_GGTools_Taxon_Citation_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_citation_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_citation_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }
        public int InsertClone(Citation entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Citation>(entity);

            //TODO
            //1. 

            SQL = "usp_GGTools_Taxon_Citation_InsertClone";

            AddParameter("citation_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            AddParameter("created_by", entity.CreatedByCooperatorID == 0 ? DBNull.Value : (object)entity.CreatedByCooperatorID, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_citation_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_citation_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }
        public int Update(Citation entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Citation>(entity);
            SQL = "usp_GGTools_Taxon_Citation_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return RowsAffected;
        }
        public int UpdateReference(string tableName, int entityId, int citationId, int modifiedBy)
        {
            Reset(CommandType.StoredProcedure);
            SQL = "usp_GGTools_Taxon_CitationReference_Update";

            AddParameter("table_name", (object)tableName, true);
            AddParameter("id_value", (object)entityId, true);
            AddParameter("citation_id", citationId > 0 ? (object)citationId : DBNull.Value, true);
            AddParameter("modified_by", (object)modifiedBy, true);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return RowsAffected;
        }
        public int Delete(Citation entity)
        {
            throw new NotImplementedException();
        }
        public int DeleteReference(string tableName, int entityId, int modifiedBy)
        {
            Reset(CommandType.StoredProcedure);
            SQL = "usp_GGTools_Taxon_CitationReference_Delete";

            AddParameter("table_name", (object)tableName, true);
            AddParameter("id_value", (object)entityId, true);
            AddParameter("modified_by", (object)modifiedBy, true);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return RowsAffected;
        }
        public Citation Get(int entityId)
        {
            return null;
        }
        public List<Citation> GetTaxonCitations(string tableName, int entityId)
        {
            List<Citation> results = new List<Citation>();

            SQL = " SELECT * FROM vw_GGTools_Taxon_Citations ";

            switch (tableName)
            {
                case "taxonomy_family_map":
                    SQL += " WHERE FamilyID IN ";
                    SQL += " (SELECT ID FROM vw_GGTools_Taxon_FamilyMaps WHERE ID = @ID OR AcceptedID = @ID) ";
                    break;
                case "taxonomy_genus":
                    SQL += " WHERE GenusID IN ";
                    SQL += " (SELECT ID FROM vw_GGTools_Taxon_Genera WHERE ID = @ID OR AcceptedID = @ID) ";
                    break;
                case "taxonomy_species":
                    SQL += " WHERE SpeciesID IN ";
                    SQL += " (SELECT ID FROM vw_GGTools_Taxon_Species WHERE ID = @ID OR AcceptedID = @ID) ";
                    break;
            }
            
            SQL += " ORDER BY TaxonName, CitationText";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", entityId > 0 ? (object)entityId : DBNull.Value, true),
            };

            results = GetRecords<Citation>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<Citation> Search(CitationSearch searchEntity)
        {
            List<Citation> results = new List<Citation>();

            SQL = "SELECT * FROM vw_GGTools_Taxon_Citations ";

            SQL += " WHERE      (@ID                            IS NULL OR ID                       =       @ID)";
            SQL += " AND        (@CreatedByCooperatorID         IS NULL OR CreatedByCooperatorID    =       @CreatedByCooperatorID)";
            SQL += " AND        (@CreatedDate                   IS NULL OR CreatedDate              =       @CreatedDate)";
            SQL += " AND        (@ModifiedByCooperatorID        IS NULL OR ModifiedByCooperatorID   =       @ModifiedByCooperatorID)";
            SQL += " AND        (@ModifiedDate                  IS NULL OR ModifiedDate             =       @ModifiedDate)";
            SQL += " AND        (@Note                          IS NULL OR Note                     LIKE    '%' + @Note + '%')";

            SQL += " AND        (@LiteratureID                  IS NULL OR LiteratureID             =       @LiteratureID)";
            SQL += " AND        (@LiteratureTypeCode            IS NULL OR LiteratureTypeCode       =       @LiteratureTypeCode)";
            SQL += " AND        (@FamilyID                      IS NULL OR FamilyID                 =       @FamilyID)";
            SQL += " AND        (@GenusID                       IS NULL OR GenusID                  =       @GenusID)";
            SQL += " AND        (@SpeciesID                     IS NULL OR SpeciesID                =       @SpeciesID)";
            SQL += " AND        (@FamilyName                    IS NULL OR FamilyName               =       @FamilyName) ";
            SQL += " AND        (@GenusName                     IS NULL OR GenusName                =       @GenusName) ";
            SQL += " AND        (@SpeciesName                    IS NULL OR SpeciesName              =       @SpeciesName) ";
            SQL += " AND        (@StandardAbbreviation          IS NULL OR StandardAbbreviation     =       @StandardAbbreviation)";
            SQL += " AND        (@Abbreviation                  IS NULL OR Abbreviation             LIKE    '%' + @Abbreviation + '%')";
            SQL += " AND        (@CitationTitle                 IS NULL OR CitationTitle            LIKE    '%' + @CitationTitle + '%')";
            SQL += " AND        (@EditorAuthorName              IS NULL OR EditorAuthorName         LIKE    '%' + @EditorAuthorName + '%')";
            SQL += " AND        (@CitationYear                  IS NULL OR CitationYear             LIKE    '%' + @CitationYear + '%')";
            SQL += " AND        (@CitationType                  IS NULL OR CitationType             =       @CitationType)";
            SQL += " AND        (@ReferenceTitle                IS NULL OR ReferenceTitle           LIKE    '%' + @ReferenceTitle + '%')";

            if (!String.IsNullOrEmpty(searchEntity.FamilyIDList))
            {
                SQL += " AND    FamilyID IN (" + searchEntity.FamilyIDList + ")";
            }

            if (!String.IsNullOrEmpty(searchEntity.GenusIDList))
            {
                SQL += " AND    GenusID IN (" + searchEntity.GenusIDList + ")";
            }

            if (!String.IsNullOrEmpty(searchEntity.SpeciesIDList))
            {
                SQL += " AND    SpeciesID IN ("  + searchEntity.SpeciesIDList  + ")";
            }

            // TODO Refactor (?) Limit search scope based on the table involved.
            switch(searchEntity.TableName)
            {
                case "taxonomy_geography_map":
                    SQL += " AND SpeciesID IS NOT NULL ";
                    break;
            }

            SQL += " ORDER BY TaxonName, CitationText ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("CreatedDate", searchEntity.CreatedDate > DateTime.MinValue ? (object)searchEntity.CreatedDate : DBNull.Value, true),
                CreateParameter("ModifiedByCooperatorID", searchEntity.ModifiedByCooperatorID > 0 ? (object)searchEntity.ModifiedByCooperatorID : DBNull.Value, true),
                CreateParameter("ModifiedDate", searchEntity.ModifiedDate > DateTime.MinValue ? (object)searchEntity.ModifiedDate : DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true),

                CreateParameter("LiteratureID", searchEntity.LiteratureID > 0 ? (object)searchEntity.LiteratureID : DBNull.Value, true),
                CreateParameter("LiteratureTypeCode", (object)searchEntity.LiteratureTypeCode ?? DBNull.Value, true),
                CreateParameter("FamilyID", searchEntity.FamilyID > 0 ? (object)searchEntity.FamilyID : DBNull.Value, true),
                CreateParameter("GenusID", searchEntity.GenusID > 0 ? (object)searchEntity.GenusID : DBNull.Value, true),
                CreateParameter("SpeciesID", searchEntity.SpeciesID > 0 ? (object)searchEntity.SpeciesID : DBNull.Value, true),
                CreateParameter("FamilyName", (object)searchEntity.FamilyName ?? DBNull.Value, true),
                CreateParameter("GenusName", (object)searchEntity.GenusName ?? DBNull.Value, true),
                CreateParameter("SpeciesName", (object)searchEntity.SpeciesName ?? DBNull.Value, true),
                CreateParameter("StandardAbbreviation", (object)searchEntity.StandardAbbreviation ?? DBNull.Value, true),
                CreateParameter("Abbreviation", (object)searchEntity.Abbreviation ?? DBNull.Value, true),
                CreateParameter("CitationTitle", (object)searchEntity.CitationTitle ?? DBNull.Value, true),
                CreateParameter("EditorAuthorName", (object)searchEntity.EditorAuthorName ?? DBNull.Value, true),
                CreateParameter("CitationYear", (object)searchEntity.CitationYear ?? DBNull.Value, true),
                CreateParameter("CitationType", (object)searchEntity.CitationType ?? DBNull.Value, true),
                CreateParameter("ReferenceTitle", (object)searchEntity.ReferenceTitle ?? DBNull.Value, true)
            };

            results = GetRecords<Citation>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }
        public List<Citation> SearchFolderItems(CitationSearch searchEntity)
        {
            List<Citation> results = new List<Citation>();

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
            results = GetRecords<Citation>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }
        public void BuildInsertUpdateParameters(Citation entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("citation_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            if (entity.AcceptedID == 0)
            {
                entity.AcceptedID = entity.ID;
            }

            //TEMP
            if (String.IsNullOrEmpty(entity.IsAcceptedName))
            {
                entity.IsAcceptedName = "N";
            }

            AddParameter("literature_id", entity.LiteratureID == 0 ? DBNull.Value : (object)entity.LiteratureID, true);
            AddParameter("citation_title", String.IsNullOrEmpty(entity.CitationTitle) ? DBNull.Value : (object)entity.CitationTitle, true);
            AddParameter("author_name", String.IsNullOrEmpty(entity.CitationAuthorName) ? DBNull.Value : (object)entity.CitationAuthorName, true);
            AddParameter("citation_year", entity.CitationYear == 0 ? DBNull.Value : (object)entity.CitationYear, true);
            AddParameter("doi_reference", String.IsNullOrEmpty(entity.DOIReference) ? DBNull.Value : (object)entity.DOIReference, true);
            AddParameter("taxonomy_species_id", entity.SpeciesID == 0 ? DBNull.Value : (object)entity.SpeciesID, true);
            AddParameter("taxonomy_family_id", entity.FamilyID == 0 ? DBNull.Value : (object)entity.FamilyID, true);
            AddParameter("taxonomy_genus_id", entity.GenusID == 0 ? DBNull.Value : (object)entity.GenusID, true);
            AddParameter("is_accepted_name", String.IsNullOrEmpty(entity.IsAcceptedName) ? DBNull.Value : (object)entity.IsAcceptedName, true);
            AddParameter("note", String.IsNullOrEmpty(entity.Note) ? DBNull.Value : (object)entity.Note, true);

            if (entity.ID > 0)
            {
                AddParameter("modified_by", entity.ModifiedByCooperatorID == 0 ? DBNull.Value : (object)entity.ModifiedByCooperatorID, true);
            }
            else
            {
                AddParameter("created_by", entity.CreatedByCooperatorID == 0 ? DBNull.Value : (object)entity.CreatedByCooperatorID, true);
            }
        }
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
            RowsAffected = codeValues.Count;
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
        public List<CodeValue> GetStandardAbbreviations()
        {
            List<CodeValue> codeValues = new List<CodeValue>();

            SQL = "SELECT standard_abbreviation AS Value, standard_abbreviation AS Title " + 
                  " FROM literature " +
                  " WHERE standard_abbreviation IS NOT NULL " +
                  " ORDER BY standard_abbreviation";
            codeValues = GetRecords<CodeValue>(SQL);
            RowsAffected = codeValues.Count;
            return codeValues;
        }
        public List<CodeValue> GetAbbreviations()
        {
            List<CodeValue> codeValues = new List<CodeValue>();

            SQL = "SELECT abbreviation AS Value, abbreviation AS Title " +
                  " FROM literature " +
                  " WHERE abbreviation IS NOT NULL " +
                  " ORDER BY abbreviation";
            codeValues = GetRecords<CodeValue>(SQL);
            RowsAffected = codeValues.Count;
            return codeValues;
        }
    }
}
