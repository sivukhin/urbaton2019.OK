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
            var extender = new MessageExtender().Extend("куча земли и куча листвы");
            foreach (var item in extender)
            {
                Console.WriteLine(item);
            }
            
        }
    }
}