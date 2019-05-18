using System;
using System.Threading;
using CleanCityCore;
using CleanCityCore.EmailSender;

namespace EmailSenderDaemon
{
    class EmailSenderDaemonEntryPoint
    {
        static void Main(string[] args)
        {
            var emailSenderRequisites = new EmailSenderRequisites
            {
                ServerLogin = "cleancity96@yandex.ru",
                ServerEmail = "cleancity96@yandex.ru",
                ServerPassword = "password",
                SmtpHost = "smtp.yandex.ru",
                SmtpPort = 25,
            };
            var emailRepository = new EmailRepository();
            var emailSender = new EmailSender(emailSenderRequisites);
            while (true)
            {
                var unsentEmails = emailRepository.ReadUnsentEmails().Shuffle();
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