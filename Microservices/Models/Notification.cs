using static Data.PublicEnum;

namespace Models
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
