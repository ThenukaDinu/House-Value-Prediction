using static Micro_House_Manage_API.Data.PublicEnum;

namespace Micro_House_Manage_API.Models
{
    public class Inquiry : CommonModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ListingId { get; set; }
        public string Message { get; set; }
        public InquiryStatus InquiryStatus { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime ResponseTime { get; set; }
        public House House { get; set; }
    }
}
