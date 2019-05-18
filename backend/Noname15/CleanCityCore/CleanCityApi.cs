using System;
using System.Collections.Generic;
using System.Linq;
using CleanCityCore.EmailSender;
using CleanCityCore.Model;

namespace CleanCityCore
{
    public class CleanCityApi : ICleanCityApi
    {
        private readonly IEmailRepository emailRepository;
        private readonly IResponsibleFounder responsibleFounder;
        private readonly IReportRepository reportRepository;
        private readonly IResponsibleRepository responsibleRepository;
        private readonly IMessageExtender messageExtender;

        public CleanCityApi(
            IEmailRepository emailRepository,
            IResponsibleFounder responsibleFounder,
            IReportRepository reportRepository,
            IResponsibleRepository responsibleRepository,
            IMessageExtender messageExtender)
        {
            this.emailRepository = emailRepository;
            this.responsibleFounder = responsibleFounder;
            this.reportRepository = reportRepository;
            this.responsibleRepository = responsibleRepository;
            this.messageExtender = messageExtender;
        }

        public Responsible[] GetResponsibles(int start, int count)
        {
            var responsibleIds = responsibleRepository.ReadResponsibles();
            return responsibleIds
                .Select(id => responsibleRepository.ReadResponsible(id))
                .Skip(start)
                .Take(count)
                .ToArray();
        }

        public ReportPreview[] GetReports(Guid responsibleId, int start, int count)
        {
            return ReadReports(reportRepository
                .ReadReportsOfResponsible(responsibleId)
                .Skip(start)
                .Take(count)
                .ToArray());
        }

        public ReportPreview[] GetReports(int start, int count)
        {
            return ReadReports(reportRepository
                .ReadReports()
                .Skip(start)
                .Take(count)
                .ToArray());
        }

        private ReportPreview[] ReadReports(IEnumerable<Guid> reportIds)
        {
            var reports = reportIds
                .Select(id => (Id: id, Report: reportRepository.ReadReport(id)))
                .Select(report => new ReportPreview
                {
                    Id = report.Id,
                    Subject = report.Report.Subject,
                    ReportText = report.Report.ReportText,
                    CreationDate = report.Report.CreationDate,
                })
                .ToArray();
            return reports;
        }

        public Report GetReport(Guid reportId)
        {
            return reportRepository.ReadReport(reportId);
        }

        public Guid SendReport(InitialReport report)
        {
            var responsible = responsibleFounder.GetResponsible(report.Location);
            if (!responsible.IsActive)
            {
                // todo(sivukhin, 18.05.2019): Handle inactive responsible case
                throw new Exception();
            }

            var reportId = reportRepository.AddReport(new Report
            {
                Subject = report.Subject,
                Location = report.Location,
                ReportText = report.ReportText,
                Attachments = report.Attachments,
                CreationDate = DateTime.UtcNow,
                ResponsibleId = responsible.Id,
            });

            // todo(sivukhin, 18.05.2019): Use message extender here
            var messageBody = "На основании пункта 7 РЕШЕНИЯ от 26 июня 2012 года N 29/61 Об утверждении правил благоустройства территории муниципального образования город Екатеринбург на территории муниципального образования город Екатеринбург запрещается: \\n"
                    +messageExtender.Extend(report.ReportText)+". \\n Сообщаю: " +report.ReportText.ToLower();
            
            
            emailRepository.AddEmail(new EmailMessage
            {
                Body = messageBody,
                Subject = report.Subject,
                RecipientEmail = responsible.Email,
                Attachments = report.Attachments,
            });
            return reportId;
        }
    }
}