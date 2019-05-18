using CleanCityCore;
using CleanCityCore.EmailSender;
using Microsoft.AspNetCore.Mvc;

namespace BackendApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class CleanCityController : ControllerBase
    {
        private readonly IEmailRepository emailRepository;

        public CleanCityController(IEmailRepository emailRepository)
        {
            this.emailRepository = emailRepository;
        }

        [HttpPost("send-email")]
        public void SendEmail(EmailMessage emailMessage)
        {
            emailRepository.AddEmail(emailMessage);
        }
    }
}