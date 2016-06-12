using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
namespace OpenData.Message
{

    public class SMTPService : ISMTPService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public void SendMail(string from, string to, string subject, string body)
        {
            SendMail(subject, body, from, to, string.Empty, string.Empty, string.Empty, true, Encoding.UTF8);
        }

        public string Host { get; set; }

        public int Port { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }


        /// <summary>
        /// Send mails Full Parameters function
        /// </summary>
        public bool SendMail(string subject, string body, string from, string to, string cc, string bcc, string attachedFile, bool htmlFormat, Encoding encodingType)
        {
            try
            {    //Encoding Verification											
                if (encodingType == null)
                {
                    encodingType = Encoding.UTF8;
                }
                MailMessage objMail = new MailMessage();
                if (!string.IsNullOrEmpty(subject))
                {
                    objMail.Subject = subject;
                }
                else
                {
                    objMail.Subject = "Null";
                }


                if (string.IsNullOrEmpty(from))
                {
                    objMail.From = new MailAddress(UserName);
                }
                else
                {
                    objMail.From = new MailAddress(UserName, from);
                }

                char[] separator = { ';', '；' };
                if (to != string.Empty)
                {
                    string[] emails = to.Split(separator);
                    foreach (string email in emails)
                    {
                        objMail.To.Add(email);
                    }
                }

                if (cc != string.Empty)
                {
                    string[] emails = cc.Split(separator);
                    foreach (string email in emails)
                    {
                        objMail.CC.Add(email);
                    }
                }
                if (bcc != string.Empty)
                {
                    string[] emails = bcc.Split(separator);
                    foreach (string email in emails)
                    {
                        objMail.Bcc.Add(email);
                    }
                }
                if (body != string.Empty)
                {
                    objMail.Body = body;
                }
                //File Attachment Initialisation								
                if (attachedFile != string.Empty && attachedFile != null)
                {
                    objMail.Attachments.Add(new Attachment(attachedFile));
                }

                objMail.IsBodyHtml =! htmlFormat;

                //Encoding Initialisation										
                objMail.BodyEncoding = encodingType;
                //Priority Initiaisation										
                objMail.Priority = MailPriority.Normal;
                //Send Mail	
                SmtpClient smtp = new SmtpClient(this.Host,this.Port);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new NetworkCredential(this.UserName, this.Password);
                smtp.Send(objMail);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("SendMail", ex);
                return false;
            }
        }
    }
}