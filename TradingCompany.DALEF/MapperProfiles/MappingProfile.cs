using AutoMapper;
using TradingCompany.DALEF.Models;
using TradingCompany.DTO;

namespace TradingCompany.DALEF.MapperProfiles
{
    public class MappingProfile :Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserModels, UserDTO>().ReverseMap();
        }

    }
}
