using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class GeographyManager : AppDataManagerBase, IManager<Geography, GeographySearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(Geography entity)
        {
            throw new NotImplementedException();
        }

        public Geography Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public int Insert(Geography entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Geography>(entity);
            SQL = "usp_GGTools_Taxon_Geography_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_geography_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_geography_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            RowsAffected = entity.ID;
            return entity.ID;
        }

        public List<Geography> Search(GeographySearch searchEntity)
        {
            List<Geography> results = new List<Geography>();
            string whereClause = String.Empty;

            SQL = " SELECT * FROM vw_GGTools_GRINGlobal_Geographies ";
            SQL += " WHERE  (@CreatedByCooperatorID     IS NULL OR   CreatedByCooperatorID      =       @CreatedByCooperatorID)";
            SQL += " AND    (@ID                        IS NULL OR   ID                         =       @ID)";
            SQL += " AND    (@CountryCode               IS NULL OR   CountryCode                =       @CountryCode)";
            SQL += " AND    (@ContinentRegionID         IS NULL OR   ContinentRegionID          =       @ContinentRegionID)";
            SQL += " AND    (@SubContinentRegionID      IS NULL OR   SubContinentRegionID       =       @SubContinentRegionID)";
            SQL += " AND    (@IsValid                   IS NULL OR   IsValid                    =       @IsValid)";

            //if (searchEntity.IsUSOnly == "Y")
            //{
            //    SQL += " AND CountryCode IN ('165','USA')";
            //    SQL += " AND Admin1TypeCode IN ('107','23')";
            //    SQL += " AND Admin2 IS NULL";
            //}

            if (!String.IsNullOrEmpty(searchEntity.ContinentIDList))
            {
                whereClause += " AND (RegionID IN (" + searchEntity.ContinentIDList + ")";
            }

            if (!String.IsNullOrEmpty(searchEntity.CountryCodeList))
            {
                if (!String.IsNullOrEmpty(searchEntity.ContinentIDList))
                {
                    whereClause += " OR ";
                }
                else
                {
                    whereClause += " AND ";
                }
                whereClause += " CountryCode IN (" + searchEntity.CountryCodeList + ")";
            }

            if (!String.IsNullOrEmpty(searchEntity.ContinentIDList))
            {
                whereClause += ")";
            }

            if (!String.IsNullOrEmpty(whereClause))
            {
                SQL += whereClause;
            }

            SQL += " ORDER BY Continent, SubContinent, CountryDescription ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("CreatedByCooperatorID", searchEntity.CreatedByCooperatorID > 0 ? (object)searchEntity.CreatedByCooperatorID : DBNull.Value, true),
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("ContinentRegionID", searchEntity.ContinentRegionID > 0 ? (object)searchEntity.ContinentRegionID : DBNull.Value, true),
                CreateParameter("SubContinentRegionID", searchEntity.SubContinentRegionID > 0 ? (object)searchEntity.SubContinentRegionID : DBNull.Value, true),
                CreateParameter("CountryCode", (object)searchEntity.CountryCode ?? DBNull.Value, true),
                CreateParameter("IsValid", (object)searchEntity.IsValid ?? DBNull.Value, true),
            };

            results = GetRecords<Geography>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public List<Geography> SearchFolderItems(GeographySearch searchEntity)
        {
            List<Geography> results = new List<Geography>();

            SQL = " SELECT auil.app_user_item_list_id AS ListID, " +
                " auil.list_name AS ListName, " +
                " auil.app_user_item_folder_id AS FolderID, " +
                " vgtf.* " +
                " FROM vw_GGTools_Taxon_Geographies vgtf " +
                " JOIN app_user_item_list auil " +
                " ON vgtf.ID = auil.id_number " +
                " WHERE auil.id_type = 'geography' ";
            SQL += "AND  (@FolderID                          IS NULL OR  auil.app_user_item_folder_id       =           @FolderID)";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("FolderID", searchEntity.FolderID > 0 ? (object)searchEntity.FolderID : DBNull.Value, true)
            };
            results = GetRecords<Geography>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public int Update(Geography entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Geography>(entity);
            SQL = "usp_GGTools_Taxon_Geography_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            
            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);
            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return RowsAffected;
        }

        protected virtual void BuildInsertUpdateParameters(Geography entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("geography_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }

            AddParameter("current_geography_id", entity.AcceptedID == 0 ? DBNull.Value : (object)entity.AcceptedID, true);
            AddParameter("country_code", (object)entity.CountryCode ?? DBNull.Value, true);
            AddParameter("adm1", (object)entity.Admin1 ?? DBNull.Value, true);
            AddParameter("adm1_type_code", (object)entity.Admin1TypeCode ?? DBNull.Value, true);
            AddParameter("adm1_abbrev", (object)entity.Admin1Abbrev ?? DBNull.Value, true);
            AddParameter("adm2", (object)entity.Admin2 ?? DBNull.Value, true);
            AddParameter("adm2_type_code", (object)entity.Admin2TypeCode ?? DBNull.Value, true);
            AddParameter("adm2_abbrev", (object)entity.Admin2Abbrev ?? DBNull.Value, true);

            if (entity.ChangedDate > DateTime.MinValue)
            {
                AddParameter("changed_date", (object)entity.ChangedDate ?? DBNull.Value, true);
            }
            else
            {
                AddParameter("changed_date", DBNull.Value, true);
            }

            AddParameter("is_valid", (object)entity.IsValid ?? DBNull.Value, false);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, true);
            
            if(entity.ID > 0)
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
            return codeValues;
        }

        //public List<Region> GetRegions()
        //{
        //    List<Region> results = new List<Region>();

        //    SQL = " SELECT r.region_id AS ID, " +
        //        " CONCAT(r.continent, CASE WHEN r.subcontinent IS NULL THEN '' ELSE CONCAT(', ', r.subcontinent) END " +
        //        " ) AS Title FROM region r ORDER BY Title ";
      
        //    results = GetRecords<Region>(SQL);
        //    RowsAffected = results.Count;
        //    return results;
        //}

        public List<Region> GetContinents()
        {
            List<Region> results = new List<Region>();

            SQL = " SELECT ID, Continent FROM [vw_GGTools_GRINGlobal_Regions] WHERE SubContinent IS NULL ";
            SQL += " ORDER BY Continent ASC ";

            results = GetRecords<Region>(SQL);
            RowsAffected = results.Count;
            return results;
        }

        public List<Region> GetSubContinents()
        {
            List<Region> results = new List<Region>();

            SQL = " SELECT ID, SubContinent FROM [vw_GGTools_GRINGlobal_Regions] WHERE SubContinent IS NOT NULL ";
            SQL += " ORDER BY SubContinent ASC ";

            results = GetRecords<Region>(SQL);
            RowsAffected = results.Count;
            return results;
        }

        public List<Country> SearchCountries(GeographySearch searchEntity)
        {
            List<Country> results = new List<Country>();

            SQL = " SELECT DISTINCT CountryCode, CountryName FROM vw_GGTools_GRINGlobal_Geographies ";

            if (!String.IsNullOrEmpty(searchEntity.SubContinentIDList))
            {
                SQL += " WHERE RegionID IN (" + searchEntity.SubContinentIDList + ')';
            }

            if (searchEntity.IncludeNonRegions == "Y")
            {
                SQL += "OR RegionID IS NULL ";
            }
            
            SQL += " ORDER BY CountryName ASC ";

            results = GetRecords<Country>(SQL);
            RowsAffected = results.Count;
            return results;

        }

        public List<Country> GetCountries()
        {
            List<Country> results = new List<Country>();

            SQL = " SELECT * FROM [vw_GGTools_GRINGlobal_Countries]  ";
            SQL += " ORDER BY SortOrder, CountryName ASC ";

            results = GetRecords<Country>(SQL);
            RowsAffected = results.Count;
            return results;
        }

        public List<Geography> GetGeographies()
        {
            List<Geography> results = new List<Geography>();

            SQL = " SELECT * FROM vw_GGTools_GRINGlobal_Geographies ";
            SQL += " ORDER BY Title ASC ";

            results = GetRecords<Geography>(SQL);
            RowsAffected = results.Count;
            return results;
        }
     
        public List<State> GetStates()
        {
            List<State> results = new List<State>();

            SQL = " SELECT * FROM [vw_GGTools_GRINGlobal_States]  ";
            SQL += " ORDER BY SortOrder, StateName ASC ";

            results = GetRecords<State>(SQL);
            RowsAffected = results.Count;
            return results;
        }
    }
}
