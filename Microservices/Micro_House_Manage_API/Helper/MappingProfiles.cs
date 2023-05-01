using AutoMapper;
using Micro_House_Manage_API.Dtos;
using Models;
using Models.Others;

namespace Micro_House_Manage_API.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<House, HouseDto>();
            CreateMap<HouseDto, House>();
            CreateMap<Inquiry, InquiryDto>();
            CreateMap<InquiryDto, Inquiry>();
            CreateMap<Listing, ListingDto>();
            CreateMap<ListingDto, Listing>();
            CreateMap<Interest, InterestDto>();
            CreateMap<InterestDto, Interest>();
            CreateMap<UserInfo, UserDto>();
            CreateMap<UserDto, UserInfo>();
        }
    }
}
