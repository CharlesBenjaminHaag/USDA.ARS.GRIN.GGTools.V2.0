using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

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

            if ((SearchEntity.SQLStatement.Contains("INSERT")) ||
                (SearchEntity.SQLStatement.Contains("UPDATE")) ||
                (SearchEntity.SQLStatement.Contains("DELETE")) )
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "This tool cannot be used to execute INSERT, UPDATE, or DELETE statements." });
            }

            if (ValidationMessages.Count > 0)
            {
                validated = false;
            }
            return validated;
        }
    }
}
