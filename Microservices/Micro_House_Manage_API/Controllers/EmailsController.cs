using Helpers;
using Micro_House_Manage_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Requests;
using System.Net.Mail;
using Attachment = Models.Attachment;

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

        [HttpPost]
        public IActionResult Post([FromForm] MailRquest mailRquest)
        {
            try
            {
                _logger.Log(LogLevel.Information, "Email send called.");

                List<Attachment> attachments = new();
                mailRquest.Attachments.ForEach(async x =>
                {
                    var byteArray = await FileHelpers.ConvertFileToByteArrayAsync(x);
                    attachments.Add(new Attachment
                    {
                        ContentType = x.ContentType,
                        FileName = x.FileName,
                        FileBytes = byteArray
                    });
                });

                var email = new EmailMessage()
                {
                    Body = mailRquest.Body,
                    Subject = mailRquest.Subject,
                    To = mailRquest.ToEmail,
                    Attachments = attachments
                };

                _messageProducer.SendingMessage(email, "emails", "emails");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
