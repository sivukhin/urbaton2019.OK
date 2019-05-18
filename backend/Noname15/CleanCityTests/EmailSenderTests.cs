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
            var sender = new EmailSender(new EmailSenderRequisites
            {
                ServerLogin = "cleancity96@yandex.ru",
                ServerEmail = "cleancity96@yandex.ru",
                ServerPassword = "password",
                SmtpHost = "smtp.yandex.ru",
                SmtpPort = 25,
            });
            sender.SendEmail(new EmailMessage
            {
                RecipientEmail = "sivukhin.nikita@yandex.ru",
                Subject = "Test subject",
                Body = "Test message",
            });
        }
    }
}