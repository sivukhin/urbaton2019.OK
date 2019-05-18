using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CleanCityCore
{
    public interface IMessageExtender
    {
        string Extend(string text);
    }

    public class ElementMessageDeny
    {
        public string Base { get; set; }
        public string Keywords { get; set; }
        public int Number { get; set; }
 
    }
    public static class ElementMessagesBuilder
    {
        public static List<ElementMessageDeny> BuildList()
        {
            return new List<ElementMessageDeny>
            {
                {new ElementMessageDeny() {Number = 1, Base="вывоз снега, льда, мусора, твердых бытовых отходов, крупногабаритного мусора, строительного мусора, смета и иных отходов в не отведенные для этого места", 
                    Keywords = @"(\w*выв.з\s*снег.|\w*выв.з\s*льда\w*|\w*выв.з\s*мус.р\w*)" }},
                {new ElementMessageDeny() {Number = 2, Base="движение машин и механизмов на гусеничном ходу по дорогам с асфальто- и цементно-бетонным покрытием (за исключением случаев проведения аварийно-восстановительных работ)", 
                    Keywords = @"(\w*тракт.р\sна\s*гусеницах\w*|\w*гусеничном\w*|\w*движение\w*гусеничном\w*)" }},
                {new ElementMessageDeny() {Number = 3, Base= "заезд и парковка транспортных средств и размещение объектов строительного или производственного оборудования на газонах, цветниках, детских и спортивных площадках", 
                    Keywords = @"(\w*пр.п.рков\w*\s*\w*\s*г.зоне|\w*п.рковка\w*\s*\w*\s*г.зоне\w*|\w*на\sг.зоне\w*|\w*на\sде.ской\sплощадке\w*|\w*парков\w*\s*\w*д..ской\w*)" }},
                {new ElementMessageDeny() {Number = 4, Base= "засорение и засыпка водоемов, загрязнение водоемов сточными водами, устройство запруд", 
                    Keywords = @"(\w*мусор\w*\s*\w*вод\w*|\w*слив\w*\s*\w*вод\w*)" }},
                {new ElementMessageDeny() {Number = 5, Base= "мойка транспортных средств вне мест, специально оборудованных для этого", 
                    Keywords = @"(\w*мо\w*\s*\w*\s*маш\w*)" }},
                {new ElementMessageDeny() {Number = 6, Base= "несанкционированная свалка мусора на отведенных и (или) прилегающих территориях", 
                    Keywords = @"(\w*свалка\w*|\w*лежит\w*\s*\w*\s*\w*мусор\w*|\w*выкинули\w*\s*\w*мусор\w*)" }},
                {new ElementMessageDeny() {Number = 7, Base= "перевозка грунта, мусора, сыпучих строительных материалов, легкой тары, листвы, ветвей деревьев без покрытия брезентом или другим материалом, исключающим загрязнение атмосферного воздуха и дорог", 
                    Keywords = @"(\w*машина\s*\w*\s*накрыта)"}},
                {new ElementMessageDeny() {Number = 8, Base= "подметание и вакуумная уборка дорог и тротуаров без предварительного увлажнения в летний период", 
                    Keywords = @"(\w*разлетается\w*пыль\w*)" }},
                {new ElementMessageDeny() {Number = 9, Base= "производство земляных работ без разрешения, оформленного в соответствии с Решением Екатеринбургской городской Думы от 30 октября 2008 года N 58/63 Об утверждении Положения о порядке выдачи разрешений на производство земляных работ при строительстве, реконструкции и ремонте сетей инженерно-технического обеспечения и иных объектов, связанных с нарушением благоустройства территории муниципального образования город Екатеринбург, за исключением случаев, предусмотренных указанным Решением", 
                    Keywords = @"(\w*вырыли\s*\w*\s*яму\w*)"}},
                {new ElementMessageDeny() {Number = 10, Base= "самовольное размещение малых архитектурных форм на землях общего пользования", 
                    Keywords = @"(\w*поставили\s*\w*\s*киоск\w*|\w*поставили\s*\w*\s*будку\w*)"}},
                {new ElementMessageDeny() {Number = 11, Base= "размещение штендеров на тротуарах, пешеходных путях передвижения, парковках автотранспорта, расположенных на землях общего пользования", 
                    Keywords = @"(\w*реклама\s*\w*\s*тротуаре\w*|\w*банер\s*\w*\s*пешеход\w*)" }},
                {new ElementMessageDeny() {Number = 12, Base= "самовольное размещение объявлений вне мест, специально отведенных для этого правовыми актами Администрации города Екатеринбурга", 
                    Keywords = @"(\w*реклама\s*\w*\s*доме\w*|\w*объявление\s*\w*\s*стене\w*)" }},
                {new ElementMessageDeny() {Number = 13, Base= "размещение парковочных барьеров и оградительных сигнальных конусов на землях общего пользования, за исключением случаев проведения аварийно-восстановительных и ремонтных работ", 
                    Keywords = @"(\w*захват\w*\s*\w*\s*парк\w*|\w*парк\w*\s*\w*\s*барьер\w*)" }},
                {new ElementMessageDeny() {Number = 14, Base= "размещение ритуальных принадлежностей и надгробных сооружений вне мест, специально предназначенных для этих целей", 
                    Keywords = @"(\w*надгроб.е\w*)" }},
                {new ElementMessageDeny() {Number = 15, Base= "размещение сырья, материалов, грунта, оборудования за пределами земельных участков, отведенных под застройку частными (индивидуальными) жилыми домами", 
                    Keywords = @"(\w*куча\w*\s*\w*\s*зем\w*)" }},
                {new ElementMessageDeny() {Number = 16, Base= "размещение, сброс бытового и строительного мусора, металлического лома, отходов производства, тары, вышедших из эксплуатации автотранспортных средств, ветвей деревьев, листвы в не отведенных под эти цели местах", 
                    Keywords = @"(\w*куча\w*\s*\w*\s*листвы\w*|\w*брош\w*\s*\w*\s*\w*авто\w*)" }},
                {new ElementMessageDeny() {Number = 17, Base= "самовольное присоединение промышленных, хозяйственно-бытовых и иных объектов к сетям ливневой канализации", 
                    Keywords = @"(\w*слив\w*\s*\w*\s*\w*ливнев\w*)" }},
                {new ElementMessageDeny() {Number = 18, Base= "сброс сточных вод и загрязняющих веществ в водные объекты и на землю", 
                    Keywords = @"(\w*сброс\w*\s*\w*\s*\w*сточ\w*\s*\w*вод)" }},
                {new ElementMessageDeny() {Number = 19, Base= "сгребание листвы, снега и грязи к комлевой части деревьев, кустарников", 
                    Keywords = @"(\w*дерев\w*\s*\w*\s*\w*мусор\w*|\w*гряз\w*\s*\w*куст\w*)" }},
                {new ElementMessageDeny() {Number = 20, Base= "самовольное разведение костров и сжигание мусора, листвы, тары, отходов, резинотехнических изделий на землях общего пользования", 
                    Keywords = @"(\w*жгут\w*\s*\w*\s*мусор\w*|\w*костер\w*)" }},
                {new ElementMessageDeny() {Number = 21, Base= "складирование тары вне торговых сооружений", 
                    Keywords = @"(\w*мусор\w*\s*\w*\s*\w*магазина\w*|\w*ящики\w*\s*\w*\s*\w*магазина\w*)" }},
                {new ElementMessageDeny() {Number = 22, Base= "размещение запасов кабеля вне распределительного муфтового шкафа", 
                    Keywords = @"(\w*кабель\w*|\w*провод\w*)" }}
            };
        }
    }
    
    public class MessageExtender : IMessageExtender
    {
        public String Extend(String text)
        {
           List<ElementMessageDeny> elemets = ElementMessagesBuilder.BuildList();
           List<string> returnDeny = new List<string>();

           var returnMsg = "";
           
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
                   ".\\n На основании пункта 7 РЕШЕНИЯ от 26 июня 2012 года N 29/61 Об утверждении правил благоустройства территории муниципального образования город Екатеринбург на территории муниципального образования город Екатеринбург запрещается: \\n";
               foreach (var item in returnDeny)
               {
                   returnMsg += i.ToString() + item + "\\n";
               }
           }

           return returnMsg;
            
            throw new NotImplementedException();
        }
    }
}