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
        static void Main(string[] args)
        {
            var secretManager = new SecretManager();
            var proxy = new HttpToSocks5Proxy(
                "104.131.65.74",
                1080,
                "tg",
                secretManager.GetSecret("proxy_pass")
            );
            proxy.ResolveHostnamesLocally = true;
            var bot = new TelegramBotClient(secretManager.GetSecret("token"), proxy);
            var responsibleRepository = new ResponsibleRepository();
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