using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Configuration;

namespace ConsoleAppSearchADTest
{
    class Email
    {
        public void SendEmail(string mailTo, string subject, string emailBody, string emailServerName = "", bool isBodyHTML = true,
            string mailToCC = "", string mailToBCC = "", string attachmentFilePath = "", string mailFrom = "doNotReply@utoledo.edu", string mailFromDisplayName = "Do Not Reply")
        {
            if(emailServerName == "")
            {
                throw new Exception("Email server was not specified.");
            }
            else
            {
                MailMessage mm = new MailMessage();
                mm.To.Add(mailTo);
                MailAddress maFrom = new MailAddress(mailFrom, mailFromDisplayName);
                mm.From = maFrom;
                if(mailToCC != "")
                {
                    mm.CC.Add(mailToCC);
                }
                
                if(mailToBCC != "")
                {
                    mm.Bcc.Add(mailToBCC);
                }
                
                mm.Subject = subject;
                mm.IsBodyHtml = isBodyHTML;
                mm.Body = emailBody;
                if(attachmentFilePath != "")
                {
                    Attachment attachment = new Attachment(attachmentFilePath, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilePath);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(attachmentFilePath);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(attachmentFilePath);
                    // Add the file attachment to this email message.
                    mm.Attachments.Add(attachment);
                }

                SmtpClient smtp = new SmtpClient(emailServerName);

                try
                {
                    //smtp.Send(mm);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in SendEmail(): {0}",
                                ex.ToString());
                }

            }


        }
    }
}
