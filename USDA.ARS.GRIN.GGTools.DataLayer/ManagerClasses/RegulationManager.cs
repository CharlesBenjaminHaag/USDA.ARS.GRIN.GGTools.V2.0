using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class RegulationManager : AppDataManagerBase, IManager<Regulation, RegulationSearch>
    {
        public int Insert(Regulation entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Regulation>(entity);

            SQL = "usp_GGTools_Taxon_Regulation_Insert";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_regulation_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            entity.ID = GetParameterValue<int>("@out_taxonomy_regulation_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception();
            }
            return RowsAffected;
        }

        public int Update(Regulation entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Regulation>(entity);

            SQL = "usp_GGTools_Taxon_Regulation_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception();
            }
            return RowsAffected;
        }

        public int Delete(Regulation entity)
        {
            throw new NotImplementedException();
        }

        public Regulation Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<Regulation> Search(RegulationSearch searchEntity)
        {
            List<Regulation> results = new List<Regulation>();

            SQL = "SELECT*  FROM vw_GGTools_Taxon_Regulations ";
            SQL += " WHERE  (@CreatedByCooperatorID     IS NULL OR   CreatedByCooperatorID      =   @CreatedByCooperatorID)";
            SQL += " AND    (@ID                        IS NULL OR   ID                         =   @ID)";
            SQL += " AND    (@GeographyID               IS NULL OR   GeographyID                =   @GeographyID)";
            SQL += " AND    (@RegulationTypeCode        IS NULL OR   RegulationTypeCode         =   @RegulationTypeCode)";
            SQL += " AND    (@RegulationLevelCode       IS NULL OR   RegulationLevelCode        =   @RegulationLevelCode)";
            SQL += " AND    (@Description               IS NULL OR   Description                =   @Description)";
            SQL += " ORDER BY AssembledName ASC";
            
            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("GeographyID", searchEntity.GeographyID > 0 ? (object)searchEntity.GeographyID : DBNull.Value, true),
                CreateParameter("RegulationTypeCode", (object)searchEntity.RegulationTypeCode ?? DBNull.Value, true),
                CreateParameter("RegulationLevelCode", (object)searchEntity.RegulationLevelCode ?? DBNull.Value, true),
                CreateParameter("Description", (object)searchEntity.Description ?? DBNull.Value, true),
            };

            results = GetRecords<Regulation>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
   
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
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
            return codeValues;
        }
        public List<Geography> GetGeographies()
        {
            SQL = "SELECT * FROM vw_GGTools_Taxon_Geographies";
            List<Geography> geographies = GetRecords<Geography>(SQL);
            return geographies;
        }
        protected virtual void BuildInsertUpdateParameters(Regulation entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_regulation_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("geography_id", entity.GeographyID == 0 ? DBNull.Value : (object)entity.GeographyID, true);
            AddParameter("regulation_type_code ", String.IsNullOrEmpty(entity.RegulationTypeCode) ? DBNull.Value : (object)entity.RegulationTypeCode, true);
            AddParameter("regulation_level_code", String.IsNullOrEmpty(entity.RegulationLevelCode) ? DBNull.Value : (object)entity.RegulationLevelCode, true);
            AddParameter("description", String.IsNullOrEmpty(entity.Description) ? DBNull.Value : (object)entity.Description, true);
            AddParameter("url_1", String.IsNullOrEmpty(entity.URL1) ? DBNull.Value : (object)entity.URL1, true);
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
