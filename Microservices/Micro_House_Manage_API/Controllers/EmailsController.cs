using Micro_House_Manage_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using Models;

namespace Micro_House_Manage_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly ILogger<EmailsController> _logger;
        private readonly IMessageProducer _messageProducer;

        public EmailsController(ILogger<EmailsController> logger, IMessageProducer messageProducer)
        {
            _logger = logger;
            _messageProducer = messageProducer;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _logger.Log(LogLevel.Information, "Emails Get");

                var email = new EmailMessage()
                {
                    Body = "<p>This is a test email body</p>",
                    Subject = "This is a test email subject",
                    To = "thenukadev@gmail.com"
                };

                _messageProducer.SendingMessage<EmailMessage>(email, "emails", "emails");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
