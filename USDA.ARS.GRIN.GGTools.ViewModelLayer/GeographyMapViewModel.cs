using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class GeographyMapViewModel : GeographyMapViewModelBase, IViewModel<GeographyMap>
    {
        public GeographyMapViewModel()
        {
        }

        public GeographyMapViewModel(int speciesId)
        {
            
        }

        public void Delete()
        {
            try
            {
                using (GeographyMapManager mgr = new GeographyMapManager())
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

        public GeographyMap Get(int entityId)
        {
            try
            {
                using (GeographyMapManager mgr = new GeographyMapManager())
                {
                    try
                    {
                        SearchEntity.ID = entityId;
                        Search();
                        if (DataCollection.Count == 1)
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
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
            return Entity;
        }
        public void GetFolderItems()
        {
            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                try
                {
                    DataCollection = new Collection<GeographyMap>(mgr.GetFolderItems(SearchEntity));
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
        public void GetList()
        {
            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                try
                {
                    EditCollection = mgr.Search(SearchEntity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public void Insert()
        {
            List<string> geographyMapIDList = new List<string>();

            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                try
                {
                    //if (!String.IsNullOrEmpty(ItemIDList))
                    //{
                    //    string[] itemIdList = ItemIDList.Split(',');
                    //    foreach (var itemId in itemIdList)
                    //    {
                    //        GeographyMap geographyMap = new GeographyMap();
                    //        geographyMap.SpeciesID = Entity.SpeciesID;
                    //        geographyMap.GeographyID = Int32.Parse(itemId);
                    //        geographyMap.GeographyStatusCode = Entity.GeographyStatusCode;
                    //        geographyMap.CreatedByCooperatorID = Entity.CreatedByCooperatorID;
                    //        var geographyMapId = mgr.Insert(geographyMap);
                    //        geographyMapIDList.Add(geographyMapId.ToString());
                    //    }
                    //}
                    //else
                    //{
                        RowsAffected = mgr.Insert(Entity);
                        Entity = mgr.Get(Entity.ID);
                        DataCollection.Add(Entity);
                    //}
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
        }

        public void Map()
        {
            var itemIdList = ItemIDList.Split(',');

            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                foreach (var id in itemIdList)
                {
                    GeographyMap geographyMap = new GeographyMap { ID = Entity.ID, SpeciesID = Int32.Parse(id) };
                    mgr.Insert(geographyMap);
                }
            }
        }

        public void Update()
        {
            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                try
                {
                    mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        public JsonResult AddCitation(int citationId, string idList)
        {
            string[] idCollection;
            idCollection = idList.Split(',');
                
            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                foreach (var id in idCollection)
                {
                    int convertedId = Int32.Parse(id);
                    mgr.AddCitation(citationId, convertedId);
                }
            }
            
            //TODO
            return null;
        }

        public void Search()
        {
            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                try
                {
                    DataCollection = new Collection<GeographyMap>(mgr.Search(SearchEntity));
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

        public void SearchFolderItems()
        {
            using (GeographyMapManager mgr = new GeographyMapManager())
            {
                try
                {
                    DataCollection = new Collection<GeographyMap>(mgr.SearchFolderItems(SearchEntity));
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

        public List<CodeValue> SearchNotes()
        {
            List<CodeValue> codeValues = new List<CodeValue>();
            using (CWRMapManager mgr = new CWRMapManager())
            {
                DataCollectionNotes = new Collection<CodeValue>(mgr.SearchNotes(SearchEntity.TableName, SearchEntity.Note));
            }
            return codeValues;
        }

        int IViewModel<GeographyMap>.Insert()
        {
            throw new NotImplementedException();
        }

        int IViewModel<GeographyMap>.Update()
        {
            throw new NotImplementedException();
        }

        public List<GeographyMap> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public void SaveSearch()
        {
            AppUserDynamicQuery appUserDynamicQuery = new AppUserDynamicQuery();
            appUserDynamicQuery.CreatedByCooperatorID = AuthenticatedUserCooperatorID;
            appUserDynamicQuery.Title = SearchEntity.SaveSearchTitle;
            appUserDynamicQuery.Description = SearchEntity.SaveSearchDescription;
            appUserDynamicQuery.DataSource = TableName;
            appUserDynamicQuery.QuerySyntax = SerializeToXml(SearchEntity);
            using (AppUserDynamicQueryManager mgr = new AppUserDynamicQueryManager())
            {
                mgr.Insert(appUserDynamicQuery);
            }
            //TODO
            //Serialize search entity
            //Save to DB

        }
    }
}
