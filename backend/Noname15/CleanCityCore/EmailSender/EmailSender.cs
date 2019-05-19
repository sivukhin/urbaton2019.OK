using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace CleanCityCore.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderRequisites emailSenderRequisites;

        public EmailSender(EmailSenderRequisites emailSenderRequisites)
        {
            this.emailSenderRequisites = emailSenderRequisites;
        }

        public void SendEmail(EmailMessage message)
        {
            var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Host = emailSenderRequisites.SmtpHost,
                Port = emailSenderRequisites.SmtpPort,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    emailSenderRequisites.ServerLogin,
                    emailSenderRequisites.ServerPassword
                ),
            };

            // todo(sivukhin, 18.05.2019): pass message.RecipientEmail param instead of myEmail when all project will be ready 
            var email = new MailMessage(emailSenderRequisites.ServerEmail, message.RecipientEmail);
            email.Subject = $"{message.Subject}";
            email.Body = message.Body;
            email.IsBodyHtml = true;
            Console.WriteLine($"Found message with {message.Attachments?.Length} attachments");
            foreach (var attachment in message.Attachments ?? new Attachment[0])
            {
                email.Attachments.Add(new System.Net.Mail.Attachment(
                    new MemoryStream(attachment.Data),
                    attachment.Filename)
                );
            }

            client.Send(email);
        }
    }
}