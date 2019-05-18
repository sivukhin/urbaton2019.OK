using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CleanCityCore;
using CleanCityCore.EmailSender;
using CleanCityCore.Model;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace CleanCityBot
{
    public class BotUserInteraction
    {
        private readonly List<Attachment> attachments;
        private readonly IBotManager manager;
        private readonly ICleanCityApi cleanCityApi;
        private bool isStarted;

        public BotUserInteraction(IBotManager manager, ICleanCityApi cleanCityApi)
        {
            this.manager = manager;
            this.cleanCityApi = cleanCityApi;
            attachments = new List<Attachment>();
            isStarted = false;
        }

        public void RunIfNeeded()
        {
            if (!isStarted)
            {
                isStarted = true;
                Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        await Process();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    finally
                    {
                        isStarted = false;
                    }
                });
            }
        }

        private async Task Process()
        {
            Console.WriteLine("Run process");
            await GetResponse();
            Console.WriteLine("Get initial response");
            var markup = new ReplyKeyboardMarkup(new[]
            {
                new[] {new KeyboardButton("Грязь")},
                new[] {new KeyboardButton("Парковка")},
                new[] {new KeyboardButton("Ещё что-то")}
            });
            var makeReport = new ReplyKeyboardMarkup(new[]
            {
                new[] {new KeyboardButton("Сформировать обращение")},
            });
            var resetMarkup = new ReplyKeyboardRemove();

            var subject = string.Empty;
            while (string.IsNullOrWhiteSpace(subject))
            {
                await manager.SendTextMessageAsync("Введите тему обращения:", markup);
                subject = (await manager.GetResponseAsync()).Text;
            }

            var reportText = string.Empty;
            while (string.IsNullOrWhiteSpace(reportText))
            {
                await manager.SendTextMessageAsync("Подробно опишите детали проблемы:", resetMarkup);
                reportText = (await manager.GetResponseAsync()).Text;
            }

            GeoLocation location = null;
            while (location == null)
            {
                await manager.SendTextMessageAsync(
                    "Укажите своё местоположение",
                    new ReplyKeyboardMarkup(KeyboardButton.WithRequestLocation("Укажите своё местоположение")));
                var locationMessage = await manager.GetResponseAsync();
                if (locationMessage.Location != null)
                {
                    location = new GeoLocation
                    {
                        Latitude = locationMessage.Location.Latitude,
                        Longitude = locationMessage.Location.Longitude,
                    };
                }
            }

            while (true)
            {
                await manager.SendTextMessageAsync(
                    "Сфотографируйте проблему, после чего нажмите кнопку \"Сформировать обращение\"",
                    makeReport);
                var message = await manager.GetResponseAsync();
                if (message.Text != null && message.Text.Contains("обращение"))
                {
                    break;
                }
            }

            var report = new InitialReport
            {
                Subject = subject,
                ReportText = reportText,
                Location = location,
                Attachments = attachments.ToArray(),
            };
            cleanCityApi.SendReport(report);
        }

        private async Task<Message> GetResponse()
        {
            var message = await manager.GetResponseAsync();
            TryAddPhoto(message);
            return message;
        }

        private void TryAddPhoto(Message message)
        {
            if (message.Photo == null || !message.Photo.Any())
            {
                return;
            }

            var highResolutionPhoto = message.Photo.OrderByDescending(x => x.Width * x.Height).First();
            Console.WriteLine($"Download file: {highResolutionPhoto.FileId}");
            var photo = DownloadFile(highResolutionPhoto.FileId);
            attachments.Add(new Attachment
            {
                Data = photo,
                Filename = "attachment.jpg",
            });
        }

        private byte[] DownloadFile(string fileId)
        {
            var photoData = new MemoryStream();
            manager.Bot.GetInfoAndDownloadFileAsync(fileId, photoData).GetAwaiter().GetResult();
            return photoData.ToArray();
        }
    }
}