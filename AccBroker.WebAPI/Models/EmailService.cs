using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Configuration;
using System;
using System.Net;

namespace AccBroker.WebAPI.Models
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Credentials:
            string smtpServer = ConfigurationManager.AppSettings["EmailSmtpServer"];
            int smtpPort = int.Parse(ConfigurationManager.AppSettings["EmailSmtpPort"]);
            bool enableSsl = bool.Parse(ConfigurationManager.AppSettings["EmailEnableSSL"]);
            string smtpUsername = ConfigurationManager.AppSettings["EmailSmtpUsername"];
            string smtpPassword = ConfigurationManager.AppSettings["EmailSmtpPassword"];
            string sentFrom = ConfigurationManager.AppSettings["EmailSentFrom"];

            // Configure the client:
            var client = new SmtpClient(smtpServer, Convert.ToInt32(587));

            client.Port = smtpPort;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = enableSsl;

            // Create the credentials:
            var credentials = new NetworkCredential(smtpUsername, smtpPassword);
            client.Credentials = credentials;

            // Create the message:
            var mail = new System.Net.Mail.MailMessage(sentFrom, message.Destination);

            mail.Subject = message.Subject;
            mail.Body = message.Body;

            // Send:
            await client.SendMailAsync(mail);
        }
    }
}