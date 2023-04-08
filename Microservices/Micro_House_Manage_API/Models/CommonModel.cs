namespace Micro_House_Manage_API.Models
{
    public class CommonModel
    {
        private DateTime? _createdDate;
        public DateTime? CreatedDate 
        {
            get => _createdDate;
            set => _createdDate = value ?? DateTime.UtcNow;
        }
        public DateTime? ModifiedDate { get; set; }
    }
}
