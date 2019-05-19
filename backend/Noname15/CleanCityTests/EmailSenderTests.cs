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
                ServerLogin = "cleancityekb@gmail.com",
                ServerEmail = "cleancityekb@gmail.com",
                ServerPassword = @".L\yW#{^0M#ExONc[p(sP=[#h",
                SmtpHost = "smtp.gmail.com",
                SmtpPort = 25,
            };
            var sender = new EmailSender(emailSenderRequisites);
            sender.SendEmail(new EmailMessage
            {
                RecipientEmail = "sivukhin.nikita@yandex.ru",
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