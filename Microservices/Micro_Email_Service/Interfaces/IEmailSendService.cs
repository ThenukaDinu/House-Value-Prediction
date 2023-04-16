using SettingsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro_Email_Service.Interfaces
{
    public interface IEmailSendService
    {
        public void Send(MailSettings mailSettings, string body = "", string subject = "", string to = "");
    }
}
