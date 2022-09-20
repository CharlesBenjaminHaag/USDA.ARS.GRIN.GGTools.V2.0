using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.Caching;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class GeographyViewModel : GeographyViewModelBase, IViewModel<GeographyViewModel>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Geography Get(int entityId)
        {
            try
            {
                using (GeographyManager mgr = new GeographyManager())
                {
                    try
                    {
                        SearchEntity.ID = entityId;
                        Search();
                        if (RowsAffected == 1)
                        {
                            Entity = DataCollection[0];
                            Entity.IsValidOption = ToBool(Entity.IsValid);
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

        //public List<Region> GetRegions()
        //{
        //    List<Region> regions = new List<Region>();

        //    ObjectCache cache = MemoryCache.Default;
        //    regions = cache["DATA-LIST-GEOGRAPHY-REGIONS"] as List<Region>;

        //    if (regions == null)
        //    {
        //        CacheItemPolicy policy = new CacheItemPolicy();
        //        using (GeographyManager mgr = new GeographyManager())
        //        {
        //            regions = mgr.GetRegions();
        //        }
        //        cache.Set("DATA-LIST-GEOGRAPHY-REGIONS", regions, policy);
        //    }
        //    return regions;
        //}

        public List<Region> GetContinents()
        {
            List<Region> regions = new List<Region>();

            ObjectCache cache = MemoryCache.Default;
            regions = cache["DATA-LIST-GEOGRAPHY-CONTINENTS"] as List<Region>;

            if (regions == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                using (GeographyManager mgr = new GeographyManager())
                {
                    regions = mgr.GetContinents();
                }
                cache.Set("DATA-LIST-GEOGRAPHY-CONTINENTS", regions, policy);
            }
            return regions;
        }

        public List<Region> GetSubContinents()
        {
            List<Region> regions = new List<Region>();

            ObjectCache cache = MemoryCache.Default;
            regions = cache["DATA-LIST-GEOGRAPHY-SUBCONTINENTS"] as List<Region>;

            if (regions == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                using (GeographyManager mgr = new GeographyManager())
                {
                    regions = mgr.GetSubContinents();
                }
                cache.Set("DATA-LIST-GEOGRAPHY-SUBCONTINENTS", regions, policy);
            }
            return regions;
        }

        public List<Country> GetCountries()
        {
            List<Country> countries = new List<Country>();

            ObjectCache cache = MemoryCache.Default;
            countries = cache["DATA-LIST-GEOGRAPHY-COUNTRIES"] as List<Country>;

            if (countries == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                using (GeographyManager mgr = new GeographyManager())
                {
                    countries = mgr.GetCountries();
                }
                cache.Set("DATA-LIST-GEOGRAPHY-COUNTRIES", countries, policy);
            }
            return countries;
        }

        public List<Geography> GetGeographies()
        {
            List<Geography> geographies = new List<Geography>();

            ObjectCache cache = MemoryCache.Default;
            geographies = cache["DATA-LIST-GEOGRAPHY-GEOGRAPHIES"] as List<Geography>;

            if (geographies == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                using (GeographyManager mgr = new GeographyManager())
                {
                    geographies = mgr.GetGeographies();
                }
                cache.Set("DATA-LIST-GEOGRAPHY-GEOGRAPHIES", geographies, policy);
            }
            return geographies;
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }


        public void Insert()
        {
            using (GeographyManager mgr = new GeographyManager())
            {
                try
                {
                    Entity.IsValid = FromBool(Entity.IsValidOption);
                    RowsAffected = mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
        }

        public void Update()
        {
            using (GeographyManager mgr = new GeographyManager())
            {
                try
                {
                    Entity.IsValid = FromBool(Entity.IsValidOption);
                    mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        public void Search()
        {
            using (GeographyManager mgr = new GeographyManager())
            {
                try
                {
                    DataCollection = new Collection<Geography>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
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

        //public void SearchSubContinents(GeographySearch searchEntity)
        //{
        //    using (GeographyManager mgr = new GeographyManager())
        //    {
        //        //DataCollectionSubContinents = new Collection<Region>(mgr.SearchSubContinents(searchEntity));
        //    }
        //}

        public void SearchCountries(GeographySearch searchEntity)
        {
            using (GeographyManager mgr = new GeographyManager())
            {
                DataCollectionCountries = new Collection<Country>(mgr.SearchCountries(searchEntity));
            }
        }

        public void SearchFolderItems()
        {
            using (GeographyManager mgr = new GeographyManager())
            {
                try
                {
                    DataCollection = new Collection<Geography>(mgr.SearchFolderItems(SearchEntity));
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

        GeographyViewModel IViewModel<GeographyViewModel>.Get(int entityId)
        {
            throw new NotImplementedException();
        }

        int IViewModel<GeographyViewModel>.Insert()
        {
            throw new NotImplementedException();
        }

        int IViewModel<GeographyViewModel>.Update()
        {
            throw new NotImplementedException();
        }

        public List<GeographyViewModel> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
