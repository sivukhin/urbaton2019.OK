using System.IO;
using System.Net;
using System.Net.Mail;

namespace CleanCityCore
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
            var email = new MailMessage(emailSenderRequisites.ServerEmail, message.RecipientEmail);
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
            email.Subject = message.Subject;
            email.Body = message.Body;
            foreach (var attachment in message.Attachments)
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