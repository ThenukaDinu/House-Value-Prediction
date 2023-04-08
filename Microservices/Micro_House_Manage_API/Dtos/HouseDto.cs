namespace Micro_House_Manage_API.Dtos
{
    public class HouseDto
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public double PredictedPrice { get; set; }
        public Guid? UserId { get; set; }
    }
}
