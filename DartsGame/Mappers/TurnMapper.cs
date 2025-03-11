using AutoMapper;
using DartsGame.DTO;
using DartsGame.Entities;

namespace DartsGame.Mapper
{
    public class TurnMapper : Profile
    {
        public TurnMapper()
        {

            CreateMap<Turn, TurnDTO>();
            CreateMap<TurnDTO, Turn>();
        }
    }
}
