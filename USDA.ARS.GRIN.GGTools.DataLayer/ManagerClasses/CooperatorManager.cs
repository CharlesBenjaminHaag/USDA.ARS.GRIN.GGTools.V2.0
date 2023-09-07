using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Linq;
using System.Configuration;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class CooperatorManager : AppDataManagerBase, IManager<Cooperator, CooperatorSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(Cooperator entity)
        {
            throw new NotImplementedException();
        }

        public Cooperator Get(int entityId, string environment = "")
        {
            List<Cooperator> cooperators = new List<Cooperator>();
            Cooperator cooperator = new Cooperator();

            CooperatorSearch cooperatorSearch = new CooperatorSearch();
            cooperatorSearch.Environment = environment;
            cooperatorSearch.ID = entityId;
            cooperators = Search(cooperatorSearch);
            if (cooperators.Count == 1)
            {
                cooperator = cooperators[0];
            }
            return cooperator;
        }

        public CooperatorStatus GetStatus(int entityId)
        {
            CooperatorStatus cooperatorStatus = new CooperatorStatus();

            SQL = "usp_GRINGlobal_Cooperator_Status_Select";

            var parameters = new List<IDbDataParameter> {
                    CreateParameter("cooperator_id", (object)entityId, false)
                };

            cooperatorStatus = GetRecord<CooperatorStatus>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return cooperatorStatus;
        }
        public List<Cooperator> GetSiteCurators(string siteShortName)
        {
            List<Cooperator> cooperators = new List<Cooperator>();
            SQL = "usp_GRINGlobal_SiteCurators_Select";

            var parameters = new List<IDbDataParameter> {
                    CreateParameter("site_short_name", (object)siteShortName, false)
                };

            cooperators = GetRecords<Cooperator>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return cooperators;
        }
        
        public List<ReportItem> GetRecordsOwned(int cooperatorId)
        {
            List<ReportItem> reportItems = new List<ReportItem>();
            SQL = "usp_GGTools_GRINGlobal_RecordsOwnedByCooperator_Select";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("cooperator_id", (object)cooperatorId, false)
            };

            reportItems = GetRecords<ReportItem>(SQL, CommandType.StoredProcedure, parameters.ToArray());

            double totalOwned = reportItems.Sum(x => x.Total);

            return reportItems;
        }

        public int Insert(Cooperator entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Cooperator>(entity);
            SQL = "usp_GRINGlobal_Cooperator_Insert";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_cooperator_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            entity.ID = GetParameterValue<int>("@out_cooperator_id", -1);
            var errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }

        public List<Cooperator> Search(CooperatorSearch searchEntity)
        {
            List<Cooperator> results = new List<Cooperator>();
            
            SQL = " SELECT * FROM vw_GRINGlobal_Cooperator";
            SQL += " WHERE (@ID                     IS NULL     OR ID                       =       @ID)";
            SQL += " AND (@FirstName                IS NULL     OR FirstName                LIKE    '%' + @FirstName + '%')";
            SQL += " AND (@LastName                 IS NULL     OR LastName                 LIKE    '%' + @LastName + '%')";
            SQL += " AND (@EmailAddress             IS NULL     OR EmailAddress             LIKE    '%' + @EmailAddress + '%')";
            SQL += " AND (@StatusCode               IS NULL     OR StatusCode               =       @StatusCode)";
            SQL += " AND (@SiteID                   IS NULL     OR SiteID                   =       @SiteID)";
            SQL += " AND (@SysUserIsEnabled         IS NULL     OR SysUserIsEnabled         =       @SysUserIsEnabled)";
        
            // Search by pre-defined time frame
            switch (searchEntity.CreatedTimeFrame)
            {
                case "TDY":
                    SQL += " AND (CONVERT(date, CreatedDate) = CONVERT(date, GETDATE()))";
                    break;
                case "3DY":
                    SQL += " AND (CreatedDate >= CAST(DATEADD(day, -3, GETDATE()) AS date) ";
                    SQL += " AND CreatedDate < CAST(GETDATE() AS date)) ";
                    break;
                case "7DY":
                    SQL += " AND (CreatedDate >= CAST(DATEADD(day, -7, GETDATE()) AS date) ";
                    SQL += " AND CreatedDate < CAST(GETDATE() AS date)) ";
                    break;
                case "30D":
                    SQL += " AND (CreatedDate >= CAST(DATEADD(day, -30, GETDATE()) AS date) ";
                    SQL += " AND CreatedDate < CAST(GETDATE() AS date)) ";
                    break;
                case "60D":
                    SQL += " AND (CreatedDate >= CAST(DATEADD(day, -60, GETDATE()) AS date) ";
                    SQL += " AND CreatedDate < CAST(GETDATE() AS date)) ";
                    break;
            }

            // Search by pre-defined time frame
            switch (searchEntity.ModifiedTimeFrame)
            {
                case "TDY":
                    SQL += " AND (CONVERT(date, ModifiedDate) = CONVERT(date, GETDATE()))";
                    break;
                case "3DY":
                    SQL += " WHERE CreatedDate >= CAST(DATEADD(day, -3, GETDATE()) AS date) ";
                    SQL += " AND CreatedDate<CAST(GETDATE() AS date) ";
                    break;
                case "7DY":
                    SQL += " WHERE CreatedDate >= CAST(DATEADD(day, -7, GETDATE()) AS date) ";
                    SQL += " AND CreatedDate<CAST(GETDATE() AS date) ";
                    break;
                case "30D":
                    SQL += " WHERE CreatedDate >= CAST(DATEADD(day, -30, GETDATE()) AS date) ";
                    SQL += " AND CreatedDate<CAST(GETDATE() AS date) ";
                    break;
                case "60D":
                    SQL += " WHERE CreatedDate >= CAST(DATEADD(day, -60, GETDATE()) AS date) ";
                    SQL += " AND CreatedDate<CAST(GETDATE() AS date) ";
                    break;
            }
            
            // If sys group tag specified, find all cooperators in the specified group.
            if (!String.IsNullOrEmpty(searchEntity.SysGroupTag))
            {
                SQL += " AND SysUserID IN (SELECT SysUserID FROM vw_GRINGlobal_Sys_Group_User_Map WHERE GroupTag = '" + searchEntity.SysGroupTag + "') ";
            }

            SQL += " ORDER BY FullName";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("FirstName", (object)searchEntity.FirstName ?? DBNull.Value, true),
                CreateParameter("LastName", (object)searchEntity.LastName ?? DBNull.Value, true),
                CreateParameter("EmailAddress", (object)searchEntity.EmailAddress ?? DBNull.Value, true),
                CreateParameter("StatusCode", (object)searchEntity.StatusCode ?? DBNull.Value, true),
                CreateParameter("SiteID", searchEntity.SiteID > 0 ? (object)searchEntity.SiteID : DBNull.Value, true),
                CreateParameter("SysUserIsEnabled", (object)searchEntity.SysUserIsEnabled ?? DBNull.Value, true),
            };

            results = GetRecords<Cooperator>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public List<Cooperator> GetBySysGroup(string sysGroupTag)
        {
            List<Cooperator> cooperators = new List<Cooperator>();
            SQL = "usp_GRINGlobal_Cooperators_By_Sys_Group_Select";

            var parameters = new List<IDbDataParameter> {
                    CreateParameter("group_tag", (object)sysGroupTag, false)
                };

            cooperators = GetRecords<Cooperator>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return cooperators;
        }

        public int Update(Cooperator entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<Cooperator>(entity);
            SQL = "usp_GRINGLobal_Cooperator_Update";

            BuildInsertUpdateParameters(entity);

            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            RowsAffected = ExecuteNonQuery();

            var errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
                throw new Exception(errorNumber.ToString());

            return entity.ID;
        }

        public int Transfer(int sourceCooperatorID, int targetCooperatorId, string tableName)
        {
            Reset(CommandType.StoredProcedure);
            SQL = "usp_GGTools_GRINGlobal_CooperatorRecordOwnershipByTable_Update";

            AddParameter("sys_table_name", tableName, true);
            AddParameter("target_cooperator_id", targetCooperatorId, true);
            AddParameter("source_cooperator_id", sourceCooperatorID, true);

            RowsAffected = ExecuteNonQuery();

            //var errorNumber = GetParameterValue<int>("@out_error_number", -1);

            //if (errorNumber > 0)
            //    throw new Exception(errorNumber.ToString());

            return RowsAffected;
        }

        public List<CodeValue> GetTimeFrameOptions()
        {
            List<CodeValue> timeFrameOptions = new List<CodeValue>();
            timeFrameOptions.Add(new CodeValue { Value = "TDY", Title = "Today" });
            timeFrameOptions.Add(new CodeValue { Value = "3DY", Title = "Within the last 3 days" });
            timeFrameOptions.Add(new CodeValue { Value = "7DY", Title = "Within the last 7 days" });
            timeFrameOptions.Add(new CodeValue { Value = "30D", Title = "Within the last 30 days" });
            timeFrameOptions.Add(new CodeValue { Value = "60D", Title = "Within the last 60 days" });
            return timeFrameOptions;
        }

        public List<SysGroup> GetGroups(int entityId)
        {
            List<SysGroup> groups = new List<SysGroup>();
            SQL = "SELECT * FROM vw_GGTools_GRINGlobal_SysGroupUserMaps " +
                " WHERE SysUserID = @SysUserID";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("SysUserID", entityId, true),
            };
            groups = GetRecords<SysGroup>(SQL, parameters.ToArray());
            return groups;
        }

        public List<State> GetStates()
        {
            List<State> states = new List<State>();
            SQL = "SELECT ID, Admin1 FROM vw_GRINGlobal_Geography_State ORDER BY Admin1";
            states = GetRecords<State>(SQL);
            return states;
        }
        public List<CodeValue> GetOrganizations()
        {
            List<CodeValue> states = new List<CodeValue>();
            SQL = "SELECT OrganizationAbbrev AS Value, Organization AS Title FROM vw_GGTools_GRINGlobal_Organizations";
            List<CodeValue> codeValues = GetRecords<CodeValue>(SQL);
            return codeValues;
        }
        public List<Site> GetSites()
        {
            List<Site> sites = new List<Site>();
            SQL = "SELECT * FROM vw_GRINGlobal_Site ORDER BY LongName";
            sites = GetRecords<Site>(SQL);
            return sites;
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
        public void BuildInsertUpdateParameters(Cooperator entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("cooperator_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
                AddParameter("web_cooperator_id", entity.WebCooperatorID == 0 ? DBNull.Value : (object)entity.WebCooperatorID, true);
            }
            AddParameter("site_id", entity.SiteID == 0 ? DBNull.Value : (object)entity.SiteID, true);
            AddParameter("last_name", String.IsNullOrEmpty(entity.LastName) ? DBNull.Value : (object)entity.LastName, true);
            AddParameter("title", String.IsNullOrEmpty(entity.Salutation) ? DBNull.Value : (object)entity.Salutation, true);
            AddParameter("first_name", String.IsNullOrEmpty(entity.FirstName) ? DBNull.Value : (object)entity.FirstName, true);
            AddParameter("job", String.IsNullOrEmpty(entity.JobTitle) ? DBNull.Value : (object)entity.JobTitle, true);
            AddParameter("organization", String.IsNullOrEmpty(entity.Organization) ? DBNull.Value : (object)entity.Organization, true);
            AddParameter("organization_abbrev", String.IsNullOrEmpty(entity.OrganizationAbbrev) ? DBNull.Value : (object)entity.OrganizationAbbrev, true);
            AddParameter("address_line1", String.IsNullOrEmpty(entity.AddressLine1) ? DBNull.Value : (object)entity.AddressLine1, true);
            AddParameter("address_line2", String.IsNullOrEmpty(entity.AddressLine2) ? DBNull.Value : (object)entity.AddressLine2, true);
            AddParameter("address_line3", String.IsNullOrEmpty(entity.AddressLine3) ? DBNull.Value : (object)entity.AddressLine3, true);
            AddParameter("city", String.IsNullOrEmpty(entity.City) ? DBNull.Value : (object)entity.City, true);
            AddParameter("postal_index", String.IsNullOrEmpty(entity.PostalIndex) ? DBNull.Value : (object)entity.PostalIndex, true);
            AddParameter("geography_id", entity.GeographyID == 0 ? DBNull.Value : (object)entity.GeographyID, true);
            AddParameter("secondary_organization", String.IsNullOrEmpty(entity.Organization) ? DBNull.Value : (object)entity.Organization, true);
            AddParameter("secondary_organization_abbrev", String.IsNullOrEmpty(entity.OrganizationAbbrev) ? DBNull.Value : (object)entity.OrganizationAbbrev, true);
            AddParameter("secondary_address_line1", String.IsNullOrEmpty(entity.SecondaryAddressLine1) ? DBNull.Value : (object)entity.SecondaryAddressLine1, true);
            AddParameter("secondary_address_line2", String.IsNullOrEmpty(entity.SecondaryAddressLine2) ? DBNull.Value : (object)entity.SecondaryAddressLine2, true);
            AddParameter("secondary_address_line3", String.IsNullOrEmpty(entity.SecondaryAddressLine3) ? DBNull.Value : (object)entity.SecondaryAddressLine3, true);
            AddParameter("secondary_city", String.IsNullOrEmpty(entity.SecondaryCity) ? DBNull.Value : (object)entity.SecondaryCity, true);
            AddParameter("secondary_postal_index", String.IsNullOrEmpty(entity.SecondaryPostalIndex) ? DBNull.Value : (object)entity.SecondaryPostalIndex, true);
            AddParameter("secondary_geography_id", entity.GeographyID == 0 ? DBNull.Value : (object)entity.GeographyID, true);
            AddParameter("primary_phone", String.IsNullOrEmpty(entity.PrimaryPhone) ? DBNull.Value : (object)entity.PrimaryPhone, true);
            AddParameter("secondary_phone", String.IsNullOrEmpty(entity.SecondaryPhone) ? DBNull.Value : (object)entity.SecondaryPhone, true);
            AddParameter("fax", String.IsNullOrEmpty(entity.Fax) ? DBNull.Value : (object)entity.Fax, true);
            AddParameter("email_address", String.IsNullOrEmpty(entity.EmailAddress) ? DBNull.Value : (object)entity.EmailAddress, true);
            AddParameter("secondary_email_address", String.IsNullOrEmpty(entity.SecondaryEmailAddress) ? DBNull.Value : (object)entity.SecondaryEmailAddress, true);
            AddParameter("status_code", String.IsNullOrEmpty(entity.StatusCode) ? DBNull.Value : (object)entity.StatusCode, true);
            AddParameter("category_code", String.IsNullOrEmpty(entity.CategoryCode) ? DBNull.Value : (object)entity.CategoryCode, true);
            AddParameter("organization_region_code", String.IsNullOrEmpty(entity.OrganizationRegionCode) ? DBNull.Value : (object)entity.OrganizationRegionCode, true);
            AddParameter("discipline_code", String.IsNullOrEmpty(entity.DisciplineCode) ? DBNull.Value : (object)entity.DisciplineCode, true);
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

        public Cooperator Get(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
