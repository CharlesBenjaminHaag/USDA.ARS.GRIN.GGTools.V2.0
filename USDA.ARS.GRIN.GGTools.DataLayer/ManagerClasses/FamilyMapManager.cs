using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class FamilyMapManager : GRINGlobalDataManagerBase, IManager<FamilyMap, FamilyMapSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(FamilyMap entity)
        {
            throw new NotImplementedException();
        }

        public FamilyMap Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<FamilyMap> GetSynonyms(int entityId)
        {
            SQL = "usp_GGTools_Taxon_FamilySynonyms_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@taxonomy_family_id", (object)entityId, false)
            };
            List<FamilyMap> familyMaps = GetRecords<FamilyMap>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return familyMaps;
        }
        public List<FamilyMap> GetSubdivisions(int entityId)
        {
            SQL = "usp_GGTools_Taxon_FamilySubdivisions_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("@taxonomy_family_id", (object)entityId, false)
            };
            List<FamilyMap> familyMaps = GetRecords<FamilyMap>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return familyMaps;
        }

        public int Insert(FamilyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<FamilyMap>(entity);
            SQL = "usp_GGTools_Taxon_Family_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_family_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_family_map_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }

        public int InsertSubfamily(FamilyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<FamilyMap>(entity);
            SQL = "usp_GGTools_Taxon_Subfamily_Insert";

            BuildSubfamilyInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_family_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_family_map_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }

        public int InsertTribe(FamilyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<FamilyMap>(entity);
            SQL = "usp_GGTools_Taxon_Tribe_Insert";

            BuildTribeInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_family_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_family_map_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;

        }

        public int InsertSubtribe(FamilyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<FamilyMap>(entity);
            SQL = "usp_GGTools_Taxon_Subtribe_Insert";

            BuildSubtribeInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_taxonomy_family_map_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_taxonomy_family_map_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;

        }

        public List<FamilyMap> Search(FamilyMapSearch searchEntity)
        {
            List<FamilyMap> results = new List<FamilyMap>();

            SQL = "SELECT *, LTRIM(RTRIM(COALESCE(FamilyName, ''))) + " +
            " CASE COALESCE(convert(nvarchar, SubfamilyName), '') WHEN '' THEN '' ELSE '' + ' subfam. ' + LTRIM(RTRIM(SubfamilyName)) END " +
            " + CASE COALESCE(convert(nvarchar, TribeName), '') WHEN '' THEN '' ELSE '' + ' tr. ' + LTRIM(RTRIM(TribeName)) END " +
            " + CASE COALESCE(convert(nvarchar, SubtribeName), '') WHEN '' THEN '' ELSE '' + ' subtr. ' + LTRIM(RTRIM(SubtribeName)) END " +
            " AS AssembledName FROM vw_GGTools_Taxon_FamilyMaps";
            SQL += " WHERE (@ID                         IS NULL OR ID = @ID) ";
            SQL += " AND  (@CreatedByCooperatorID       IS NULL OR CreatedByCooperatorID        =           @CreatedByCooperatorID)";
            SQL += " AND  (@OrderID                     IS NULL OR OrderID                      =           @OrderID)";
            SQL += " AND  (@FamilyID                    IS NULL OR FamilyID                     =           @FamilyID)";
            SQL += " AND  (@SubfamilyID                 IS NULL OR SubfamilyID                  =           @SubfamilyID)";
            SQL += " AND  (@TribeID                     IS NULL OR TribeID                      =           @TribeID)";
            SQL += " AND  (@SubtribeID                  IS NULL OR SubtribeID                   =           @SubtribeID)";
            SQL += " AND  (@FamilyName                  IS NULL OR FamilyName                   LIKE '%' +  @FamilyName + '%')";
            SQL += " AND  (@SubfamilyName               IS NULL OR SubfamilyName                LIKE '%' +  @SubfamilyName + '%')";
            SQL += " AND  (@TribeName                   IS NULL OR TribeName                    LIKE '%' +  @TribeName + '%')";
            SQL += " AND  (@SubtribeName                IS NULL OR SubtribeName                 LIKE '%' +  @SubtribeName + '%')";
            SQL += " AND  (@FamilyTypeCode              IS NULL OR FamilyTypeCode               =           @FamilyTypeCode)";
            SQL += " AND  (@FamilyRank                  IS NULL OR FamilyRank                   =           @FamilyRank)";
            SQL += " AND  (@IsAcceptedName              IS NULL OR IsAcceptedName               =           @IsAcceptedName)";
            SQL += " AND  (@Authority                   IS NULL OR Authority                    LIKE '%' +  @Authority + '%')";
            SQL += " AND  (@Note                        IS NULL OR Note                         LIKE '%' +  @Note + '%')";

            if (searchEntity.IsInfrafamilal == "Y")
            {
                SQL += " AND FamilyRank <> 'FAMILY'";
            }

            if (searchEntity.FamilyRank == "") searchEntity.FamilyRank = null;

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("OrderID", searchEntity.OrderID > 0 ? (object)searchEntity.OrderID : DBNull.Value, true),
                CreateParameter("FamilyID", searchEntity.FamilyID > 0 ? (object)searchEntity.FamilyID : DBNull.Value, true),
                CreateParameter("SubfamilyID", searchEntity.SubFamilyID > 0 ? (object)searchEntity.SubFamilyID : DBNull.Value, true),
                CreateParameter("TribeID", searchEntity.TribeID > 0 ? (object)searchEntity.TribeID : DBNull.Value, true),
                CreateParameter("SubtribeID", searchEntity.SubTribeID > 0 ? (object)searchEntity.SubTribeID : DBNull.Value, true),
                CreateParameter("FamilyName", (object)searchEntity.FamilyName ?? DBNull.Value, true),
                CreateParameter("SubfamilyName", (object)searchEntity.SubFamilyName ?? DBNull.Value, true),
                CreateParameter("TribeName", (object)searchEntity.TribeName ?? DBNull.Value, true),
                CreateParameter("SubtribeName", (object)searchEntity.SubTribeName ?? DBNull.Value, true),
                CreateParameter("FamilyTypeCode", (object)searchEntity.FamilyTypeCode ?? DBNull.Value, true),
                CreateParameter("FamilyRank", (object)searchEntity.FamilyRank ?? DBNull.Value, true),
                CreateParameter("IsAcceptedName", (object)searchEntity.IsAcceptedName ?? DBNull.Value, true),
                CreateParameter("Authority", (object)searchEntity.Authority ?? DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.Note ?? DBNull.Value, true)
            };

            results = GetRecords<FamilyMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;

            return results;
        }

        public List<FamilyMap> SearchFolderItems(FamilyMapSearch searchEntity)
        {
            List<FamilyMap> results = new List<FamilyMap>();

            SQL = " SELECT vgtcn.* FROM vw_GGTools_Taxon_FamilyMaps vgtcn JOIN vw_GGTools_GRINGlobal_AppUserItemLists vgga " +
                   " ON vgtcn.ID = vgga.EntityID WHERE vgga.TableName = 'taxonomy_family_map' ";
            SQL += "AND  (@FolderID                          IS NULL OR  FolderID       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<FamilyMap>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }
        public int Update(FamilyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<FamilyMap>(entity);
            SQL = "usp_GGTools_Taxon_Family_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }

        public int UpdateSubfamily(FamilyMap entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<FamilyMap>(entity);
            SQL = "usp_GGTools_Taxon_Subfamily_Update";

            BuildSubfamilyInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }
        public int UpdateTribe(FamilyMap entity)
        {
            throw new NotImplementedException();
        }
        public int UpdateSubtribe(FamilyMap entity)
        {
            throw new NotImplementedException();
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

        public void BuildInsertUpdateParameters(FamilyMap entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("taxonomy_family_map_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("taxonomy_family_map_accepted_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("type_taxonomy_genus_id", entity.TypeGenusID == 0 ? DBNull.Value : (object)entity.TypeGenusID, true);
            AddParameter("taxonomy_order_id", entity.OrderID == 0 ? DBNull.Value : (object)entity.OrderID, true);
            AddParameter("family_name", (object)entity.FamilyName ?? DBNull.Value, true);
            AddParameter("family_authority", (object)entity.Authority ?? DBNull.Value, true);
            AddParameter("alternate_name", (object)entity.AlternateName ?? DBNull.Value, true);
            AddParameter("family_type_code", (object)entity.FamilyTypeCode ?? DBNull.Value, true);
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

        public void BuildSubfamilyInsertUpdateParameters(FamilyMap entity)
        {
            AddParameter("taxonomy_family_map_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            AddParameter("taxonomy_family_map_accepted_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("taxonomy_family_id", entity.FamilyID == 0 ? DBNull.Value : (object)entity.FamilyID, true);
            AddParameter("taxonomy_subfamily_id", entity.SubfamilyID == 0 ? DBNull.Value : (object)entity.SubfamilyID, true);
            AddParameter("type_taxonomy_genus_id", entity.TypeGenusID == 0 ? DBNull.Value : (object)entity.TypeGenusID, true);
            AddParameter("subfamily_name", (object)entity.SubfamilyName ?? DBNull.Value, true);
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

        public void BuildTribeInsertUpdateParameters(FamilyMap entity)
        {
            AddParameter("taxonomy_family_map_id", entity.ParentID == 0 ? DBNull.Value : (object)entity.ParentID, true);
            AddParameter("taxonomy_family_map_accepted_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("type_taxonomy_genus_id", entity.TypeGenusID == 0 ? DBNull.Value : (object)entity.TypeGenusID, true);
            AddParameter("tribe_name", (object)entity.FamilyName ?? DBNull.Value, true);
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
        public void BuildSubtribeInsertUpdateParameters(FamilyMap entity)
        {
            AddParameter("taxonomy_family_map_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            AddParameter("taxonomy_family_map_accepted_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("type_taxonomy_genus_id", entity.TypeGenusID == 0 ? DBNull.Value : (object)entity.TypeGenusID, true);
            AddParameter("taxonomy_subfamily_id", entity.SubfamilyID == 0 ? DBNull.Value : (object)entity.SubfamilyID, true);
            AddParameter("taxonomy_tribe_id", entity.TribeID == 0 ? DBNull.Value : (object)entity.TribeID, true);
            AddParameter("subtribe_name", (object)entity.SubtribeName ?? DBNull.Value, true);
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





    }
}
