using AutoMapper;
using EscapeRoomApi.Dtos;
using EscapeRoomApi.Models;

namespace EscapeRoomApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserProfile, UserDto>();
            CreateMap<UserProfile, LeaderboardDto>();
        }
    }
}
