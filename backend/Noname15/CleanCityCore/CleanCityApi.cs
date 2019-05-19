using System;
using System.Collections.Generic;
using System.Linq;
using CleanCityCore.EmailSender;
using CleanCityCore.MessageExtending;
using CleanCityCore.Model;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;

namespace CleanCityCore
{
    public class CleanCityApi : ICleanCityApi
    {
        private readonly IEmailRepository emailRepository;
        private readonly IResponsibleFounder responsibleFounder;
        private readonly IReportRepository reportRepository;
        private readonly IResponsibleRepository responsibleRepository;
        private readonly IUserRepository userRepository;
        private readonly IMessageExtender messageExtender;

        public CleanCityApi(
            IEmailRepository emailRepository,
            IResponsibleFounder responsibleFounder,
            IReportRepository reportRepository,
            IResponsibleRepository responsibleRepository,
            IUserRepository userRepository,
            IMessageExtender messageExtender)
        {
            this.emailRepository = emailRepository;
            this.responsibleFounder = responsibleFounder;
            this.reportRepository = reportRepository;
            this.responsibleRepository = responsibleRepository;
            this.userRepository = userRepository;
            this.messageExtender = messageExtender;
        }

        public Responsible GetResponsible(Guid responsibleId)
        {
            return responsibleRepository.ReadResponsible(responsibleId);
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
            var report = reportRepository.ReadReport(reportId);
            report.Subject = messageExtender.ExtendSubject(report);
            report.ReportText = messageExtender.ExtendReportText(report);
            return report;
        }

        public Guid SendReport(InitialReport report)
        {
            var responsible = responsibleFounder.GetResponsible(report.Location);
            Console.WriteLine($"Found responsible: {JsonConvert.SerializeObject(responsible)}");
            if (!responsible.IsActive)
            {
                // todo(sivukhin, 18.05.2019): Handle inactive responsible case
                throw new Exception();
            }

            var fullReport = new Report
            {
                UserId = report.UserId,
                Subject = report.Subject,
                Location = report.Location,
                ReportText = report.ReportText,
                Attachments = report.Attachments,
                CreationDate = DateTime.UtcNow,
                ResponsibleId = responsible.Id,
            };
            var reportId = reportRepository.AddReport(fullReport);

            var user = userRepository.GetUser(report.UserId);
            emailRepository.AddEmail(new EmailMessage
            {
                Body = messageExtender.ExtendReportText(responsible, user, fullReport),
                Subject = messageExtender.ExtendSubject(fullReport),
                RecipientEmail = responsible.Email,
                ResponsibleId = responsible.Id,
                Attachments = report.Attachments,
            });
            return reportId;
        }

        public void AddOrUpdateUser(User user)
        {
            userRepository.AddOrUpdateUser(user);
        }

        public User GetUser(long userId)
        {
            return userRepository.GetUser(userId);
        }

        public void AddDoubler(Guid responsibleId, Responsible doubler)
        {
            responsibleRepository.AddDoubler(responsibleId, doubler);
        }
    }
}