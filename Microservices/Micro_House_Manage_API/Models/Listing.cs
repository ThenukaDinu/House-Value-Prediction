using static Micro_House_Manage_API.Data.PublicEnum;

namespace Micro_House_Manage_API.Models
{
    public class Listing : CommonModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int HouseId { get; set; }
        public string Description { get; set; }
        public ListingStatus ListingStatus { get; set; }
        public bool IsFeatured { get; set; }
    }
}
