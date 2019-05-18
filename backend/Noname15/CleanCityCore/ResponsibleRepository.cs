using System;
using System.Linq;
using System.Net;
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
                // todo(sivukhin, 18.05.2019): Fix data race here
                var sqlResponsible = context.ResponsibleList.SingleOrDefault(x => x.Email == responsible.Email);
                if (sqlResponsible != null)
                    return sqlResponsible.Id;
                context.ResponsibleList.Add(new ResponsibleSql
                {
                    Id = responsible.Id,
                    Name = responsible.Name,
                    Email = responsible.Email,
                    IsActive = responsible.IsActive,
                });
                context.SaveChanges();
                return responsible.Id;
            }
        }

        private Responsible ParseFromSql(ResponsibleSql responsibleSql)
        {
            return new Responsible
            {
                Id = responsibleSql.Id,
                Name = responsibleSql.Name,
                Email = responsibleSql.Email,
                IsActive = responsibleSql.IsActive,
            };
        }
    }
}