using static Micro_House_Manage_API.Data.PublicEnum;

namespace Micro_House_Manage_API.Dtos
{
    public class InquiryDto
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public string Message { get; set; }
        public InquiryStatus InquiryStatus { get; set; }
        public string ResponseMessage { get; set; }
        public int HouseId { get; set; }
    }
}
