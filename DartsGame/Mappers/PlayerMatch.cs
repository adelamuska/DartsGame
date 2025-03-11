using AutoMapper;
using DartsGame.DTO;
using DartsGame.Entities;

namespace DartsGame.Mapper
{
    public class PlayerMatch : Profile
    {
        public PlayerMatch()
        {

            CreateMap<PlayerMatch, PlayerMatchDTO>();
            CreateMap<PlayerMatchDTO, PlayerMatch>();
        }
    }
}
