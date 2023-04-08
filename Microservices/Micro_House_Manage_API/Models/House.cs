namespace Micro_House_Manage_API.Models
{
    public class House : CommonModel
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public double PredictedPrice { get; set; }
        public Guid? UserId { get; set; }
        public ICollection<Inquiry> Inquiries { get; set; }
        public Listing Listing { get; set; }
    }
}
