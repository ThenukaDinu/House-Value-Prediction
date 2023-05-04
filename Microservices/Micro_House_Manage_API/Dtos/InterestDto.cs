namespace Micro_House_Manage_API.Dtos
{
    public class InterestDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public Guid UserId { get; set; }
    }
}
