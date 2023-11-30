using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysDynamicQueryViewModel: SysDynamicQueryViewModelBase
    {
        public void Search()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            using (SysDynamicQueryManager mgr = new SysDynamicQueryManager())
            {
                DataCollectionDataTable = mgr.Search(SearchEntity);
            }
        }
        public void Get()
        {
            AppUserItemFolderViewModel appUserItemFolderViewModel = new AppUserItemFolderViewModel();
            AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();

            appUserItemFolderViewModel.SearchEntity.ID = Entity.ID;
            appUserItemFolderViewModel.Get();

            appUserItemListViewModel.SearchEntity.AppUserItemFolderID = Entity.ID;
            appUserItemListViewModel.Search();

            PageTitle = "Edit Saved Search";
            Entity.ParentID = appUserItemListViewModel.Entity.ID;
            Entity.ID = appUserItemFolderViewModel.Entity.ID;
            Entity.Title = appUserItemFolderViewModel.Entity.FolderName;
            Entity.Description = appUserItemFolderViewModel.Entity.Description;
            Entity.SQLStatement = appUserItemListViewModel.Entity.Properties;
            Entity.CreatedByCooperatorID = appUserItemFolderViewModel.Entity.CreatedByCooperatorID;
            Entity.CreatedByCooperatorName = appUserItemFolderViewModel.Entity.CreatedByCooperatorName;
            Entity.CreatedDate = appUserItemFolderViewModel.Entity.CreatedDate;
            Entity.ModifiedByCooperatorID = appUserItemFolderViewModel.Entity.ModifiedByCooperatorID;
            Entity.ModifiedByCooperatorName = appUserItemFolderViewModel.Entity.ModifiedByCooperatorName;
            Entity.ModifiedDate = appUserItemFolderViewModel.Entity.ModifiedDate;
        }
        public void SaveSearch()
        {
            AppUserItemFolderViewModel appUserItemFolderViewModel = new AppUserItemFolderViewModel();
            appUserItemFolderViewModel.Entity.FolderName = Entity.Title;
            appUserItemFolderViewModel.Entity.Description = Entity.Description;
            appUserItemFolderViewModel.Entity.Category = "";
            appUserItemFolderViewModel.Entity.FolderType = "SQLQUERY";
            appUserItemFolderViewModel.Entity.DataType = TableName;
            appUserItemFolderViewModel.Entity.CreatedByCooperatorID = Entity.CreatedByCooperatorID;
            appUserItemFolderViewModel.Insert();

            if (appUserItemFolderViewModel.Entity.ID <= 0)
            {
                throw new IndexOutOfRangeException("Error adding new folder.");
            }

            AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
            appUserItemListViewModel.Entity.AppUserItemFolderID = appUserItemFolderViewModel.Entity.ID;
            appUserItemListViewModel.Entity.CooperatorID = AuthenticatedUserCooperatorID;
            appUserItemListViewModel.Entity.TabName = "GGTools Taxon Editor";
            appUserItemListViewModel.Entity.ListName = appUserItemFolderViewModel.Entity.FolderName;
            appUserItemListViewModel.Entity.IDNumber = 0;
            appUserItemListViewModel.Entity.IDType = "FOLDER";
            appUserItemListViewModel.Entity.SortOrder = 0;
            appUserItemListViewModel.Entity.Title = appUserItemFolderViewModel.Entity.FolderName;
            appUserItemListViewModel.Entity.Description = "Added in GGTools Taxonomy Editor";
            appUserItemListViewModel.Entity.CreatedByCooperatorID = Entity.CreatedByCooperatorID;
            appUserItemListViewModel.Entity.Properties = Entity.SQLStatement;
            appUserItemListViewModel.Insert();
        }
        public void Insert()
        { }

        public void Update()
        { }

        public override bool Validate()
        {
            bool validated = true;

            if (String.IsNullOrEmpty(SearchEntity.SQLStatement))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "Please enter a valid SQL statement." });
            }
            else
            {
                if ((SearchEntity.SQLStatement.Contains("INSERT")) ||
                    (SearchEntity.SQLStatement.Contains("UPDATE")) ||
                    (SearchEntity.SQLStatement.Contains("DELETE")))
                {
                    ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "This tool cannot be used to execute INSERT, UPDATE, or DELETE statements." });
                }
            }

            if (ValidationMessages.Count > 0)
            {
                validated = false;
            }
            return validated;
        }

        public void Clean()
        {
            if (!String.IsNullOrEmpty(SearchEntity.SQLStatement))
            {
                SearchEntity.SQLStatement = SearchEntity.SQLStatement.Replace('"',' ');
                SearchEntity.SQLStatement = SearchEntity.SQLStatement.Replace("from", "FROM");
            }
        }

        /// <summary>
        /// Parses SQL statement to determine the driving table used in the query, based on position
        /// relative to
        /// </summary>
        /// <returns></returns>
        public string GetSQLQueryDrivingTable()
        {
            Regex regex = new Regex(@"\bJOIN\s+(?<Retrieve>[a-zA-Z\._\d\[\]]+)\b|\bFROM\s+(?<Retrieve>[a-zA-Z\._\d\[\]]+)\b|\bUPDATE\s+(?<Update>[a-zA-Z\._\d]+)\b|\bINSERT\s+(?:\bINTO\b)?\s+(?<Insert>[a-zA-Z\._\d]+)\b|\bTRUNCATE\s+TABLE\s+(?<Delete>[a-zA-Z\._\d]+)\b|\bDELETE\s+(?:\bFROM\b)?\s+(?<Delete>[a-zA-Z\._\d]+)\b");

            var obj = regex.Matches(SearchEntity.SQLStatement);

            foreach (Match m in obj)
            {
                TableName = m.ToString().Substring(m.ToString().IndexOf(" ")).Trim();
            }
            return TableName;
        }

        public SysTableField GetColumnInfo(string sysTableName, string sysTableFieldName)
        {
            SysTableField sysTableField = new SysTableField();

            using (SysTableManager mgr = new SysTableManager())
            {
                sysTableField = mgr.GetSysTableField(sysTableName, sysTableFieldName);
            }
            return sysTableField;
        }
    }
}
