using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CleanCityCore.Model;

namespace CleanCityCore.MessageExtending
{
    public class MessageExtender : IMessageExtender
    {
        public string ExtendSubject(Report report)
        {
            return $"Заявка общественному квартальному: {report.Subject}";
        }

        public String ExtendReportText(Responsible responsible, User user, Report report)
        {
            List<ElementMessageDeny> elemets = ElementMessagesBuilder.BuildList();
            List<string> returnDeny = new List<string>();

            var returnMsg = "Добрый день, " + responsible.Name + "<br/>";

            foreach (var item in elemets)
            {
                Regex regex = new Regex(item.Keywords);
                MatchCollection matches = regex.Matches(report.ReportText);
                if (matches.Count > 0) returnDeny.Add(item.Base);
            }

            if (returnDeny.Count > 0)
            {
                returnMsg +=
                    "<br/>На основании пункта 7 РЕШЕНИЯ от 26 июня 2012 года N 29/61 \"Об утверждении правил благоустройства территории муниципального образования город Екатеринбург\" на территории муниципального образования город Екатеринбург запрещается:<br>";
                returnMsg += "<ol type=\"1\">";
                foreach (var item in returnDeny)
                {
                    returnMsg += $"<li>{item}</li>\n";
                }

                returnMsg += "</ol>";
            }

            var userDefined = user == null ? "" : $" от {user.Username} (email: {user.Email})";
            returnMsg += $"<br/>Обращение{userDefined}:<br/>" + report.ReportText;
            var geolocationLink =
                $"https://yandex.ru/maps/?ll={report.Location.Longitude}%2C{report.Location.Latitude}&" +
                $"mode=whatshere&" +
                $"whatshere%5Bpoint%5D=60.494202%2C56.809559&" +
                $"whatshere%5Bzoom%5D=10&z=14";
            returnMsg +=
                $"<br/>Геолокация, указанная пользователем: <a href=\"{geolocationLink}\">ссылка на Яндекс.Карты</a>";


            return returnMsg;
        }
    }
}