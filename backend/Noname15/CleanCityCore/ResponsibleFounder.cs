using System;
using CleanCityCore.Model;

namespace CleanCityCore
{
    public class ResponsibleFounder : IResponsibleFounder
    {
        public Responsible GetResponsible(GeoLocation location)
        {
            return new Responsible
            {
                Id = Guid.Parse("B4B8FEB8-981B-42A0-85D9-9B9AB18DC245"),
                Name = "Сивухин Никита",
                Email = "sivukhin.nikita@yandex.ru",
                IsActive = true,
            };
        }
    }
}