using System;
using System.Configuration;
using System.Net.Mail;

namespace Cooperative.Models
{
    public class Communication
    {
        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="To">To : Recipients</param>
        /// <param name="CC">CC : Recipients</param>
        /// <param name="BCC">BCC : Recipients</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Mail body</param>
        /// <param name="IsBodyHTML">Is mail body in html format</param>
        /// <param name="IsError">ref Is error occurred</param>
        /// <param name="ErrorMessage">If error occurred then error message</param>
        public static void SendMail(EmailEntity objEmailEntity)
        {
            try
            {
                // SMTP Credentials
                SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SMTPHost"], Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]));
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SMTPUserName"], ConfigurationManager.AppSettings["SMTPPassword"]);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;


                // Mail Settings
                MailMessage mail = new MailMessage();

                if (!string.IsNullOrEmpty(objEmailEntity.To))
                    foreach (string _To in objEmailEntity.To.Split(',')) // List of To Receipients
                        if (!string.IsNullOrEmpty(_To))
                            mail.To.Add(_To);

                if (!string.IsNullOrEmpty(objEmailEntity.CC))
                    foreach (string _CC in objEmailEntity.CC.Split(',')) // List of CC Receipients
                        if (!string.IsNullOrEmpty(_CC))
                            mail.CC.Add(_CC);

                if (!string.IsNullOrEmpty(objEmailEntity.Bcc))
                    foreach (string _BCC in objEmailEntity.Bcc.Split(',')) // List of BCC Receipients
                        if (!string.IsNullOrEmpty(_BCC))
                            mail.Bcc.Add(_BCC);

                mail.Subject = objEmailEntity.Subject; // Subject of mail
                mail.Body = objEmailEntity.Body; // Mail body
                mail.From = new MailAddress(ConfigurationManager.AppSettings["SMTPMailFrom"]); // mail from Sender
                mail.IsBodyHtml = objEmailEntity.IsBodyHtml;

                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                ILogger objFileLogger = new FileLogger();
                objFileLogger.ErrorHandle(new ErrorLogEntity() { FunctionName = "SendMail(EmailEntity objEmailEntity)", PageName = "Cooperative.Models.DBLogger.Communication" }, ex);
            }
        }

        /// <summary>
        /// Send SMS
        /// </summary>
        /// <param name="MobileNo">Mobile No. of Recipient</param>
        /// <param name="Message">Message</param>
        public static void SendSMS(string MobileNo, string Message)
        {
            try
            {
                //SMS logic here
            }
            catch (Exception ex)
            {
                ILogger objFileLogger = new FileLogger();
                objFileLogger.ErrorHandle(new ErrorLogEntity() { FunctionName = "SendSMS(string MobileNo, string Message)", PageName = "Cooperative.Modelss.DBLogger.Communication" }, ex);
            }
        }
    }
}