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
        public List<string> Keywords { get; set; }
        public int Number { get; set; }
 
    }
    public static class ElementMessagesBuilder
    {
        public static List<ElementMessageDeny> BuildList()
        {
            return new List<ElementMessageDeny>
            {
                {new ElementMessageDeny() {Number = 1, Base="вывоз снега, льда, мусора, твердых бытовых отходов, крупногабаритного мусора, строительного мусора, смета и иных отходов в не отведенные для этого места", 
                    Keywords = new List<string>{"вывоз снега", "отходы"} }},
                {new ElementMessageDeny() {Number = 2, Base="движение машин и механизмов на гусеничном ходу по дорогам с асфальто- и цементно-бетонным покрытием (за исключением случаев проведения аварийно-восстановительных работ)", 
                    Keywords = new List<string>{"трактор на гусеницах", "гусеницах"} }},
                {new ElementMessageDeny() {Number = 3, Base= "заезд и парковка транспортных средств и размещение объектов строительного или производственного оборудования на газонах, цветниках, детских и спортивных площадках", 
                    Keywords = new List<string>{"припаркован на газоне", "парковка на газоне", "на газоне", "на детской площадке", "парковка на детской площадке"} }},
                {new ElementMessageDeny() {Number = 4, Base= "засорение и засыпка водоемов, загрязнение водоемов сточными водами, устройство запруд", 
                    Keywords = new List<string>{"мусор в водоеме", "мусор в воде", "слив в водоем"} }},
                {new ElementMessageDeny() {Number = 5, Base= "мойка транспортных средств вне мест, специально оборудованных для этого", 
                    Keywords = new List<string>{"моет машину", "мойка машины"} }},
                {new ElementMessageDeny() {Number = 6, Base= "несанкционированная свалка мусора на отведенных и (или) прилегающих территориях", 
                    Keywords = new List<string>{"свалка", "лежит мусор", "выкинули мусор"} }},
                {new ElementMessageDeny() {Number = 7, Base= "перевозка грунта, мусора, сыпучих строительных материалов, легкой тары, листвы, ветвей деревьев без покрытия брезентом или другим материалом, исключающим загрязнение атмосферного воздуха и дорог", 
                    Keywords = new List<string>{"машина не накрыта"} }},
                {new ElementMessageDeny() {Number = 8, Base= "подметание и вакуумная уборка дорог и тротуаров без предварительного увлажнения в летний период", 
                    Keywords = new List<string>{"разлетается пыль"} }},
                {new ElementMessageDeny() {Number = 9, Base= "производство земляных работ без разрешения, оформленного в соответствии с Решением Екатеринбургской городской Думы от 30 октября 2008 года N 58/63 Об утверждении Положения о порядке выдачи разрешений на производство земляных работ при строительстве, реконструкции и ремонте сетей инженерно-технического обеспечения и иных объектов, связанных с нарушением благоустройства территории муниципального образования город Екатеринбург, за исключением случаев, предусмотренных указанным Решением", 
                    Keywords = new List<string>{"вырыли яму", "копают яму"} }},
                {new ElementMessageDeny() {Number = 10, Base= "самовольное размещение малых архитектурных форм на землях общего пользования", 
                    Keywords = new List<string>{"поставили киоск", "поставили будку"} }},
                {new ElementMessageDeny() {Number = 11, Base= "размещение штендеров на тротуарах, пешеходных путях передвижения, парковках автотранспорта, расположенных на землях общего пользования", 
                    Keywords = new List<string>{"реклама на тротуаре", "банер на пешеходной зоне"} }},
                {new ElementMessageDeny() {Number = 12, Base= "самовольное размещение объявлений вне мест, специально отведенных для этого правовыми актами Администрации города Екатеринбурга", 
                    Keywords = new List<string>{"реклама на доме", "объявление на стене"} }},
                {new ElementMessageDeny() {Number = 13, Base= "размещение парковочных барьеров и оградительных сигнальных конусов на землях общего пользования, за исключением случаев проведения аварийно-восстановительных и ремонтных работ", 
                    Keywords = new List<string>{"захватили парковку", "парковочный барьер"} }},
                {new ElementMessageDeny() {Number = 14, Base= "размещение ритуальных принадлежностей и надгробных сооружений вне мест, специально предназначенных для этих целей", 
                    Keywords = new List<string>{"нагробье"} }},
                {new ElementMessageDeny() {Number = 15, Base= "размещение сырья, материалов, грунта, оборудования за пределами земельных участков, отведенных под застройку частными (индивидуальными) жилыми домами", 
                    Keywords = new List<string>{"куча земли", "земля"} }},
                {new ElementMessageDeny() {Number = 16, Base= "размещение, сброс бытового и строительного мусора, металлического лома, отходов производства, тары, вышедших из эксплуатации автотранспортных средств, ветвей деревьев, листвы в не отведенных под эти цели местах", 
                    Keywords = new List<string>{"куча листвы", "куча лома", "заброщенный автомобиль"} }},
                {new ElementMessageDeny() {Number = 17, Base= "самовольное присоединение промышленных, хозяйственно-бытовых и иных объектов к сетям ливневой канализации", 
                    Keywords = new List<string>{"слив в ливневку", "сливают в ливневку"} }},
                {new ElementMessageDeny() {Number = 18, Base= "сброс сточных вод и загрязняющих веществ в водные объекты и на землю", 
                    Keywords = new List<string>{"сброс сточных вод", "сброс воды"} }},
                {new ElementMessageDeny() {Number = 19, Base= "сгребание листвы, снега и грязи к комлевой части деревьев, кустарников", 
                    Keywords = new List<string>{"деревья в мусоре", "грязь на кустах"} }},
                {new ElementMessageDeny() {Number = 20, Base= "самовольное разведение костров и сжигание мусора, листвы, тары, отходов, резинотехнических изделий на землях общего пользования", 
                    Keywords = new List<string>{"жгут мусор", "костер"} }},
                {new ElementMessageDeny() {Number = 21, Base= "складирование тары вне торговых сооружений", 
                    Keywords = new List<string>{"мусор у магазина", "ящики у магазина"} }},
                {new ElementMessageDeny() {Number = 22, Base= "размещение запасов кабеля вне распределительного муфтового шкафа", 
                    Keywords = new List<string>{"кабель", "провод"} }}
            };
        }
    }
    
    public class MessageExtender : IMessageExtender
    {
        public String Extend(String text)
        {
           List<ElementMessageDeny> elemets = ElementMessagesBuilder.BuildList();
           var messageReturnEnd = " пункта 7 решения от 26 июня 2012 года N 29/61 ОБ УТВЕРЖДЕНИИ ПРАВИЛ БЛАГОУСТРОЙСТВА ТЕРРИТОРИИ МУНИЦИПАЛЬНОГО ОБРАЗОВАНИЯ ГОРОД ЕКАТЕРИНБУРГ На территории муниципального образования город Екатеринбург запрещается: ";
           var messageReturn = "На основании абзаца ";
           foreach (var item in elemets)
           {
               foreach (var itemKeyword in item.Keywords)
               {
                   if (text.IndexOf(itemKeyword) > -1) messageReturn += item.Number + messageReturnEnd + item.Base;
               }
           }
            

            return messageReturn;
            throw new NotImplementedException();
        }
    }
}