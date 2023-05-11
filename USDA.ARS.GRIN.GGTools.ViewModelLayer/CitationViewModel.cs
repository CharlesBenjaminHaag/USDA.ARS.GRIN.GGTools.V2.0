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
        public void GetFolderItems()
        {
            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    DataCollection = new Collection<Citation>(mgr.GetFolderItems(SearchEntity));
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
                        Entity.IsAcceptedNameOption = ToBool(Entity.IsAcceptedName);
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
                return Entity;
            }
        }

        public void GetSpeciesCitations(int speciesId)
        {
            try
            {
                using (CitationManager mgr = new CitationManager())
                {
                    DataCollection = new Collection<Citation>(mgr.GetSpeciesCitations(speciesId));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateSpeciesCitation(string tableName, int entityId, int citationId, int cooperatorId)
        {
            using (CitationManager mgr = new CitationManager())
            {
                return mgr.UpdateSpeciesCitation(tableName, entityId, citationId, cooperatorId);
            }
        }

        public int Insert()
        {
            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    Entity.IsAcceptedName = FromBool(Entity.IsAcceptedNameOption);
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

        public int Update()
        {
            using (CitationManager mgr = new CitationManager())
            {
                try
                {
                    Entity.IsAcceptedName = FromBool(Entity.IsAcceptedNameOption);
                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
            return RowsAffected;
        }
       
        public override bool Validate()
        {
            bool validated = true;

            if ((Entity.FamilyID == 0) && (Entity.GenusID == 0) && (Entity.SpeciesID == 0))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "You must specify a taxon." });
            }

            if ((String.IsNullOrEmpty(Entity.CitationTitle)) && (String.IsNullOrEmpty(Entity.Abbreviation)))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "You must specify either a title, or an abbreviated literature source." });
            }

            if (ValidationMessages.Count > 0)
            {
                validated = false;
            }
            return validated;
        }

    }
}
