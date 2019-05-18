using System;
using System.Linq;
using CleanCityCore.Sql;

namespace CleanCityCore
{
    public class ResponsibleRepository : IResponsibleRepository
    {
        public Responsible ReadResponsible(Guid responsibleId)
        {
            using (var context = new CleanCityContext())
            {
                var responsible = context.ResponsibleList.SingleOrDefault(r => r.Id == responsibleId);
                if (responsible == null)
                    return null;
                return ParseFromSql(responsible);
            }
        }

        public Guid[] ReadResponsibles()
        {
            using (var context = new CleanCityContext())
            {
                return context.ResponsibleList.Select(x => x.Id).ToArray();
            }
        }

        public Guid AddResponsible(Responsible responsible)
        {
            using (var context = new CleanCityContext())
            {
                var responsibleId = Guid.NewGuid();
                context.ResponsibleList.Add(new ResponsibleSql
                {
                    Id = responsibleId,
                    Name = responsible.Name,
                    Email = responsible.Email,
                    IsActive = responsible.IsActive,
                });
                context.SaveChanges();
                return responsibleId;
            }
        }

        private Responsible ParseFromSql(ResponsibleSql responsibleSql)
        {
            return new Responsible
            {
                Name = responsibleSql.Name,
                Email = responsibleSql.Email,
                IsActive = responsibleSql.IsActive,
            };
        }
    }
}