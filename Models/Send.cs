using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Models
{
    public class Send
    {
        public string SendMail(string ToEmail, string Token)
        {
            string FromEmail = "poojadonawadi7@gmail.com";
            MailMessage message = new MailMessage(FromEmail, ToEmail);
            string MailBody = "The token for the reset password: " + Token;
            message.Subject = "Token generated for resetiing password";
            message.Body = MailBody.ToString();
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            NetworkCredential credential = new NetworkCredential("poojadonawadi7@gmail.com", "imsk hnuc gnhj qdru");

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = credential;

            smtpClient.Send(message);
            return ToEmail;
        }
    }
}
