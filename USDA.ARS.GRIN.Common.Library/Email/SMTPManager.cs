using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace USDA.ARS.GRIN.Common.Library.Email
{
    public class SMTPManager
    {
        protected static string SMTP_SERVER = "mailproxy1.usda.gov";

        public bool SendMessage(SMTPMailMessage sMTPMailMessage)
        {
            string[] recipientList;
            MailMessage mailMessage = new MailMessage();

            try
            {
                mailMessage.From = new MailAddress("noreply@usda.gov");
                mailMessage.ReplyToList.Add(new MailAddress("gringlobal-orders@usda.gov"));
                recipientList = sMTPMailMessage.To.Split(';');
                foreach (var recipient in recipientList)
                {
                    if (!String.IsNullOrEmpty(recipient))
                    {
                        mailMessage.To.Add(recipient);
                    }
                }
                mailMessage.Subject = sMTPMailMessage.Subject;
                mailMessage.Body = sMTPMailMessage.Body;
                mailMessage.Bcc.Add("c.benjamin.haag@outlook.com");
                mailMessage.IsBodyHtml = true;
                SmtpClient client = new SmtpClient(SMTP_SERVER);
                client.Send(mailMessage);
            }
            catch (SmtpException smex)
            {
                throw smex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}
