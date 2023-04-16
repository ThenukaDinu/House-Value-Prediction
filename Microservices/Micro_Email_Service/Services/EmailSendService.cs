using Data;
using MailKit.Net.Smtp;
using MailKit.Security;
using Micro_Email_Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Models;
using SettingsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Data.PublicEnum;
using Attachment = Models.Attachment;

namespace Micro_Email_Service.Services
{
    public class EmailSendService : IEmailSendService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailSendService> _logger;

        public EmailSendService(IConfiguration configuration, ILogger<EmailSendService> logger)
        {
            _config  = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string body = "", string subject = "", string to = "", List<Attachment> attachments = null)
        {
            try
            {
                MailSettings mailSettings = _config.GetSection("MailSettings").Get<MailSettings>();

                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                var builder = new BodyBuilder();
                if (attachments != null)
                {
                    foreach (var file in attachments)
                    {
                        builder.Attachments.Add(file.FileName, file.FileBytes, ContentType.Parse(file.ContentType));
                    }
                }
                builder.HtmlBody = body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.SecretKey);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending email");
            }
        }

        public Task SendEmailAsync(EmailTemplateType emailTemplateType, string body = "", string subject = "", string to = "", List<Attachment> attachments = null)
        {
            throw new NotImplementedException();
        }
    }
}
