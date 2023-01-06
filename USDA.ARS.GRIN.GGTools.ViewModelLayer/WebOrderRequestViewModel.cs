using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class WebOrderRequestViewModel : WebOrderRequestViewModelBase, IViewModel<WebOrderRequest>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public WebOrderRequest Get(int entityId)
        {
            try { 
                using (WebOrderRequestManager mgr = new WebOrderRequestManager())
                {
                    Entity = mgr.Get(entityId);
                    DataCollectionItems =  new Collection<WebOrderRequestItem>(mgr.GetWebOrderRequestItems(entityId));

                    CodeValue codeValue = mgr.GetWebOrderRequestEmailAddresses(entityId);
                    Entity.EmailAddressList = codeValue.Title;

                    RowsAffected = mgr.RowsAffected; 
                    DataCollection.Add(Entity);
                }
                using (EmailTemplateManager emailTemplateMgr = new EmailTemplateManager())
                {
                    DataCollectionEmailTemplates = new Collection<EmailTemplate>(emailTemplateMgr.Search(new EmailTemplateSearch()));
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw (ex);
            }

            return Entity;
        }

        public void GetWebOrderRequestActions()
        {
            try 
            {
                using (WebOrderRequestManager mgr = new WebOrderRequestManager())
                {
                    DataCollectionActions = new Collection<WebOrderRequestAction>(mgr.GetWebOrderRequestActions(SearchEntity.ID));

                    // Get web order request actions, grouped by day
                    var queryWebOrderRequestActionDates =
                    from action in DataCollectionActions
                    group action by action.ActionDate into webOrderRequestActionGroup
                    orderby webOrderRequestActionGroup.Key descending
                    select webOrderRequestActionGroup;

                    foreach (var group in queryWebOrderRequestActionDates)
                    {
                        WebOrderRequestActionGroup webOrderRequestActionGroup = new WebOrderRequestActionGroup { DateGroup = DateTime.Parse(group.Key.ToString()) };
                        foreach (var subGroup in group)
                        {
                            WebOrderRequestAction webOrderRequestAction = new WebOrderRequestAction();
                            webOrderRequestAction.ID = subGroup.ID;
                            webOrderRequestAction.ActionCode = subGroup.ActionCode;
                            webOrderRequestAction.ActionTitle = subGroup.ActionTitle;
                            webOrderRequestAction.ActionDescription = subGroup.ActionDescription;
                            webOrderRequestAction.Note = subGroup.Note;
                            webOrderRequestAction.CreatedDate = subGroup.CreatedDate;
                            webOrderRequestAction.CreatedByCooperatorID = subGroup.CreatedByCooperatorID;
                            webOrderRequestAction.CreatedByCooperatorName = subGroup.CreatedByCooperatorName;
                            webOrderRequestActionGroup.WebOrderRequestActions.Add(webOrderRequestAction);
                        }
                        DataCollectionActionGroups.Add(webOrderRequestActionGroup);
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        public void GetOrderRequestActions()
        {
            try
            {
                using (WebOrderRequestManager mgr = new WebOrderRequestManager())
                {
                    DataCollectionActions = new Collection<WebOrderRequestAction>(mgr.GetWebOrderRequestActions(SearchEntity.ID));

                    // Get web order request actions, grouped by day
                    var queryWebOrderRequestActionDates =
                    from action in DataCollectionActions
                    group action by action.ActionDate into webOrderRequestActionGroup
                    orderby webOrderRequestActionGroup.Key descending
                    select webOrderRequestActionGroup;

                    foreach (var group in queryWebOrderRequestActionDates)
                    {
                        WebOrderRequestActionGroup webOrderRequestActionGroup = new WebOrderRequestActionGroup { DateGroup = DateTime.Parse(group.Key.ToString()) };
                        foreach (var subGroup in group)
                        {
                            WebOrderRequestAction webOrderRequestAction = new WebOrderRequestAction();
                            webOrderRequestAction.ID = subGroup.ID;
                            webOrderRequestAction.ActionCode = subGroup.ActionCode;
                            webOrderRequestAction.ActionTitle = subGroup.ActionTitle;
                            webOrderRequestAction.ActionDescription = subGroup.ActionDescription;
                            webOrderRequestAction.Note = subGroup.Note;
                            webOrderRequestAction.CreatedDate = subGroup.CreatedDate;
                            webOrderRequestAction.CreatedByCooperatorID = subGroup.CreatedByCooperatorID;
                            webOrderRequestAction.CreatedByCooperatorName = subGroup.CreatedByCooperatorName;
                            webOrderRequestActionGroup.WebOrderRequestActions.Add(webOrderRequestAction);
                        }
                        DataCollectionActionGroups.Add(webOrderRequestActionGroup);
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        public EmailTemplate GetEmailTemplate(string typeCode)
        {
            EmailTemplate emailTemplate = new EmailTemplate();
            try
            {
                using (EmailTemplateManager mgr = new EmailTemplateManager())
                {
                    EmailTemplateSearch emailTemplateSearch = new EmailTemplateSearch();
                    emailTemplateSearch.CategoryCode = typeCode;
                    DataCollectionEmailTemplates = new Collection<EmailTemplate>(mgr.Search(emailTemplateSearch));
                    if (RowsAffected == 1)
                    {
                        emailTemplate = DataCollectionEmailTemplates[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return emailTemplate;
        }
        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            throw new NotImplementedException();
        }

        public int InsertNote(int webOrderRequestId, string actionNote, int webUserId)
        {
            int rowsAffected = 0;

            WebOrderRequestAction webOrderRequestAction = new WebOrderRequestAction();
            webOrderRequestAction.WebOrderRequestID = webOrderRequestId;
            webOrderRequestAction.ActionCode = "NRR_NOTE";
            webOrderRequestAction.CreatedByWebUserID = webUserId;
            webOrderRequestAction.Note = actionNote;

            using (WebOrderRequestManager mgr = new WebOrderRequestManager())
            {
                rowsAffected = mgr.InsertWebOrderRequestAction(webOrderRequestAction);
            }
            return rowsAffected;
        }
        public void InsertAction(WebOrderRequestAction webOrderRequestAction)
        {
            int rowsAffected = 0;

            using (WebOrderRequestManager mgr = new WebOrderRequestManager())
            {
                rowsAffected = mgr.InsertWebOrderRequestAction(webOrderRequestAction);
            }
        }
        public void Search()
        {
            using (WebOrderRequestManager mgr = new WebOrderRequestManager())
            {
                try
                {
                    DataCollection = new Collection<WebOrderRequest>(mgr.Search(SearchEntity));
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

        public int Update()
        {
            bool isLoggable = true;
            SMTPManager sMTPManager = new SMTPManager();
            SMTPMailMessage sMTPMailMessage = new SMTPMailMessage();

            // If WOR had been previously reviewed, suppress email notification.
            if (Entity.IsPreviouslyNRRReviewed == "Y")
            {
                SendInternalNotification = false;
                SendRequestorNotification = false;
            }

            using (EmailTemplateManager emailTemplateMgr = new EmailTemplateManager())
            {
                DataCollectionEmailTemplates = new Collection<EmailTemplate>(emailTemplateMgr.Search(new EmailTemplateSearch()));
            }

            using (WebOrderRequestManager mgr = new WebOrderRequestManager())
            {
                try
                {
                    switch (Entity.StatusCode)
                    {
                        case "NRR_REVIEW":
                            mgr.Update(Entity);
                            isLoggable = false;
                            break;
                        case "NRR_REVIEW_END":
                            mgr.Update(Entity);
                            isLoggable = false;
                            break;
                        case "NRR_REOPEN":
                            mgr.Update(Entity);
                            isLoggable = true;
                            break;
                        case "NRR_FLAG_COOP":
                            //TODO: Refactor --CBH, 2/10/22
                            WebCooperator webCooperator = new WebCooperator();
                            webCooperator.ID = Entity.WebCooperatorID;
                            webCooperator.VettedStatusCode = "FLAGGED";
                            webCooperator.ModifiedByWebUserID = Entity.OwnedByWebUserID;
                            webCooperator.Note = Entity.Note;
                            break;
                        case "NRR_VET_COOP":
                            //TODO: Refactor --CBH, 2/10/22
                            WebCooperator webCooperator2 = new WebCooperator();
                            webCooperator2.ID = Entity.WebCooperatorID;
                            webCooperator2.VettedStatusCode = "VETTED";
                            webCooperator2.ModifiedByWebUserID = Entity.OwnedByWebUserID;
                            webCooperator2.Note = Entity.Note;
                            break;
                        case "NRR_NOTE":
                            break;
                        case "NRR_INFO":
                            SMTPMailMessage infoRequestEmailMessage = new SMTPMailMessage();
                            infoRequestEmailMessage.From = ActionEmailFrom;
                            infoRequestEmailMessage.To = ActionEmailTo;
                            infoRequestEmailMessage.Subject = ActionEmailSubject;
                            infoRequestEmailMessage.Body = ActionEmailBody;
                            infoRequestEmailMessage.IsHtml = true;
                            sMTPManager.SendMessage(infoRequestEmailMessage);
                            break;
                        case "NRR_CUR":
                            SMTPMailMessage curatorEmailMessage = new SMTPMailMessage();
                            curatorEmailMessage.From = ActionEmailFrom;
                            curatorEmailMessage.To = ActionEmailTo;
                            curatorEmailMessage.Subject = ActionEmailSubject;
                            curatorEmailMessage.Body = ActionEmailBody;
                            curatorEmailMessage.IsHtml = true;
                            sMTPManager.SendMessage(curatorEmailMessage);
                            break;
                        case "NRR_APPROVE":
                            mgr.Update(Entity);
                            if (SendInternalNotification)
                            {
                                SMTPMailMessage internalEmailMessage = new SMTPMailMessage();
                                EmailTemplate rejectionEmailTemplate = DataCollectionEmailTemplates.Where(x => x.CategoryCode == "CAP").First();
                                internalEmailMessage.From = rejectionEmailTemplate.EmailFrom;
                                internalEmailMessage.To = Entity.EmailAddressList;
                                internalEmailMessage.Subject = rejectionEmailTemplate.Subject.Replace("[ID_HERE]", Entity.ID.ToString());
                                internalEmailMessage.Body = rejectionEmailTemplate.Body.Replace("[ID_HERE]", Entity.ID.ToString());
                                internalEmailMessage.IsHtml = true;
                                sMTPManager.SendMessage(internalEmailMessage);
                            }
                            break;
                        case "NRR_REJECT":
                            mgr.Update(Entity);
                            if (SendInternalNotification)
                            {
                                SMTPMailMessage internalEmailMessage = new SMTPMailMessage();
                                EmailTemplate rejectionEmailTemplate = DataCollectionEmailTemplates.Where(x => x.CategoryCode == "CCL").First();
                                internalEmailMessage.From = rejectionEmailTemplate.EmailFrom;
                                internalEmailMessage.To = Entity.EmailAddressList;
                                internalEmailMessage.Subject = rejectionEmailTemplate.Subject.Replace("[ID_HERE]", Entity.ID.ToString());
                                internalEmailMessage.Body = rejectionEmailTemplate.Body.Replace("[ID_HERE]", Entity.ID.ToString());
                                internalEmailMessage.IsHtml = true;
                                sMTPManager.SendMessage(internalEmailMessage);
                            }

                            if (SendRequestorNotification)
                            {
                                SMTPMailMessage requestorEmailMessage = new SMTPMailMessage();
                                requestorEmailMessage.From = ActionEmailFrom;
                                requestorEmailMessage.To = ActionEmailTo;
                                requestorEmailMessage.Subject = ActionEmailSubject;
                                requestorEmailMessage.Body = ActionEmailBody;
                                requestorEmailMessage.IsHtml = true;
                                sMTPManager.SendMessage(requestorEmailMessage);
                            }
                            break;
                    }

                    // Populate the action that will be logged when specified.
                    if (isLoggable)
                    {
                        WebOrderRequestAction webOrderRequestAction = new WebOrderRequestAction();
                        webOrderRequestAction.WebOrderRequestID = Entity.ID;
                        webOrderRequestAction.ActionCode = Entity.StatusCode;
                        webOrderRequestAction.Note = Entity.Note;
                        webOrderRequestAction.OwnedByWebUserID = Entity.OwnedByWebUserID;
                        mgr.InsertWebOrderRequestAction(webOrderRequestAction);
                    }

                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
            return RowsAffected;
        }

        public string GetCSSClass(string statusCode)
        {
            string cssClass = String.Empty;

            if (statusCode.Contains("NRR_FLAG"))
            {
                cssClass = "far fa-flag bg-red";
            }
            else
            {
                switch (statusCode)
                {
                    case "NRR_FLAGGED":
                        cssClass = "far fa-flag bg-yellow";
                        break;
                    case "NRR_NOTE":
                        cssClass = "far fa-comment bg-purple";
                        break;
                    case "NRR_REVIEW":
                        cssClass = "far fa-eye bg-blue";
                        break;
                    case "NRR_INFO":
                        cssClass = "far fa-envelope margin-r-5 bg-maroon";
                        break;
                    case "NRR_REJECT":
                        cssClass = "far fa-thumbs-down bg-red";
                        break;
                    case "NRR_APPROVE":
                        cssClass = "far fa-thumbs-up bg-green";
                        break;
                    default:
                        cssClass = "far fa-clock bg-aqua";
                        break;
                }
            }
            return cssClass;
        }
    }
}
