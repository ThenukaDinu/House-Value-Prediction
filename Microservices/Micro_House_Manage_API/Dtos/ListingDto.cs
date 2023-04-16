using static Data.PublicEnum;

namespace Micro_House_Manage_API.Dtos
{
    public class ListingDto
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public int HouseId { get; set; }
        public string Description { get; set; }
        public ListingStatus ListingStatus { get; set; }
        public bool IsFeatured { get; set; }
    }
}
