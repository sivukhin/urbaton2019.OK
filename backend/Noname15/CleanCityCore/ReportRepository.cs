using System;
using System.Linq;
using System.Text;
using CleanCityCore.Model;
using CleanCityCore.Sql;
using Newtonsoft.Json;
using NpgsqlTypes;

namespace CleanCityCore
{
    public class ReportRepository : IReportRepository
    {
        public Guid AddReport(Report report)
        {
            using (var context = new CleanCityContext())
            {
                var reportId = Guid.NewGuid();
                context.Reports.Add(new ReportSql
                {
                    Id = reportId,
                    Body = report.ReportText,
                    CreationDate = report.CreationDate,
                    Subject = report.Subject,
                    Location = new NpgsqlPoint(report.Location.Latitude, report.Location.Longitude),
                    ResponsibleId = report.ResponsibleId,
                    Payload = SerializeReport(report),
                });
                context.SaveChanges();
                return reportId;
            }
        }

        public Guid[] ReadReports()
        {
            using (var context = new CleanCityContext())
            {
                return context
                    .Reports
                    .OrderByDescending(x => x.CreationDate)
                    .Select(x => x.Id)
                    .ToArray();
            }
        }

        public Guid[] ReadReportsOfResponsible(Guid responsibleId)
        {
            using (var context = new CleanCityContext())
            {
                return context
                    .Reports
                    .OrderByDescending(x => x.CreationDate)
                    .Where(x => x.ResponsibleId == responsibleId)
                    .Select(x => x.Id)
                    .ToArray();
            }
        }

        public Report ReadReport(Guid reportId)
        {
            using (var context = new CleanCityContext())
            {
                var rawReport = context.Reports.SingleOrDefault(x => x.Id == reportId);
                if (rawReport == null)
                    return null;
                return DeserializeObject(rawReport.Payload);
            }
        }

        private byte[] SerializeReport(Report report)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(report));
        }

        private Report DeserializeObject(byte[] data)
        {
            return JsonConvert.DeserializeObject<Report>(Encoding.UTF8.GetString(data));
        }
    }
}