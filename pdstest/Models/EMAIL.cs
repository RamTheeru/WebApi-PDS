using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class EMAIL
    {

        public static bool SendEmail(string senderMAil, string toMAil, string body, string subject)
        {
            bool isSucess = true;
            try
            {
                MailMessage mail = new MailMessage(senderMAil, toMAil);
                SmtpClient client = new SmtpClient();
                //client.Port = 587;
                //client.Host = "smtp.gmail.com";
                client.Host = "relay-hosting.secureserver.net";
                client.Port = 25;
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("theeru999@gmail.com", "9490033131");
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                client.Send(mail);
        
            }
            catch (Exception e)
            {
                isSucess = false;
                // HandleException(e, "HandleSMS");
               // throw e;
            }
            return isSucess;
        }
    }
}
