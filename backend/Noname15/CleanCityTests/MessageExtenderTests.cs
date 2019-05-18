using System;
using CleanCityCore;
using CleanCityCore.MessageExtending;
using NUnit.Framework;

namespace CleanCityTests
{
    [TestFixture]
    public class MessageExtenderTests
    {
        [Test]
        public void Test()
        {
            var extender = new MessageExtender().ExtendReportText(
                "Какой-то квартальный",
                "припаркован на газоне и лежит куча мусора");
            foreach (var item in extender)
            {
                Console.WriteLine(item);
            }
        }
    }
}