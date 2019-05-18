using System;
using System.IO;
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
                ServerLogin = "cleancity96@yandex.ru",
                ServerEmail = "cleancity96@yandex.ru",
                ServerPassword = secretManager.GetSecret("password"),
                SmtpHost = "smtp.yandex.ru",
                SmtpPort = 25,
            };
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
                        emailSender.SendEmail(email);
                        emailRepository.SetEmailProcessed(emailId);
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