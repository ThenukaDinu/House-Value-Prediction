namespace Models
{
    public class User : CommonModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDay { get; set; }
        public string AvatarLink { get; set; }
        public ICollection<Interest> Interests { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}
