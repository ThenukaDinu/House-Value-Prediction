using static Micro_House_Manage_API.Data.PublicEnum;

namespace Micro_House_Manage_API.Models
{
    public class Notification : CommonModel
    {
        public int Id { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public bool IsSent { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
    }
}
