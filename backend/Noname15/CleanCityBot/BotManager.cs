using System;
using System.Threading.Tasks;
using Nito.AsyncEx;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CleanCityBot
{
    class BotManager : IBotManager
    {
        private readonly ITelegramBotClient bot;
        private readonly long chatId;
        private AsyncCollection<Message> messageQueue;

        public BotManager(ITelegramBotClient bot, long chatId)
        {
            this.bot = bot;
            this.chatId = chatId;
            messageQueue = new AsyncCollection<Message>();
        }

        public async Task<Message> GetResponseAsync()
        {
            var result = await messageQueue.TakeAsync();
            return result;
        }

        public async Task SendTextMessageAsync(string text, IReplyMarkup replyMarkup = null)
        {
            Console.WriteLine($"Send message to chat {chatId}: {text}");
            await bot.SendTextMessageAsync(chatId, text, replyMarkup: replyMarkup);
        }

        public void AddMessage(Message message)
        {
            messageQueue.Add(message);
        }

        public ITelegramBotClient Bot => bot;
        public long UserId => chatId;
    }
}