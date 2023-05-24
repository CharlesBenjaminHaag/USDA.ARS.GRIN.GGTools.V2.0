using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class CommonNameLanguageViewModel : CommonNameLanguageViewModelBase, IViewModel<CommonNameLanguage>
    {
        public void Delete()
        {
            try
            {
                using (CommonNameLanguageManager mgr = new CommonNameLanguageManager())
                {
                    mgr.Delete(TableName, Entity.ID);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }

        public CommonNameLanguage Get(int entityId)
        {
            try
            {
                using (CommonNameLanguageManager mgr = new CommonNameLanguageManager())
                {
                    try
                    {
                        SearchEntity.ID = entityId;
                        Search();
                    }
                    catch (Exception ex)
                    {
                        PublishException(ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
            return Entity;
        }

        public void GetFolderItems()
        {
            using (CommonNameLanguageManager mgr = new CommonNameLanguageManager())
            {
                try
                {
                    DataCollection = new Collection<CommonNameLanguage>(mgr.GetFolderItems(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            using (CommonNameLanguageManager mgr = new CommonNameLanguageManager())
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

        public void Search()
        {
            using (CommonNameLanguageManager mgr = new CommonNameLanguageManager())
            {
                try
                {
                    DataCollection = new Collection<CommonNameLanguage>(mgr.Search(SearchEntity));
                    if (DataCollection.Count == 1)
                    {
                        Entity = DataCollection[0];
                    }
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public List<CommonNameLanguage> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            using (CommonNameLanguageManager mgr = new CommonNameLanguageManager())
            {
                try
                {
                    if (!String.IsNullOrEmpty(Entity.ItemIDList))
                    {
                        foreach (var id in Entity.ItemIDList.Split(','))
                        {
                            Entity.ID = Int32.Parse(id);
                            mgr.UpdateCountry(Int32.Parse(id), Entity.CountryCode, Entity.ModifiedByCooperatorID);
                        }
                    }
                    else
                    {
                        RowsAffected = mgr.Update(Entity);
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
    }
}
