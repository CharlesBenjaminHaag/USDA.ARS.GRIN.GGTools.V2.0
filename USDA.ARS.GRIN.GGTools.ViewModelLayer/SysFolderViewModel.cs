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
    public class SysFolderViewModel: SysFolderViewModelBase
    {
        public bool IsFavoriteSelector { get; set; }
        public SysFolderViewModel()
        { }
        public SysFolderViewModel(int cooperatorId) : base(cooperatorId)
        {}
        public void Get()
        {
            
        }
              
        public int Search()
        {
            return 0;
            //using (SysFolderManager mgr = new SysFolderManager())
            //{
            //    try
            //    {
            //        DataCollection = new Collection<SysFolder>(mgr.Search(SearchEntity));
            //        RowsAffected = mgr.RowsAffected;

            //        if (DataCollection.Count == 1)
            //        {
            //            Entity = DataCollection[0];
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        PublishException(ex);
            //        throw ex;
            //    }
            //    return mgr.RowsAffected;
            //}
        }
        public int Insert()
        {
            return 0;
            //using (SysFolderManager mgr = new SysFolderManager())
            //{
            //    try
            //    {
            //        if (!String.IsNullOrEmpty(Entity.NewCategory))
            //        {
            //            Entity.Category = Entity.NewCategory;
            //        }
            //        Entity.ID = mgr.Insert(Entity);
            //        InsertItems();
            //    }
            //    catch (Exception ex)
            //    {
            //        PublishException(ex);
            //        throw ex;
            //    }
            //    return RowsAffected;
            //}
        }
        
       
        public int Update()
        {
            return 0;
            //using (SysFolderManager mgr = new SysFolderManager())
            //{
            //    try
            //    {
            //        if (!String.IsNullOrEmpty(Entity.NewCategory))
            //        {
            //            Entity.Category = Entity.NewCategory;
            //        }
            //        RowsAffected = mgr.Update(Entity);
            //        AppUserItemListViewModel appUserItemListViewModel = new AppUserItemListViewModel();
            //    }
            //    catch (Exception ex)
            //    {
            //        PublishException(ex);
            //        throw ex;
            //    }
            //    return RowsAffected;
            //}
        }
        
        public void Delete()
        {
            try
            {
                //using (SysFolderManager mgr = new SysFolderManager())
                //{
                //    mgr.Delete(TableName, Entity.ID);
                //}
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }
    }
}
