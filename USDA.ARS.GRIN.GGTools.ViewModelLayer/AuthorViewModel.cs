using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class AuthorViewModel : AuthorViewModelBase, IViewModel<AuthorViewModel>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Author Get(int entityId)
        {
            try
            {
                using (AuthorManager mgr = new AuthorManager())
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

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            using (AuthorManager mgr = new AuthorManager())
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

        public void Search()
        {
            using (AuthorManager mgr = new AuthorManager())
            {
                try
                {
                    if (!String.IsNullOrEmpty(SearchEntity.AuthorityText))
                    {
                        DataCollection = new Collection<Author>(mgr.SearchTaxa(SearchEntity.TableName, SearchEntity.AuthorityText));
                    }
                    else 
                    {
                        DataCollection = new Collection<Author>(mgr.Search(SearchEntity));
                    }
                    Entity = DataCollection[0];
                    RowsAffected = mgr.RowsAffected;

                    //String DEBUG = SerializeToXml<CitationSearch>(SearchEntity);
                    //CitationSearch DEBUG2 = Deserialize<CitationSearch>(DEBUG);

                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
        }

        public List<CodeValue> SearchNotes()
        {
            List<CodeValue> codeValues = new List<CodeValue>();
            using (CropForCWRManager mgr = new CropForCWRManager())
            {
                DataCollectionNotes = new Collection<CodeValue>(mgr.SearchNotes(SearchEntity.TableName, SearchEntity.Note));
            }
            return codeValues;
        }

        public int Update()
        {
            using (AuthorManager mgr = new AuthorManager())
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

        public void SearchFolderItems()
        {
            using (AuthorManager mgr = new AuthorManager())
            {
                try
                {
                    DataCollection = new Collection<Author>(mgr.SearchFolderItems(SearchEntity));
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

        AuthorViewModel IViewModel<AuthorViewModel>.Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<AuthorViewModel> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
