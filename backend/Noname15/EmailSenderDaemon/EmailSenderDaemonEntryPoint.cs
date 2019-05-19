using System;
using System.IO;
using System.Linq;
using System.Threading;
using CleanCityCore;
using CleanCityCore.EmailSender;
using CleanCityCore.Infra;

namespace EmailSenderDaemon
{
    class EmailSenderDaemonEntryPoint
    {
        static void Main(string[] args)
        {
            var secretManager = new SecretManager();
            var emailSenderRequisites = new EmailSenderRequisites
            {
                ServerLogin = "postmaster@sandbox04ced59a05e34ccd8a0ebd92846412d8.mailgun.org",
                ServerEmail = "cleancity96@yandex.ru",
                ServerPassword = secretManager.GetSecret("password"),
                SmtpHost = "smtp.mailgun.org",
                SmtpPort = 587,
            };
            var responsibleRepository = new ResponsibleRepository();
            var emailRepository = new EmailRepository();
            var emailSender = new EmailSender(emailSenderRequisites);
            Console.Out.WriteLine("Email sender started!");
            while (true)
            {
                var unsentEmails = emailRepository.ReadUnsentEmails().Shuffle();
                Console.Out.WriteLine($"Found {unsentEmails.Length} unsent emails");
                foreach (var emailId in unsentEmails)
                {
                    try
                    {
                        var email = emailRepository.ReadEmail(emailId);
                        var doublers = responsibleRepository.GetDoublers(email.ResponsibleId);
                        var myEmail = "sivukhin.nikita@yandex.ru";
                        // todo(sivukhin, 19.05.2019): Use email.RecipientEmail instead of myEmail
                        foreach (var targetEmail in new[] {myEmail}.Concat(doublers.Select(x => x.Email)))
                        {
                            Console.Out.WriteLine($"Send message to recipient: {targetEmail}");
                            email.RecipientEmail = targetEmail;
                            emailSender.SendEmail(email);
                            emailRepository.SetEmailProcessed(emailId);
                        }

                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e);
                    }
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}