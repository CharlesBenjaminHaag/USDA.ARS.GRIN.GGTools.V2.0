using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class CooperatorViewModel : CooperatorViewModelBase, IViewModel<Cooperator>
    {
        public int AuthenticatedUserCooperatorSiteID { get; set; }
        
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Cooperator Get(int entityId, string environment = "")
        {
            using (CooperatorManager mgr = new CooperatorManager())
            {
                Entity = mgr.Get(entityId, environment);
            }
            return Entity;
        }

        public void GetCooperatorsBySysGroup(string sysGroupName)
        {
            using (CooperatorManager mgr = new CooperatorManager())
            {
                try
                {
                    DataCollection = new Collection<Cooperator>(mgr.GetCooperatorsBySysGroup(sysGroupName));
                    if (DataCollection.Count() == 1)
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

        public void GetRecordsOwned(int cooperatorId)
        {
            using (CooperatorManager mgr = new CooperatorManager())
            {
                DataCollectionReportItems = new Collection<ReportItem>(mgr.GetRecordsOwned(cooperatorId));
                TotalRecordsOwned = DataCollectionReportItems.ToList().Sum(x => x.ItemCount);
            }
        }
      
        public int Insert()
        {
            SysUserViewModel sysUserViewModel = new SysUserViewModel();

            using (CooperatorManager mgr = new CooperatorManager())
            {
                try
                {
                    Entity.ID = mgr.Insert(Entity);

                    if (Entity.ID > 0)
                    {
                        sysUserViewModel.Entity.UserName = Entity.FirstName + "." + Entity.LastName;
                        sysUserViewModel.Entity.CooperatorID = Entity.ID;
                        sysUserViewModel.Entity.CooperatorID = Entity.CreatedByCooperatorID;
                        sysUserViewModel.Entity.Password = sysUserViewModel.GetSecurePassword("TEST");
                        sysUserViewModel.Insert();
                    }

                    //sysUserViewModel.SendNotification("N");
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return Entity.ID;
            }
        }

        public void Search()
        {
            using (CooperatorManager mgr = new CooperatorManager())
            {
                try
                {
                    DataCollection = new Collection<Cooperator>(mgr.Search(SearchEntity));
                    if (DataCollection.Count() == 1)
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

        //public void SearchSiteCurators(int siteId)
        //{
        //    using (CooperatorManager mgr = new CooperatorManager())
        //    {
        //        try
        //        {
        //            DataCollection = new Collection<Cooperator>(mgr.SearchSiteCurators(siteId));
        //            if (DataCollection.Count() == 1)
        //            {
        //                Entity = DataCollection[0];
        //            }

        //            RowsAffected = mgr.RowsAffected;
        //        }
        //        catch (Exception ex)
        //        {
        //            PublishException(ex);
        //            throw ex;
        //        }
        //    }
        //}

        public List<Cooperator> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            using (CooperatorManager mgr = new CooperatorManager())
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
                return Entity.ID;
            }
        }

        public void SendNotification()
        {
            SMTPManager sMTPManager = new SMTPManager();
            SMTPMailMessage infoRequestEmailMessage = new SMTPMailMessage();
            infoRequestEmailMessage.From = "gringlobal-support@usda.gov";
            infoRequestEmailMessage.To = Entity.EmailAddress;
            infoRequestEmailMessage.Subject = "Your GRIN-Global CT Account Password Has Been Reset";

            infoRequestEmailMessage.Body = "Please store your new password securely. It will expire on " + SysUserEntity.SysUserPasswordExpirationDate.ToShortDateString() + ".";
            infoRequestEmailMessage.Body += "<br/>";

            infoRequestEmailMessage.Body += "<table>";
            infoRequestEmailMessage.Body += "<tr>";
            infoRequestEmailMessage.Body += "<td><strong>Username</strong></td><td>" + SysUserEntity.SysUserName + "</td>";
            infoRequestEmailMessage.Body += "</tr>";
            infoRequestEmailMessage.Body += "<tr>";
            infoRequestEmailMessage.Body += "<td><strong>New Password</strong></td><td>" + SysUserEntity.SysUserPlainTextPassword + "</td>";
            infoRequestEmailMessage.Body += "</tr>";
            infoRequestEmailMessage.Body += "</table>";

            infoRequestEmailMessage.IsHtml = true;
            sMTPManager.SendMessage(infoRequestEmailMessage);
        }

        public Cooperator Get(int entityId)
        {
            SearchEntity.ID = entityId;
            Search();
            return Entity;
        }

    }
}
