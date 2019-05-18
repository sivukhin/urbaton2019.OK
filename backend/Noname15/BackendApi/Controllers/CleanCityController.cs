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

        [HttpPost("send-report")]
        public ActionResult<Guid> SendReport(InitialReport report)
        {
            return cleanCityApi.SendReport(report);
        }
    }
}