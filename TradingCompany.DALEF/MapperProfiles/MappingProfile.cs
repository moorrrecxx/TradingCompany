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
            CreateMap<RoleModels, RoleDTO>().ReverseMap();
            CreateMap<LogModels, LogDTO>().ReverseMap();
            CreateMap<OrderModels, OrderDTO>().ReverseMap();
            CreateMap<ShipmentModels, ShipmentDTO>().ReverseMap();
            CreateMap<StatusModels, StatusDTO>().ReverseMap();
            CreateMap<UserRoleModels, UserRoleDTO>().ReverseMap();

        }

    }
}
