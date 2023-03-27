namespace Micro_House_Manage_API.Models
{
    public class User : CommonModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDay { get; set; }
        public string AvatarLink { get; set; }
        public ICollection<Interest> Interests { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}
