using AutoMapper;
using DartsGame.DTO;
using DartsGame.Entities;

namespace DartsGame.Mapper
{
    public class SetResultMapper : Profile
    {
        public SetResultMapper()
        {

            CreateMap<SetResult, SetResultDTO>();
            CreateMap<SetResultDTO, SetResult>();
        }
    }
}
