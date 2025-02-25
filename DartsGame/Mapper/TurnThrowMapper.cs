using AutoMapper;
using DartsGame.DTO;
using DartsGame.Entities;

namespace DartsGame.Mapper
{
    public class TurnThrowMapper : Profile
    {
        public TurnThrowMapper()
        {

            CreateMap<TurnThrow, TurnThrowDTO>();
            CreateMap<TurnThrowDTO, TurnThrow>();
        }
    }
}
