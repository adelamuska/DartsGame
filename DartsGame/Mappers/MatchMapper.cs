using AutoMapper;
using DartsGame.DTO;
using DartsGame.DTOs;
using DartsGame.Entities;

namespace DartsGame.Mapper
{
    public class MatchMapper : Profile
    {
        public MatchMapper()
        {

            CreateMap<Match, MatchDTO>();
            CreateMap<MatchDTO, Match>();
            CreateMap<Match, MatchWithStatsDTO>()
             .ForMember(dest => dest.Players, opt => opt.Ignore());

        }
    }
}
