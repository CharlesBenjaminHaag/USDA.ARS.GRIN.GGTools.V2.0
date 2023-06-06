using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class CWRMapViewModel: CWRMapViewModelBase, IViewModel<CWRMap>
    {
        public override bool Validate()
        {
            bool validated = true;

            Entity.IsCrop = FromBool(Entity.IsCropOption);

            if (Entity.CropForCWRID == 0)
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "Please select a crop." });
            }

            if (Entity.SpeciesID == 0)
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "Please select a species." });
            }

            if ((Entity.IsCrop == "Y") && (String.IsNullOrEmpty(Entity.CropCommonName)))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "Because you have designated this map as a crop, you must specify a common name." });
            }

            if (ValidationMessages.Count > 0)
                validated = false;

            return validated;
        }
        public void HandleRequest()
        {
           
        }

        public CWRMap Get(int entityId)
        {
            try
            {
                using (CWRMapManager mgr = new CWRMapManager())
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
            using (CWRMapManager mgr = new CWRMapManager())
            {
                try
                {
                    DataCollection = new Collection<CWRMap>(mgr.GetFolderItems(SearchEntity));
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
        public int Search()
        {
            using (CWRMapManager mgr = new CWRMapManager())
            {
                try
                {
                    DataCollection = new Collection<CWRMap>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                        Entity.IsCropOption = ToBool(Entity.IsCrop);
                        Entity.IsGraftStockOption = ToBool(Entity.IsGraftstock);
                        Entity.IsPotentialOption = ToBool(Entity.IsPotential);
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return 0;
            }
        }

        public void SearchFolderItems()
        {
            using (CWRMapManager mgr = new CWRMapManager())
            {
                try
                {
                    DataCollection = new Collection<CWRMap>(mgr.SearchFolderItems(SearchEntity));
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

        public void Save()
        {
            if (Entity.ID > 0)
            {
                Update();
            }
            else
            {
                Insert();
            }
        }

        public void InsertBatch()
        {
            string[] itemIDList = Entity.ItemIDList.Split(',');
            foreach (var itemId in itemIDList)
            {
                CWRMap cWRMap = new CWRMap();
                cWRMap.CropForCWRID = Entity.CropForCWRID;
                cWRMap.SpeciesID = Int32.Parse(itemId);
                cWRMap.GenepoolCode = Entity.GenepoolCode;
                cWRMap.CropCommonName = Entity.CropCommonName;
                cWRMap.CreatedByCooperatorID = Entity.CreatedByCooperatorID;
                Entity = cWRMap;
                Insert();
            }
        }

        public void Insert()
        {
            using (CWRMapManager mgr = new CWRMapManager())
            {
                try
                {
                    RowsAffected = mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
                }
            }
        }

        public void Update()
        {
            using (CWRMapManager mgr = new CWRMapManager())
            {
                try
                {
                    mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
        }

        void IViewModel<CWRMap>.Search()
        {
            throw new NotImplementedException();
        }

        int IViewModel<CWRMap>.Insert()
        {
            throw new NotImplementedException();
        }

        int IViewModel<CWRMap>.Update()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            try
            {
                using (CWRMapManager mgr = new CWRMapManager())
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

        public List<CodeValue> SearchNotes()
        {
            List<CodeValue> codeValues = new List<CodeValue>();
            using (CWRMapManager mgr = new CWRMapManager())
            {
                DataCollectionNotes = new Collection<CodeValue>(mgr.SearchNotes(SearchEntity.TableName, SearchEntity.Note));
            }
            return codeValues;
        }

        public List<CWRMap> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
