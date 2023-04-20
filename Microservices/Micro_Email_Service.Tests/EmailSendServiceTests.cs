using FakeItEasy;
using SettingsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Micro_Email_Service.Services;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using FluentAssertions;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Micro_Email_Service.Tests
{
    public class EmailSendServiceTests
    {
        public EmailSendServiceTests() { }

        [Fact]
        public async Task SendEmailAsync_Should_Send_Email_With_Correct_Details()
        {
            // Arrange
            var configuration = A.Fake<IConfiguration>();
            var logger = A.Fake <ILogger<EmailSendService>>();
            var mailSettings = A.Fake<MailSettings>();
            //A.CallTo(() => configuration.GetSection("MailSettings")).Returns(mailSettings);
            var emailSendService = new EmailSendService(configuration, logger);
            var smtp = A.Fake<SmtpClient>();

            var body = "Test email body";
            var subject = "Test email subject";
            var to = "test@example.com";

            // Act
            await emailSendService.SendEmailAsync(body, subject, to);

            // Assert
            A.CallTo(() => configuration.GetSection("MailSettings")).MustHaveHappenedOnceExactly();
        }
    }
}
