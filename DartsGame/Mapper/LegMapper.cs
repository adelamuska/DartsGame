using AutoMapper;
using DartsGame.DTO;
using DartsGame.Entities;

namespace DartsGame.Mapper
{
    public class LegMapper : Profile
    {
        public LegMapper() { 

            CreateMap<Leg, LegDTO>();
            CreateMap<LegDTO, Leg>();
        }
    }
}
