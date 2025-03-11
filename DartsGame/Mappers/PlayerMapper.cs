using AutoMapper;
using DartsGame.DTO;
using DartsGame.Entities;

namespace DartsGame.Mapper
{
    public class PlayerMapper : Profile
    {
        public PlayerMapper()
        {

            CreateMap<Player, PlayerDTO>();
            CreateMap<PlayerDTO, Player>();
        }
    }
}
