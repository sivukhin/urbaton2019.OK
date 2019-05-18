using System;
using CleanCityCore;
using NUnit.Framework;

namespace CleanCityTests
{
    [TestFixture]
    public class MessageExtenderTests
    {
        [Test]
        public void Test()
        {
            var extender = new MessageExtender().Extend("припаркован на газоне и лежит куча мусора");
            foreach (var item in extender)
            {
                Console.WriteLine(item);
            }
            
        }
    }
}