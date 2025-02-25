using AutoMapper;
using DartsGame.DTO;
using DartsGame.Entities;

namespace DartsGame.Mapper
{
    public class LegScore : Profile
    {
        public LegScore()
        {

            CreateMap<LegScore, LegScoreDTO>();
            CreateMap<LegScoreDTO, LegScore>();
        }
    }
}
