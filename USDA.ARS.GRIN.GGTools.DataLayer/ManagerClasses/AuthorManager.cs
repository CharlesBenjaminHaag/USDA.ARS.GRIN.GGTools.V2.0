using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class AuthorManager : AppDataManagerBase, IManager<Author, AuthorSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(Author entity)
        {
            throw new NotImplementedException();
        }

        public Author Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(Author entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Author>(entity);

            SQL = "usp_GGTools_Taxon_Author_Insert";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_author_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            entity.ID = GetParameterValue<int>("@out_taxonomy_author_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception();
            }
            return RowsAffected;
        }

        public List<Author> Search(AuthorSearch searchEntity)
        {
            List<Author> results = new List<Author>();

            SQL = "SELECT * FROM vw_GGTools_Taxon_Authors";
            SQL += " WHERE  (@ID                IS NULL     OR  ID = @ID) ";
            SQL += " AND    (@FullName          IS NULL     OR FullName         LIKE    '%' + @FullName + '%')";

            if (searchEntity.IsShortNameExactMatch == "Y")
            {
                SQL += " AND      (@ShortName   IS NULL     OR ShortName        =       '' + @ShortName + '')";
            }
            else
            {
                SQL += " AND      (@ShortName   IS NULL     OR ShortName        LIKE    '%' + @ShortName + '%')";
            }
            
            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("FullName", (object)searchEntity.FullName ?? DBNull.Value, true),
                CreateParameter("ShortName", (object)searchEntity.ShortName ?? DBNull.Value, true)
            };

            results = GetRecords<Author>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public List<Author> SearchFolderItems(AuthorSearch searchEntity)
        {
            List<Author> results = new List<Author>();

            SQL = " SELECT auil.app_user_item_list_id AS ListID, " +
                " auil.list_name AS ListName, " +
                " auil.app_user_item_folder_id AS FolderID, " +
                " vgta.* " +
                " FROM vw_GGTools_Taxon_Authors vgta " +
                " JOIN app_user_item_list auil " +
                " ON vgta.ID = auil.id_number " +
                " WHERE auil.id_type = 'taxonomy_author' ";
            SQL += "AND  (@FolderID                          IS NULL OR  auil.app_user_item_folder_id       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Author>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public List<Author> SearchTaxa(string tableName, string searchText)
        {
            List<Author> results = new List<Author>();

            SQL = "SELECT DISTINCT " +
                " AuthorityText " +
                " FROM vw_GGTools_Taxon_Authorities ";
            SQL += " WHERE  (@AuthorityText     IS NULL OR AuthorityText        LIKE  '%' + @AuthorityText + '%') ";
            SQL += " AND    (@TableName         IS NULL OR TableName            LIKE  '%' + @TableName + '%')";
            SQL += " ORDER BY AuthorityText ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("AuthorityText", (object)searchText, true),
                CreateParameter("TableName", (object)tableName, true)
            };
            results = GetRecords<Author>(SQL, parameters.ToArray());
            return results;
        }

        public int Update(Author entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Author>(entity);

            SQL = "usp_GGTools_Taxon_Author_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
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

        protected void BuildInsertUpdateParameters(Author entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_author_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("short_name", String.IsNullOrEmpty(entity.ShortName) ? DBNull.Value : (object)entity.ShortName, true);
            AddParameter("full_name", String.IsNullOrEmpty(entity.FullName) ? DBNull.Value : (object)entity.FullName, true);
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
    }
}
