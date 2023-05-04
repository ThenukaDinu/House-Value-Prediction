using static Data.PublicEnum;

namespace Models
{
    public class Interest : CommonModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public InterestType Type { get; set; }
        public Guid UserId { get; set; }
    }
}
