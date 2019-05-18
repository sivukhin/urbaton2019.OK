using System;
using System.Linq;
using CleanCityCore;
using CleanCityCore.Model;
using NUnit.Framework;

namespace CleanCityTests
{
    public class FakeResponsibleRepository : IResponsibleRepository
    {
        public Responsible ReadResponsible(Guid responsibleId)
        {
            return null;
        }

        public Guid[] ReadResponsibles()
        {
            return new Guid[0];
        }

        public Guid AddResponsible(Responsible responsible)
        {
            return Guid.Empty;
        }

        public Responsible[] GetDoublers(Guid responsibleId)
        {
            return new Responsible[0];
        }
    }

    [TestFixture]
    public class ResponsibleFounderTests
    {
        [Test]
        public void Test()
        {
            var responsible = new ResponsibleFounder(new FakeResponsibleRepository()).GetResponsible(new GeoLocation
            {
                Latitude = 56.809457,
                Longitude = 60.493791
            });
            Console.WriteLine(responsible.Name);
        }

        [Test]
        public void TestAll()
        {
            var responsibles = new ResponsibleFounder(new FakeResponsibleRepository()).GetAllResponsibles();
            Console.WriteLine(string.Join(", ", responsibles.Select(x => x.Name)));
        }
    }
}