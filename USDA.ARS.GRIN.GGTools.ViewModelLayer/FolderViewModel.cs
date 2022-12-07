using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class FolderViewModel: FolderViewModelBase, IViewModel<Folder>
    {
        public string ItemViewName { get; set; }
        public FolderViewModel()
        { }
        public FolderViewModel(int cooperatorId, string tableName) : base(cooperatorId, tableName)
        { 
        
        }

        public void HandleRequest()
        {
        }

        public int Search()
        {
            using (FolderManager mgr = new FolderManager())
            {
                try
                { 
                    DataCollection = new Collection<AppUserItemFolder>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return mgr.RowsAffected;
            }
        }
        public int GetSharedFolders(int cooperatorId)
        {
            using (FolderManager mgr = new FolderManager())
            {
                try
                {
                    DataCollectionShared = new Collection<AppUserItemFolder>(mgr.GetSharedFolders(cooperatorId));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
            return RowsAffected;
        }
        public int GetAvailableFolders(int cooperatorId, string tableName)
        {
            using (FolderManager mgr = new FolderManager())
            {
                try
                {
                    DataCollectionAvailableFolders = new Collection<AppUserItemFolder>(mgr.GetAvailableFolders(cooperatorId, tableName));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
            return RowsAffected;
        }
        void IViewModel<Folder>.Search()
        {
            throw new NotImplementedException();
        }
        public AppUserItemFolder Get(int entityId)
        {
            DataCollection = new Collection<AppUserItemFolder>();
            using (FolderManager mgr = new FolderManager())
            {
                SearchEntity.ID = entityId;
                Search();
                if (RowsAffected == 1)
                {
                    Entity = DataCollection[0];
                //    DataCollectionAvailableCooperators = new Collection<Cooperator>(mgr.GetAvailableCollaborators(Entity.ID));
                //    DataCollectionCurrentCooperators = new Collection<Cooperator>(mgr.GetCurrentCollaborators(Entity.ID));
                }
            }
            return Entity;
        }

        public void GetAvailableCollaborators()
        {
            using (FolderManager mgr = new FolderManager())
            {
                DataCollectionAvailableCooperators = new Collection<Cooperator>(mgr.GetAvailableCollaborators(Entity.ID));
            }
        }

        public void GetCurrentCollaborators()
        {
            using (FolderManager mgr = new FolderManager())
            {
                DataCollectionCurrentCooperators = new Collection<Cooperator>(mgr.GetCurrentCollaborators(Entity.ID));
            }
        }

        public void GetFolderItems(int folderId)
        {
            using (FolderManager mgr = new FolderManager())
            {
                //TODO Determine which table to load

                DataCollectionFolderItems = new Collection<AppUserItemList>(mgr.GetFolderItems(folderId));
            }
        }
        public int Insert()
        {
            using (FolderManager mgr = new FolderManager())
            {
                try
                {
                    RowsAffected = mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }
        public int InsertFolderItems()
        {
            using (FolderManager mgr = new FolderManager())
            {
                RowsAffected = mgr.InsertItems(Entity);                
            }
            return 0;
        }
        public int InsertCollaborators()
        {
            using (FolderManager mgr = new FolderManager())
            {
                try
                {
                    string[] itemIdArray = ItemIDList.Split(',');
                    foreach (var itemId in itemIdArray)
                    {
                        int cooperatorId = Int32.Parse(itemId);
                        RowsAffected = mgr.InsertCollaborator(cooperatorId, Entity.ID);
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
        public int DeleteCollaborators()
        {
            using (FolderManager mgr = new FolderManager())
            {
                try
                {
                    string[] itemIdArray = ItemIDList.Split(',');
                    foreach (var itemId in itemIdArray)
                    {
                        int cooperatorId = Int32.Parse(itemId);
                        RowsAffected = mgr.DeleteCollaborator(cooperatorId, Entity.ID);
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
            using (FolderManager mgr = new FolderManager())
            {
                try
                {
                    RowsAffected = mgr.Update(Entity);

                    //If there is an item list, add
                    //TODO

                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }
        public void Delete()
        {
            using (FolderManager mgr = new FolderManager())
            {
                try
                {
                    RowsAffected = mgr.Delete(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        //public string GetCssClass(string tableName)
        //{
        //    string cssClass = String.Empty;
        //    switch (tableName)
        //    {
        //        case "taxonomy_family":
        //            cssClass = "bg-green";
        //            break;
        //        case "taxonomy_genus":
        //            cssClass = "bg-fuschia";
        //            break;
        //        case "taxonomy_species":
        //            cssClass = "bg-green";
        //            break;
        //        case "taxonomy_cwr_crop":
        //            cssClass = "bg-aqua";
        //            break;
        //        case "taxonomy_cwr_map":
        //            cssClass = "bg-teal";
        //            break;
        //        case "taxonomy_cwr_trait":
        //            cssClass = "bg-orange";
        //            break;
        //        case "citation":
        //            cssClass = "bg-maroon";
        //            break;
        //        case "regulation":
        //            cssClass = "bg-purple";
        //            break;
        //        default:
        //            cssClass = "bg-navy";
        //            break;
        //    }
        //    return cssClass;
        //}

        //public List<Folder> SearchNotes(string searchText)
        //{
        //    throw new NotImplementedException();
        //}
        Folder IViewModel<Folder>.Get(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}
