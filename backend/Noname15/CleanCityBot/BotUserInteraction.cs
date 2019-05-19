using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using CleanCityCore;
using CleanCityCore.EmailSender;
using CleanCityCore.Infra;
using CleanCityCore.Model;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = CleanCityCore.Model.User;

namespace CleanCityBot
{
    public class BotUserInteraction
    {
        private readonly IBotManager manager;
        private readonly ICleanCityApi cleanCityApi;
        private bool isStarted;

        public BotUserInteraction(IBotManager manager, ICleanCityApi cleanCityApi)
        {
            this.manager = manager;
            this.cleanCityApi = cleanCityApi;
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
                    catch (BotCancelOperationException e)
                    {
                        Console.WriteLine($"User {manager.UserId} cancel operation");
                    }
                    catch (Exception e)
                    {
                        await manager.SendTextMessageAsync(
                            "Что-то пошло не так. Мы уже разбираемся с проблемой, повторите свой запрос позже.",
                            resetMarkup);
                        Console.Error.WriteLine(e);
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
            var firstResponse = await GetResponseAsync();
            if (firstResponse.Text == "/start" || firstResponse.Text == "/help")
            {
                await ProcessHelp();
            }

            else if (firstResponse.Text == "/register")
            {
                await ProcessRegistration();
            }

            else if (firstResponse.Text == "/report")
            {
                await ProcessReport();
            }
        }

        private async Task ProcessHelp()
        {
            await manager.SendTextMessageAsync(UserHelp, new ReplyKeyboardRemove());
        }

        private async Task ProcessRegistration()
        {
            var currentUser = cleanCityApi.GetUser(manager.UserId);
            if (currentUser == null)
            {
                await ProcessNewUser();
            }
            else
            {
                await ProcessOldUser(currentUser);
            }
        }

        private async Task ProcessOldUser(User user)
        {
            while (true)
            {
                await manager.SendTextMessageAsync(
                    $"Ваши реквизиты:\n{PrintUserInformation(user)}\n" +
                    $"{ChangeRequisitesHelp}");
                var response = (await GetResponseAsync()).Text;
                if (response == "/done")
                {
                    cleanCityApi.AddOrUpdateUser(user);
                    await manager.SendTextMessageAsync("Данные успешно сохранены");
                    break;
                }
                else if (response == "/change_name")
                {
                    await manager.SendTextMessageAsync("Введите новое имя:");
                    user.Username = (await GetResponseAsync()).Text;
                }
                else if (response == "/change_email")
                {
                    await manager.SendTextMessageAsync("Введите новый email:");
                    user.Email = (await GetResponseAsync()).Text;
                }
                else if (response == "/change_address")
                {
                    await manager.SendTextMessageAsync("Введите новый адрес:");
                    user.Address = (await GetResponseAsync()).Text;
                }
                else
                {
                    await manager.SendTextMessageAsync("Неизвестная команда, попробуйте ещё раз");
                }
            }
        }

        private async Task ProcessNewUser()
        {
            var userName = string.Empty;
            while (string.IsNullOrWhiteSpace(userName))
            {
                await manager.SendTextMessageAsync("Введите ваши имя и фамилию:");
                userName = (await GetResponseAsync()).Text;
            }

            var address = string.Empty;
            while (string.IsNullOrWhiteSpace(address))
            {
                await manager.SendTextMessageAsync("Введите ваш адрес:");
                address = (await GetResponseAsync()).Text;
            }

            var email = string.Empty;
            while (string.IsNullOrWhiteSpace(email))
            {
                await manager.SendTextMessageAsync("Введите email, по которому можно будет с вами связаться:");
                email = (await GetResponseAsync()).Text;
            }

            var user = new User
            {
                Email = email,
                Address = address,
                Username = userName,
                UserId = manager.UserId,
            };
            await ProcessOldUser(user);
        }

        private string ChangeRequisitesHelp => "Если все данные введены корректно, воспользуйтесь команой /done\n" +
                                               "Если хотите исправить имя, воспользуйтесь командой /change_name\n" +
                                               "Если хотите исправить email, воспользуйтесь командой /change_email\n" +
                                               "Если хотите исправить адрес, воспользуйтесь командой /change_address";

        private string UserHelp => "Бот общественный квартальный.\n" +
                                   "Сперва вам нужно пройти процесс регистрации с помощью команды /register\n" +
                                   "После этого вы сможете создавать обращения квартальным города Екатеринбург с помощью команды /report\n" +
                                   "Для отмены воспользуйтесь командой /cancel";

        private IReplyMarkup resetMarkup = new ReplyKeyboardRemove();

        private string PrintUserInformation(User user)
        {
            return $"Имя: {user.Username}\n" +
                   $"e-mail: {user.Email}\n" +
                   $"Адрес: {user.Address}\n";
        }

        private async Task ProcessReport()
        {
            var user = cleanCityApi.GetUser(manager.UserId);
            if (user == null)
            {
                await manager.SendTextMessageAsync(
                    "Пройдите процедуру регистрации (/register), чтобы иметь возможность оформлять обращения к городским квартальным");
                return;
            }

            var attachments = new List<Attachment>();
            var markup = new ReplyKeyboardMarkup(new[]
            {
                new[] {new KeyboardButton("Грязь на тротуаре")},
                new[] {new KeyboardButton("Парковка в неположенном месте")},
                new[] {new KeyboardButton("Реклама в неположенном месте")},
                new[] {new KeyboardButton("Складированое в неположенном месте")},
            });
            var makeReport = new ReplyKeyboardMarkup(new[]
            {
                new[] {new KeyboardButton("Отправить обращение")},
            });

            var subject = string.Empty;
            while (string.IsNullOrWhiteSpace(subject))
            {
                await manager.SendTextMessageAsync("Выберите одну из предложенных тем обращения или введите свою:",
                    markup);
                subject = (await GetResponseAsync(attachments)).Text;
            }

            var reportText = string.Empty;
            while (string.IsNullOrWhiteSpace(reportText))
            {
                await manager.SendTextMessageAsync("Подробно опишите детали проблемы:", resetMarkup);
                reportText = (await GetResponseAsync(attachments)).Text;
            }

            GeoLocation location = null;
            while (location == null)
            {
                await manager.SendTextMessageAsync(
                    "Укажите своё местоположение",
                    new ReplyKeyboardMarkup(
                        KeyboardButton.WithRequestLocation("Отправить моё текущее местоположение")));
                var locationMessage = await GetResponseAsync(attachments);
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
                var (count, caption) =
                    Pluralizator.Pluralize(attachments.Count, "фотографий", "фотографию", "фотографии");
                await manager.SendTextMessageAsync(
                    $"Добавьте к своему обращению фотографии, чтобы оно было быстрее решено\n" +
                    $"Мы уже прикрепили к вашему обращению {count} {caption}. Вы можете отправить ещё фотографии или сформировать обращение",
                    makeReport);
                var message = await GetResponseAsync(attachments);
                if (message.Text != null && message.Text.Contains("обращение"))
                {
                    break;
                }
            }

            var initialReport = new InitialReport
            {
                UserId = manager.UserId,
                Subject = subject,
                ReportText = reportText,
                Location = location,
                Attachments = attachments.ToArray(),
            };
            var reportId = cleanCityApi.SendReport(initialReport);
            var report = cleanCityApi.GetReport(reportId);
            var responsible = cleanCityApi.GetResponsible(report.ResponsibleId);
            await manager.SendTextMessageAsync(
                $"Обращение успешно сформировано и отправлено соответствующему квартальному: " +
                $"{responsible.Name}\n" +
                $"Вы можете оформить ещё одно обращение с помощью команды /report", resetMarkup);
        }

        private async Task<Message> GetResponseAsync(List<Attachment> attachments)
        {
            var message = await GetResponseAsync();
            TryAddPhoto(attachments, message);
            return message;
        }

        private async Task<Message> GetResponseAsync()
        {
            var message = await manager.GetResponseAsync();
            if (message.Text == "/cancel")
            {
                await manager.SendTextMessageAsync($"Действие отменено.\n{UserHelp}", resetMarkup);
                throw new BotCancelOperationException("User interrupted session");
            }

            return message;
        }

        private void TryAddPhoto(List<Attachment> attachments, Message message)
        {
            if (message.Photo == null || !message.Photo.Any())
            {
                return;
            }

            var highResolutionPhoto = message.Photo.OrderByDescending(x => x.Width * x.Height).First();
            Console.WriteLine($"Download file: {highResolutionPhoto.FileId}");
            var photo = DownloadFile(highResolutionPhoto.FileId);
            var photoName = $"attachment_{attachments.Count}.jpg";
            attachments.Add(new Attachment
            {
                Data = photo,
                Filename = photoName,
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