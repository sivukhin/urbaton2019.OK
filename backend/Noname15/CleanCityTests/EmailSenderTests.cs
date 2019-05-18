using System.Text;
using CleanCityCore;
using CleanCityCore.EmailSender;
using NUnit.Framework;

namespace CleanCityTests
{
    public class EmailSenderTests
    {
        [Test]
        public void TestSending()
        {
            var emailSenderRequisites = new EmailSenderRequisites
            {
                ServerLogin = "postmaster@sandbox04ced59a05e34ccd8a0ebd92846412d8.mailgun.org",
                ServerEmail = "cleancity96@yandex.ru",
                ServerPassword = "password",
                SmtpHost = "smtp.mailgun.org",
                SmtpPort = 587,
            };
            var sender = new EmailSender(emailSenderRequisites);
            sender.SendEmail(new EmailMessage
            {
                RecipientEmail = "nikitos7991@yandex.ru",
                Subject = "Test subject",
                Body = "Test message",
                Attachments = new[]
                {
                    new Attachment
                    {
                        Data = Encoding.UTF8.GetBytes("Hello from attachment"),
                        Filename = "simple.txt",
                    }
                }
            });
        }
    }
}