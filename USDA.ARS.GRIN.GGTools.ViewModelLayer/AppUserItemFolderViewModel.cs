using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class AppUserItemFolderViewModel: AppUserItemFolderViewModelBase
    {
        public AppUserItemFolderViewModel()
        { }
        public AppUserItemFolderViewModel(int cooperatorId) : base(cooperatorId)
        {}
        public int Insert()
        {
            using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
            {
                try
                {
                    Entity.ID = mgr.Insert(Entity);
                    AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
                    foreach (var entityId in EntityIDList.Split(','))
                    {
                        AppUserItemList appUserItemList = new AppUserItemList();
                        appUserItemList.AppUserItemFolderID = Entity.FolderID;
                        appUserItemList.ItemTitle = Entity.FolderName;
                        appUserItemList.EntityID = Int32.Parse(entityId);
                        appUserItemList.CreatedByCooperatorID = Entity.CreatedByCooperatorID;
                        appUserItemListViewModel.Insert();
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }
        public int Update()
        {
            using (AppUserItemFolderManager mgr = new AppUserItemFolderManager())
            {
                try
                {
                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }
    }
}
