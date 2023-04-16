using static Data.PublicEnum;

namespace Models
{
    public class Listing : CommonModel
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        public int HouseId { get; set; }
        public string Description { get; set; }
        public ListingStatus ListingStatus { get; set; }
        public bool IsFeatured { get; set; }
        public House House { get; set; }
        public double ListingPrice { get; set; }
    }
}
