using static Data.PublicEnum;

namespace Models
{
    public class Inquiry : CommonModel
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public string Message { get; set; }
        public InquiryStatus InquiryStatus { get; set; }
        public string ResponseMessage { get; set; }
        public int HouseId { get; set; }
        public DateTime ResponseTime { get; set; }
        public Listing Listing { get; set; }
        public House House { get; set; }
    }
}
