﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class EMAIL
    {

        public static Tuple<bool,string> SendEmail(string senderMAil, string toMAil, string body, string subject)
        {
             bool isSucess = true;
            string msg = "";
            Tuple<bool, string> tu = Tuple.Create(isSucess, msg);
            try
            {
                MailMessage mail = new MailMessage(senderMAil, toMAil);
                SmtpClient client = new SmtpClient();
                //client.Port = 587;
                //client.Host = "smtp.gmail.com";
                client.Host = "sg2nlvphout-v01.shr.prod.sin2.secureserver.net";
                client.Port = 25;
                client.EnableSsl = false;
                client.Timeout = 10000;
              ///  client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("theeru999@gmail.com", "9490033131");
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                client.Send(mail);
                tu = Tuple.Create(isSucess, "Mail Sent Successfully to this user.Plese check in INBOX (or) SPAM after ten minutes");

            }
            catch(Exception e)
            {
                isSucess = false;
                string m = "Error occured while sending mail due to " + e.Message;
                tu = Tuple.Create(isSucess, m);
                // HandleException(e, "HandleSMS");
                // throw e;
            }
            return tu;
        }
    }
}
