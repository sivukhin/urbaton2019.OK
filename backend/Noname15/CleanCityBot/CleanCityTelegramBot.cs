using System;
using System.Collections.Concurrent;
using System.Linq;
using CleanCityCore;
using CleanCityCore.EmailSender;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace CleanCityBot
{
    public class CleanCityTelegramBot
    {
        private readonly ITelegramBotClient bot;
        private readonly ICleanCityApi cleanCityApi;
        private readonly ConcurrentDictionary<long, BotUserInteraction> userInteractions;
        private readonly ConcurrentDictionary<long, BotManager> userManagers;

        public CleanCityTelegramBot(TelegramBotClient bot, ICleanCityApi cleanCityApi)
        {
            this.bot = bot;
            this.cleanCityApi = cleanCityApi;
            userInteractions = new ConcurrentDictionary<long, BotUserInteraction>();
            userManagers = new ConcurrentDictionary<long, BotManager>();
            bot.OnMessage += OnMessage;
        }

        public void Start()
        {
            bot.StartReceiving();
            Console.WriteLine("Bot is started!");
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            var chatId = e.Message.Chat.Id;
            var manager = userManagers.GetOrAdd(chatId, x => new BotManager(bot, x));
            var interaction = userInteractions.GetOrAdd(chatId, x => new BotUserInteraction(manager, cleanCityApi));
            interaction.RunIfNeeded();
            manager.AddMessage(e.Message);
        }
    }
}