using AutoMapper;
using DartsGame.DTO;
using DartsGame.Entities;

namespace DartsGame.Mapper
{
    public class SetMapper : Profile
    {
        public SetMapper()
        {

            CreateMap<Set, SetDTO>();
            CreateMap<SetDTO, Set>();
        }
    }
}
