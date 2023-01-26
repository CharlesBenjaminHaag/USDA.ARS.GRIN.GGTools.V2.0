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
    public class SpeciesViewModel : SpeciesViewModelBase, IViewModel<Species>
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

        public void DeleteTag(int tagMapId)
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    RowsAffected = mgr.DeleteTag(tagMapId);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public Species Get(int entityId)
        {
            DataCollection = new Collection<Species>();
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    SearchEntity.ID = entityId;
                    Search();

                    Entity.IsSpecificHybridOption = ToBool(Entity.IsSpecificHybrid);
                    Entity.IsSubSpecificHybridOption = ToBool(Entity.IsSubspecificHybrid);
                    Entity.IsVarietalHybridOption = ToBool(Entity.IsVarietalHybrid);
                    Entity.IsSubvarietalHybridOption = ToBool(Entity.IsSubVarietalHybrid);
                    Entity.IsFormaHybridOption = ToBool(Entity.IsFormaHybrid);
                    Entity.IsAccepted = ToBool(Entity.IsAcceptedName);
                    Entity.IsWebVisibleOption = ToBool(Entity.IsWebVisible);

                    //DataCollectionConspecificTaxa = new Collection<Species>(mgr.GetConspecificTaxa(entityId));
                    //DataCollectionSynonyms = new Collection<Species>(mgr.GetSynonyms(entityId));
                    //DataCollectionCitations = new Collection<Citation>(mgr.GetCitations(entityId));
                    //DataCollectionCommonNames = new Collection<CommonName>(mgr.GetCommonNames(entityId));
                    //DataCollectionEconomicUses = new Collection<EconomicUse>(mgr.GetEconomicUses(entityId));
                    //DataCollectionGeographyMaps = new Collection<GeographyMap>(mgr.GetGeographyMaps(entityId));
                    //DataCollectionRegulationMaps = new Collection<RegulationMap>(mgr.GetRegulationMaps(entityId));
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
                }
            }

            return Entity;
        }

        public void GetConspecific()
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    DataCollection = new Collection<Species>(mgr.GetConspecificTaxa(SearchEntity.ID));
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                }
            }
        }

        public int Insert()
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    Entity.IsSpecificHybrid = FromBool(Entity.IsSpecificHybridOption);
                    Entity.IsSubspecificHybrid = FromBool(Entity.IsSubSpecificHybridOption);
                    Entity.IsVarietalHybrid = FromBool(Entity.IsVarietalHybridOption);
                    Entity.IsSubVarietalHybrid = FromBool(Entity.IsSubvarietalHybridOption);
                    Entity.IsFormaHybrid = FromBool(Entity.IsFormaHybridOption);
                    Entity.IsWebVisible = FromBool(Entity.IsWebVisibleOption);

                    SetSpeciesName();
                    SetSpeciesNameAuthority();
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
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    DataCollection = new Collection<Species>(mgr.Search(SearchEntity));
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

        public void GetFolderItems()
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    DataCollection = new Collection<Species>(mgr.GetFolderItems(SearchEntity));
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

        public void SearchProtologues(string protologue)
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    DataCollectionProtologues = new Collection<CodeValue>(mgr.SearchProtologues(protologue));
                    RowsAffected = mgr.RowsAffected;
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
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    Entity.IsSpecificHybrid = FromBool(Entity.IsSpecificHybridOption);
                    Entity.IsSubspecificHybrid = FromBool(Entity.IsSubSpecificHybridOption);
                    Entity.IsVarietalHybrid = FromBool(Entity.IsVarietalHybridOption);
                    Entity.IsSubVarietalHybrid = FromBool(Entity.IsSubvarietalHybridOption);
                    Entity.IsFormaHybrid = FromBool(Entity.IsFormaHybridOption);
                    Entity.IsWebVisible = FromBool(Entity.IsWebVisibleOption);

                    SetSpeciesName();
                    SetSpeciesNameAuthority();
                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }

            //if ((EventAction == "DEMT") && (DemoteCreateFolder == true))
            //{
            //    FolderViewModel folderVm = new FolderViewModel();
            //    folderVm.Entity.TableName = TableName;
            //    folderVm.Entity.Title = "TO DO: Species Subtaxa Pending Re-Assignment";
            //    folderVm.Entity.Description = "** AUTO-GENERATED ** Contains taxa linked to " + Entity.SpeciesName + ".";
            //    folderVm.Entity.Category = "Priority";
            //    folderVm.Entity.IsFavorite = true;
            //    folderVm.Entity.ItemIDList = DemoteInfo;
            //    folderVm.Entity.CreatedByCooperatorID = Entity.ModifiedByCooperatorID;

            //    //TODO Get comma-sep list of species ID's to add.

            //    using (FolderManager folderMgr = new FolderManager())
            //    {
            //        folderMgr.Insert(folderVm.Entity);
            //    }
            //}
            return RowsAffected;
        }

        public void SetVerificationStatus(string statusCode) 
        {
            Entity = Get(Entity.ID);
            switch (statusCode)
            {
                case "Y":
                    Entity.VerifiedByCooperatorID = Entity.CreatedByCooperatorID;
                    Entity.NameVerifiedDate = DateTime.Now;
                    break;
                case "N":
                    Entity.VerifiedByCooperatorID = 0;
                    Entity.NameVerifiedDate = DateTime.MinValue;
                    break;
            }
            Update();
            Get(Entity.ID);
        }

        public int UpdateVerification()
        {
            using (SpeciesManager mgr = new SpeciesManager())
            {
                try
                {
                    if (Entity.IsVerified == "Y")
                    {
                        Entity.VerifiedByCooperatorID = Entity.ModifiedByCooperatorID;
                        Entity.NameVerifiedDate = System.DateTime.Now;
                    }
                    else
                    {
                        Entity.VerifiedByCooperatorID = 0;
                        Entity.NameVerifiedDate = DateTime.MinValue;
                    }

                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }

            //if ((EventAction == "DEMT") && (DemoteCreateFolder == true))
            //{
            //    FolderViewModel folderVm = new FolderViewModel();
            //    folderVm.Entity.TableName = TableName;
            //    folderVm.Entity.Title = "TO DO: Species Subtaxa Pending Re-Assignment";
            //    folderVm.Entity.Description = "** AUTO-GENERATED ** Contains taxa linked to " + Entity.SpeciesName + ".";
            //    folderVm.Entity.Category = "Priority";
            //    folderVm.Entity.IsFavorite = true;
            //    folderVm.Entity.ItemIDList = DemoteInfo;
            //    folderVm.Entity.CreatedByCooperatorID = Entity.ModifiedByCooperatorID;

            //    //TODO Get comma-sep list of species ID's to add.

            //    using (FolderManager folderMgr = new FolderManager())
            //    {
            //        folderMgr.Insert(folderVm.Entity);
            //    }
            //}
            return RowsAffected;
        }

        public JsonResult AddCitation(int citationId, string idList)
        {
            string[] idCollection;
            idCollection = idList.Split(',');

            using (SpeciesManager mgr = new SpeciesManager())
            {
                if (!String.IsNullOrEmpty(idList))
                {
                    foreach (var id in idCollection)
                    {
                        int convertedId = Int32.Parse(id);
                        mgr.AddCitation(citationId, convertedId);
                    }
                }
                else
                {
                    mgr.AddCitation(citationId, Entity.ID);
                }
            }

            //TODO
            return null;
        }

        public JsonResult AddTag(string tag, string idList)
        {
            string[] idCollection;
            idCollection = idList.Split(',');

            using (SpeciesManager mgr = new SpeciesManager())
            {
                if (!String.IsNullOrEmpty(idList))
                {
                    foreach (var id in idCollection)
                    {
                        int convertedId = Int32.Parse(id);
                        mgr.AddTag(tag, "taxonomy_species", convertedId);
                    }
                }
                else
                {
                }
            }
            return null;
        }

        public override bool Validate()
        {
            bool validated = true;
            
            switch (Entity.Rank.ToUpper())
                {
                    case "SPECIES":
                        if (String.IsNullOrEmpty(Entity.SpeciesName))
                        {
                            ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The species epithet is required." });
                        }
                        break;
                    case "SUBSPECIES":
                        if (String.IsNullOrEmpty(Entity.SubspeciesName))
                        {
                            ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The subspecies epithet is required." });
                        }
                        break;
                    case "VARIETY":
                        if (String.IsNullOrEmpty(Entity.VarietyName))
                        {
                            ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The variety epithet is required." });
                        }
                        break;
                    case "SUBVARIETY":
                        if (String.IsNullOrEmpty(Entity.SubvarietyName))
                        {
                            ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The subvariety epithet is required." });
                        }
                        break;
                    case "FORMA":
                        if (String.IsNullOrEmpty(Entity.FormaName))
                        {
                            ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The forma epithet is required." });
                        }
                        if (String.IsNullOrEmpty(Entity.FormaRankType))
                        {
                            ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The forma rank type is required." });
                        }
                        break;
                }

            if (Entity.GenusID == 0)
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The genus is required." });
            }

            //If user has designated an accepted name, require a synonym code -- and vice-versa.
            if (Entity.IsAcceptedName == "N")
            {
                if (Entity.AcceptedID == 0)
                {
                    ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "You must select an accepted species." });
                }

                //if (String.IsNullOrEmpty(Entity.SynonymCode))
                //{
                //    ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "You must select a synonym code." });
                //}
            }

            if (ValidationMessages.Count > 0)
            {
                validated = false;
            }
            return validated;
        }

        public bool ValdiateAuthority(string authorityText)
        {
            bool result = true;
            int errorCount = 0;
            string[] authList;

            authorityText = Regex.Replace(authorityText, @"\(|\)|,|\?| ex | non |sensu | et al\.", "&");
            if (authorityText.Contains("&"))
            {
                authList = authorityText.Split('&');
            }
            else
            {
                authList = authorityText.Split(' ');
            }

            foreach (string auth in authList)
            {
                string authClean = auth.Trim();
                if (!String.IsNullOrEmpty(authClean))
                {
                    using (SpeciesManager mgr = new SpeciesManager())
                    {
                        try
                        {
                            result = mgr.ValidateAuthority(authClean);
                            if (!result)
                            {
                                errorCount++; 
                            }
                        }
                        catch (Exception ex)
                        {
                            PublishException(ex);
                        }
                    }
                }
            }

            if (errorCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string GetTagCSSClassName(string tag) 
        {
            switch (tag)
            {
                case "HOMOTYPIC":
                    return "bg-green";
                case "HETEROTYPIC":
                    return "bg-yellow";
                case "INVALID":
                    return "bg-red";
                case "BASIONYM":
                    return "bg-light-blue";
                case "AUTONYM":
                    return "bg-aqua";
                default:
                    return "bg-gray";
            }
        }

        public void SetSpeciesName()
        {
            Entity.Name = Entity.GenusName + " " + Entity.SpeciesName;

            if (Entity.IsSpecificHybrid == "Y")
            {
                Entity.Name = "x " + Entity.SpeciesName;
            }
            if (Entity.IsSubspecificHybrid == "Y")
            {
                Entity.Name = "x " + Entity.SubspeciesName;
            }
            if (Entity.IsVarietalHybrid == "Y")
            {
                Entity.Name = "x " + Entity.VarietyName;
            }
            if (Entity.IsSubVarietalHybrid == "Y")
            {
                Entity.Name = "x " + Entity.SubvarietyName;
            }
            if (Entity.IsFormaHybrid == "Y")
            {
                Entity.Name = "x " + Entity.FormaName;
            }
        }

        public void SetSpeciesNameAuthority()
        {
            switch (Entity.Rank.ToUpper())
            {
                case "SUBSPECIES":
                    Entity.NameAuthority = Entity.SubspeciesAuthority;
                    break;
                case "VARIETY":
                    Entity.NameAuthority = Entity.VarietyAuthority;
                    break;
                case "SUBVARIETY":
                    Entity.NameAuthority = Entity.FormaAuthority;
                    break;
                case "FORMA":
                    Entity.NameAuthority = Entity.FormaAuthority;
                    break;
            }
        }

        public int CompareNames(string s, string t)
        {

            int n = s.Length; //length of s

            int m = t.Length; //length of t

            int[,] d = new int[n + 1, m + 1]; // matrix

            int cost; // cost

            // Step 1

            if (n == 0) return m;

            if (m == 0) return n;

            // Step 2

            for (int i = 0; i <= n; d[i, 0] = i++) ;

            for (int j = 0; j <= m; d[0, j] = j++) ;

            // Step 3

            for (int i = 1; i <= n; i++)
            {

                //Step 4

                for (int j = 1; j <= m; j++)
                {

                    // Step 5

                    cost = (t.Substring(j - 1, 1) == s.Substring(i - 1, 1) ? 0 : 1);

                    // Step 6

                    d[i, j] = System.Math.Min(System.Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                              d[i - 1, j - 1] + cost);

                }

            }


            // Step 7


            return d[n, m];

        }

    }
}
