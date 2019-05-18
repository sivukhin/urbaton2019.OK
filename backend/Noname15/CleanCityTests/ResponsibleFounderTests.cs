using System;
using CleanCityCore;
using CleanCityCore.Model;
using NUnit.Framework;

namespace CleanCityTests
{
    [TestFixture]
    public class ResponsibleFounderTests
    {
        [Test]
        public void Test()
        {
            var responsible = new ResponsibleFounder().GetResponsible(new GeoLocation
            {
                Latitude = 56.809457,
                Longitude = 60.493791
            });
            Console.WriteLine(responsible.Name);
        }
    }
}