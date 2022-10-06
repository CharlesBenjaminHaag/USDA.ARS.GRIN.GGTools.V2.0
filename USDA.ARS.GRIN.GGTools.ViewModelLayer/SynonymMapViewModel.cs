using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class SynonymMapViewModel: SynonymMapViewModelBase, IViewModel<SpeciesSynonymMap>
    {
        public void Delete()
        {
            try
            {
                using (SpeciesSynonymMapManager mgr = new SpeciesSynonymMapManager())
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

        public SpeciesSynonymMap Get(int entityId)
        {
            DataCollection = new Collection<SpeciesSynonymMap>();
            using (SpeciesSynonymMapManager mgr = new SpeciesSynonymMapManager())
            {
                try
                {
                    SearchEntity.ID = entityId;
                    Search();
                    if (DataCollection.Count == 0)
                    {
                        Entity = DataCollection[0];
                    }
                    return Entity;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
                }
            }
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            throw new NotImplementedException();
        }

        public List<SpeciesSynonymMap> InsertMultiple()
        {
            int speciesSynonymMapId = 0;
            string[] speciesIdListSubject = SpeciesIDListSubject.Split(',');
            string[] speciesIdListPredicate = SpeciesIDListPredicate.Split(',');
            List<SpeciesSynonymMap> synonymMaps = new List<SpeciesSynonymMap>();

            using (SpeciesSynonymMapManager mgr = new SpeciesSynonymMapManager())
            {
                foreach (var subjectId in speciesIdListSubject)
                {
                    foreach (var predicateId in speciesIdListPredicate)
                    {
                        SpeciesSynonymMap speciesSynonymMap = new SpeciesSynonymMap();
                        speciesSynonymMap.SpeciesAID = Int32.Parse(subjectId);
                        speciesSynonymMap.SynonymCode = Entity.SynonymCode;
                        speciesSynonymMap.SpeciesBID = Int32.Parse(predicateId);
                        speciesSynonymMapId = mgr.Insert(speciesSynonymMap);
                        
                        // Add new syn map record to a list of recs created in the current session.
                        SpeciesSynonymMap speciesSynonymMap1 = mgr.Get(speciesSynonymMapId);
                        synonymMaps.Add(speciesSynonymMap1);
                    }
                }
            }
            DataCollection = new Collection<SpeciesSynonymMap>(synonymMaps);
            return synonymMaps;
        }

        public void Search()
        {
            using (SpeciesSynonymMapManager mgr = new SpeciesSynonymMapManager())
            {
                try
                {
                    DataCollection = new Collection<SpeciesSynonymMap>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;

                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }

                    //String DEBUG = SerializeToXml<CitationSearch>(SearchEntity);
                    //CitationSearch DEBUG2 = Deserialize<CitationSearch>(DEBUG);

                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
