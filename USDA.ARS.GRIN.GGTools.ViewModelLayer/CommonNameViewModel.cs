using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class CommonNameViewModel : CommonNameViewModelBase, IViewModel<CommonName>
    {
        public void Delete()
        {
            try
            {
                using (SpeciesManager mgr = new SpeciesManager())
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

        public CommonName Get(int entityId)
        {
            try
            {
                using (CommonNameManager mgr = new CommonNameManager())
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
            using (CommonNameManager mgr = new CommonNameManager())
            {
                try
                {
                    DataCollection = new Collection<CommonName>(mgr.GetFolderItems(SearchEntity));
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
            using (CommonNameManager mgr = new CommonNameManager())
            {
                try
                {
                    SetSimplifiedName();
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

        public int Update()
        {
            using (CommonNameManager mgr = new CommonNameManager())
            {
                try
                {
                    SetSimplifiedName();
                    mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
                return RowsAffected;
            }
        }

        public void Search()
        {
            using (CommonNameManager mgr = new CommonNameManager())
            {
                try
                {
                    DataCollection = new Collection<CommonName>(mgr.Search(SearchEntity));
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

        public void SearchFolderItems()
        {
            using (CommonNameManager mgr = new CommonNameManager())
            {
                try
                {
                    DataCollection = new Collection<CommonName>(mgr.SearchFolderItems(SearchEntity));
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

        public List<CommonName> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public override bool Validate()
        {
            bool validated = true;

            if ((Entity.SpeciesID == 0) && (Entity.GenusID == 0))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "Please identify either a species or genus in reference to this common name." });
            }

            if (String.IsNullOrEmpty(Entity.Name))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The name is required." });
            }

            if (ValidationMessages.Count > 0)
            {
                validated = false;
            }
            return validated;
        }

    }
}
