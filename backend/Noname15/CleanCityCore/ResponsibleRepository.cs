using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CleanCityCore.Model;
using CleanCityCore.Sql;
using Microsoft.EntityFrameworkCore;

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
                var sqlResponsible = context.ResponsibleList.SingleOrDefault(x => x.Id == responsible.Id);
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

        public Responsible[] GetDoublers(Guid responsibleId)
        {
            using (var context = new CleanCityContext())
            {
                var responsible = context.ResponsibleList
                    .Include(x => x.DoublerList)
                    .SingleOrDefault(x => x.Id == responsibleId);
                if (responsible == null)
                    return new Responsible[0];
                var doublers = responsible.DoublerList;
                if (doublers == null)
                    return new Responsible[0];
                return doublers
                    .ToList()
                    .Select(x => new Responsible
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        IsActive = true,
                    })
                    .ToArray();
            }
        }

        public void AddDoubler(Guid responsibleId, Responsible doubler)
        {
            using (var context = new CleanCityContext())
            {
                var responsible = context.ResponsibleList.SingleOrDefault(x => x.Id == responsibleId);
                if (responsible == null)
                    return;
                var sqlDoubler = new ResponsibleDoublerSql
                {
                    Id = Guid.NewGuid(),
                    Name = doubler.Name,
                    Email = doubler.Email,
                    OriginalResponsible = responsible
                };
                if (responsible.DoublerList == null)
                    responsible.DoublerList = new List<ResponsibleDoublerSql>();
                responsible.DoublerList.Add(sqlDoubler);
                context.SaveChanges();
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