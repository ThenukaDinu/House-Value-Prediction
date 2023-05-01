using Models.Others;

namespace Micro_House_Manage_API.Dtos
{
    public class UserDto : UserInfo
    {
        public List<InterestDto> UserInterests { get; set; }
    }
}
