using System;
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

        [HttpGet("report/{reportId}")]
        public ActionResult<Report> GetReport(Guid reportId)
        {
            return cleanCityApi.GetReport(reportId);
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
            cleanCityApi.AddDoubler(responsibleId, doubler);
        }
    }
}
