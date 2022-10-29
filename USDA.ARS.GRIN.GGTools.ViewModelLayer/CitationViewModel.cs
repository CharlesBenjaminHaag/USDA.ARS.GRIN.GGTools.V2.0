using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class CitationViewModel : CitationViewModelBase, IViewModel<Citation>
    {
        public void Delete()
        {
            try
            {
                using (GRINGlobalDataManagerBase mgr = new GRINGlobalDataManagerBase())
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

        public void DeleteReference()
        {
            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    RowsAffected = mgr.DeleteReference(Entity.TableName, ReferencedEntityID, Entity.ModifiedByCooperatorID);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
        }

        public Citation Get(int entityId)
        {
            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    SearchEntity.ID = entityId;
                    DataCollection = new Collection<Citation>(mgr.Search(SearchEntity));
                    if (DataCollection.Count == 1)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
                return Entity;
            }
        }

        public void GetTaxonCitations()
        { }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            using (CitationManager mgr = new CitationManager())
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
            }
            return RowsAffected;
        }
        
        public int InsertClone(string tableName, int entityId, int citationId)
        {
            int clonedCitationId = 0;

            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    clonedCitationId = mgr.InsertClone(Entity);

                    switch (tableName)
                    {
                        case "taxonomy_geography_map":
                            GeographyMapViewModel geographyMapViewModel = new GeographyMapViewModel();
                            geographyMapViewModel.SearchEntity.ID = entityId;
                            geographyMapViewModel.Search();
                            geographyMapViewModel.Entity.CitationID = clonedCitationId;
                            geographyMapViewModel.Update();
                            break;
                        case "taxonomy_use":
                            EconomicUseViewModel economicUseViewModel = new EconomicUseViewModel();
                            economicUseViewModel.SearchEntity.ID = entityId;
                            economicUseViewModel.Search();
                            economicUseViewModel.Entity.CitationID = clonedCitationId;
                            economicUseViewModel.Update();
                            break;
                        case "taxonomy_common_name":
                            CommonNameViewModel commonNameViewModel = new CommonNameViewModel();
                            commonNameViewModel.SearchEntity.ID = entityId;
                            commonNameViewModel.Search();
                            commonNameViewModel.Entity.CitationID = clonedCitationId;
                            commonNameViewModel.Update();
                            break;
                        case "taxonomy_cwr_map":
                            CWRMapViewModel cWRMapViewModel = new CWRMapViewModel();
                            cWRMapViewModel.SearchEntity.ID = entityId;
                            cWRMapViewModel.Search();
                            cWRMapViewModel.Entity.CitationID = clonedCitationId;
                            cWRMapViewModel.Update();
                            break;
                        case "taxonomy_cwr_trait":
                            CWRTraitViewModel cWRTraitViewModel = new CWRTraitViewModel();
                            cWRTraitViewModel.SearchEntity.ID = entityId;
                            cWRTraitViewModel.Search();
                            cWRTraitViewModel.Entity.CitationID = clonedCitationId;
                            cWRTraitViewModel.Update();
                            break;
                    }

                    
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
            return RowsAffected;
        }
        
        public int InsertMultiple()
        {
            int citationId = 0;
            string[] itemIdList = Entity.ItemIDList.Split(',');
           
            //TODO Refactor.
            if (String.IsNullOrEmpty(EntityIDList))
            {
                EntityIDList = ReferencedEntityID.ToString();
            }
            string[] entityIdList = EntityIDList.Split(',');

            using (CitationManager mgr = new CitationManager())
            {
                foreach (var itemId in itemIdList)
                {
                    foreach (var entityId in entityIdList)
                    {
                        Citation citation = new Citation();
                        citation.FamilyID = Entity.FamilyID;
                        citation.GenusID = Entity.GenusID;
                        citation.SpeciesID = Entity.SpeciesID;
                        citation.ID = Int32.Parse(itemId);
                        citation.CreatedByCooperatorID = Entity.CreatedByCooperatorID;
                        citationId = mgr.InsertClone(citation);

                        // TODO Get id of new citation and set citation ID of table to it.
                        switch (Entity.TableName)
                        {
                            case "taxonomy_geography_map":
                                GeographyMapViewModel geographyMapViewModel = new GeographyMapViewModel();
                                geographyMapViewModel.SearchEntity.ID = Int32.Parse(entityId);
                                geographyMapViewModel.Search();
                                geographyMapViewModel.Entity.CitationID = citationId;
                                geographyMapViewModel.Update();
                                break;
                            case "taxonomy_use":
                                EconomicUseViewModel economicUseViewModel = new EconomicUseViewModel();
                                economicUseViewModel.SearchEntity.ID = ReferencedEntityID;
                                economicUseViewModel.Search();
                                economicUseViewModel.Entity.CitationID = citationId;
                                economicUseViewModel.Update();
                                break;
                            case "taxonomy_common_name":
                                CommonNameViewModel commonNameViewModel = new CommonNameViewModel();
                                commonNameViewModel.SearchEntity.ID = ReferencedEntityID;
                                commonNameViewModel.Search();
                                commonNameViewModel.Entity.CitationID = citationId;
                                commonNameViewModel.Update();
                                break;
                            case "taxonomy_cwr_map":
                                CWRMapViewModel cWRMapViewModel = new CWRMapViewModel();
                                cWRMapViewModel.SearchEntity.ID = ReferencedEntityID;
                                cWRMapViewModel.Search();
                                cWRMapViewModel.Entity.CitationID = citationId;
                                cWRMapViewModel.Update();
                                break;
                            case "taxonomy_cwr_trait":
                                CWRTraitViewModel cWRTraitViewModel = new CWRTraitViewModel();
                                cWRTraitViewModel.SearchEntity.ID = ReferencedEntityID;
                                cWRTraitViewModel.Search();
                                cWRTraitViewModel.Entity.CitationID = citationId;
                                cWRTraitViewModel.Update();
                                break;
                        }
                    }
                }
            }
            return RowsAffected;
        }
        public void Search()
        {
            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    DataCollection = new Collection<Citation>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
                }
            }
        }

        public void SearchFolderItems()
        {
            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    DataCollection = new Collection<Citation>(mgr.SearchFolderItems(SearchEntity));
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

        public List<Citation> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
            return RowsAffected;
        }
        public void UpdateReference()
        {
            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    string[] entityIdList = EntityIDList.Split(',');
                    Entity.ID = Int32.Parse(entityIdList[0]);
                    RowsAffected = mgr.UpdateReference(Entity.TableName, ReferencedEntityID, Entity.ID, Entity.ModifiedByCooperatorID);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
    }
}
