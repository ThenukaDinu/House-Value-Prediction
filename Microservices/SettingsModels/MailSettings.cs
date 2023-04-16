using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsModels
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string SecretKey { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
    }
}
