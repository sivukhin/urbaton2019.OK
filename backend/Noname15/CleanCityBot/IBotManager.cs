using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CleanCityBot
{
    public interface IBotManager
    {
        Task<Message> GetResponseAsync();
        Task SendTextMessageAsync(string text, IReplyMarkup replyMarkup = null);
        ITelegramBotClient Bot { get; }
        long UserId { get; }
    }
}