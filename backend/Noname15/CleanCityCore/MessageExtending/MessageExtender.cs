using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CleanCityCore.MessageExtending
{
    public class MessageExtender : IMessageExtender
    {
        public string ExtendSubject(string text)
        {
            return $"Заявка общественному квартальному: {text}";
        }

        public String ExtendReportText(string responsibleName, string text)
        {
            List<ElementMessageDeny> elemets = ElementMessagesBuilder.BuildList();
            List<string> returnDeny = new List<string>();

            var returnMsg = "Дорбрый день, " + responsibleName + "<br/>";

            foreach (var item in elemets)
            {
                Regex regex = new Regex(item.Keywords);
                MatchCollection matches = regex.Matches(text);
                if (matches.Count > 0) returnDeny.Add(item.Base);
            }

            int i = 1;
            if (returnDeny.Count > 0)
            {
                returnMsg +=
                    ".<br/> На основании пункта 7 РЕШЕНИЯ от 26 июня 2012 года N 29/61 Об утверждении правил благоустройства территории муниципального образования город Екатеринбург на территории муниципального образования город Екатеринбург запрещается:<br>";
                returnMsg += "<ol type=\"1\">";
                foreach (var item in returnDeny)
                {
                    returnMsg += $"<li>{item}</li>\n";
                }

                returnMsg += "<ol/>";

                returnMsg += "<br/>Сообщаю: " + text;
            }

            return returnMsg;
        }
    }
}