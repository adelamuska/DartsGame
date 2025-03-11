using AutoMapper;
using DartsGame.DTO;
using DartsGame.Entities;

namespace DartsGame.Mapper
{
    public class MatchMapper : Profile
    {
        public MatchMapper()
        {

            CreateMap<Match, MatchDTO>();
            CreateMap<MatchDTO, Match>();
        }
    }
}
