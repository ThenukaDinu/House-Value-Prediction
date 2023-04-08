using AutoMapper;
using Micro_House_Manage_API.Dtos;
using Micro_House_Manage_API.Models;

namespace Micro_House_Manage_API.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<House, HouseDto>();
            CreateMap<Inquiry, inquiryDto>();
            CreateMap<Listing, ListingDto>();
        }
    }
}
