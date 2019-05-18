using System;
using System.Threading;
using CleanCityCore;
using CleanCityCore.Infra;
using CleanCityCore.MessageExtending;
using Microsoft.EntityFrameworkCore.Internal;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace CleanCityBot
{
    class CleanCityBotEntryPoint
    {
        private static SecretManager secretManager = new SecretManager();

        static TelegramBotClient GetClient(bool useProxy)
        {
            if (useProxy)
            {
                var proxy = new HttpToSocks5Proxy(
                    "104.131.65.74",
                    1080,
                    "tg",
                    secretManager.GetSecret("proxy_pass")
                );
                proxy.ResolveHostnamesLocally = true;
                return new TelegramBotClient(secretManager.GetSecret("token"), proxy);
            }

            return new TelegramBotClient(secretManager.GetSecret("token"));
        }

        static void Main(string[] args)
        {
            var responsibleRepository = new ResponsibleRepository();
            var bot = GetClient(secretManager.GetSecret("use_proxy") == "true");
            // todo(sivukhin, 19.05.2019): Add dependency injection container
            var cleanCityApi = new CleanCityApi(
                new EmailRepository(),
                new ResponsibleFounder(responsibleRepository),
                new ReportRepository(),
                responsibleRepository,
                new UserRepository(),
                new MessageExtender());
            var client = new CleanCityTelegramBot(bot, cleanCityApi);
            client.Start();
            Thread.CurrentThread.Join();
        }
    }
}