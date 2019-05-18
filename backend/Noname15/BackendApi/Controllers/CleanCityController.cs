using System;
using System.Net.Mail;
using CleanCityCore;
using CleanCityCore.EmailSender;
using CleanCityCore.Model;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class CleanCityController : ControllerBase
    {
        private readonly IEmailRepository emailRepository;
        private readonly IResponsibleFounder responsibleFounder;
        private readonly IReportRepository reportRepository;
        private readonly IMessageExtender messageExtender;

        public CleanCityController(
            IEmailRepository emailRepository,
            IResponsibleFounder responsibleFounder,
            IReportRepository reportRepository,
            IMessageExtender messageExtender)
        {
            this.emailRepository = emailRepository;
            this.responsibleFounder = responsibleFounder;
            this.reportRepository = reportRepository;
            this.messageExtender = messageExtender;
        }

        [HttpPost("send-email")]
        public void SendEmail(EmailMessage emailMessage)
        {
            emailRepository.AddEmail(emailMessage);
        }

        [HttpPost("send-report")]
        public void SendReport(InitialReport report)
        {
            var responsible = responsibleFounder.GetResponsible(report.Location);
            if (!responsible.IsActive)
            {
                // todo(sivukhin, 18.05.2019): Handle inactive responsible case
                throw new Exception();
            }

            reportRepository.AddReport(new Report
            {
                Subject = report.Subject,
                Location = report.Location,
                ReportText = report.ReportText,
                Attachments = report.Attachments,
                CreationDate = DateTime.UtcNow,
                ResponsibleId = responsible.Id,
            });
            
            //            var messageBody = messageExtender.Extend(report.ReportText);
            var messageBody = report.ReportText;
            emailRepository.AddEmail(new EmailMessage
            {
                Body = messageBody,
                Subject = report.Subject,
                RecipientEmail = responsible.Email,
                Attachments = report.Attachments,
            });
        }
    }
}