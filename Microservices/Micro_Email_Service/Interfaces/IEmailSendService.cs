using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Data.PublicEnum;

namespace Micro_Email_Service.Interfaces
{
    public interface IEmailSendService
    {
        public Task SendEmailAsync(string body = "", string subject = "", string to = "", List<Attachment> attachments = null);
        public Task SendEmailAsync(EmailTemplateType emailTemplateType,string body = "", string subject = "", string to = "", List<Attachment> attachments = null);
    }
}
