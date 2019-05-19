using System;
using System.Net.Mime;
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
        private readonly ICleanCityApi cleanCityApi;

        public CleanCityController(ICleanCityApi cleanCityApi)
        {
            this.cleanCityApi = cleanCityApi;
        }

        [HttpGet("report/{reportId}/image/{imageId}")]
        public ActionResult GetImage(Guid reportId, int imageId)
        {
            var report = cleanCityApi.GetReport(reportId);
            if (report == null || report.Attachments.Length <= imageId || imageId < 0)
                return NotFound();
            var image = report.Attachments[imageId];
            return new FileContentResult(image.Data, MediaTypeNames.Image.Jpeg);
        }

        [HttpGet("report/{reportId}")]
        public ActionResult<Report> GetReport(Guid reportId)
        {
            var report = cleanCityApi.GetReport(reportId);
            foreach (var attachment in report.Attachments)
            {
                attachment.Data = null;
            }

            return report;
        }

        [HttpGet("reports")]
        public ActionResult<ReportPreview[]> GetReports(int start = 0, int count = 10)
        {
            return cleanCityApi.GetReports(start, count);
        }

        [HttpGet("reports/{responsibleId}")]
        public ActionResult<ReportPreview[]> GetReportsOfResponsible(Guid responsibleId, int start = 0, int count = 10)
        {
            return cleanCityApi.GetReports(responsibleId, start, count);
        }

        [HttpGet("responsible/{responsibleId}")]
        public ActionResult<Responsible> GetResponsible(Guid responsibleId)
        {
            return cleanCityApi.GetResponsible(responsibleId);
        }

        [HttpGet("responsibles")]
        public ActionResult<Responsible[]> GetResponsibles(int start = 0, int count = 10)
        {
            return cleanCityApi.GetResponsibles(start, count);
        }

        [HttpPost("add-doubler")]
        public void AddDoubler(AddDoublerQuery query)
        {
            var responsibleId = query.ResponsibleId;
            var doubler = query.Doubler;
            Console.WriteLine($"Adding doubler for {responsibleId}: info ({doubler.Email}, {doubler.Name})");
            cleanCityApi.AddDoubler(responsibleId, new Responsible
            {
                Id = Guid.NewGuid(),
                Name = doubler.Name,
                Email = doubler.Email,
                IsActive = true,
            });
        }
    }
}