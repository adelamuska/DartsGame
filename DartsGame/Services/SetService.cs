using AutoMapper;
using DartsGame.DTO;
using DartsGame.Entities;
using DartsGame.Repositories;

namespace DartsGame.Services
{
    public class SetService
    {
        public readonly SetRepository _setRepository;
        public readonly IMapper _mapper;

        public SetService(SetRepository setRepository, IMapper mapper)
        {
            _setRepository = setRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SetDTO>> GetAll()
        {
            var sets = await _setRepository.GetAll();
            return _mapper.Map<IEnumerable<SetDTO>>(sets);
        }

        public async Task<SetDTO> GetById(Guid setId)
        {
            var set = await _setRepository.GetById(setId);
            if (set == null)
            {
                throw new KeyNotFoundException($"Set with ID {setId} not found.");
            }
            return _mapper.Map<SetDTO>(set);
        }

        public async Task<SetDTO> AddSet(SetDTO setDTO)
        {
            if (setDTO == null)
            {
                throw new ArgumentNullException("Set cannot be null.");
            }

            var setEntity = _mapper.Map<Set>(setDTO);
            var addedSet = await _setRepository.Create(setEntity);

            if (addedSet == null)
            {
                throw new InvalidOperationException("Failed to add set.");
            }

            return _mapper.Map<SetDTO>(addedSet);
        }

        public async Task<SetDTO> UpdateSet(Guid setId, SetDTO setDTO)
        {
            if (setDTO == null)
            {
                throw new ArgumentNullException("Set cannot be null.");
            }

            var setById = await _setRepository.GetById(setId);
            if (setById == null)
            {
                throw new KeyNotFoundException($"Set with ID {setId} not found.");
            }

            var setEntity = _mapper.Map(setDTO, setById);
            setEntity.SetId = setId;

            var updatedSet = await _setRepository.Update(setEntity);

            return _mapper.Map<SetDTO>(updatedSet);
        }

        public async Task DeleteSet(Guid setId)
        {
            var set = await _setRepository.GetById(setId);
            if (set == null)
            {
                throw new KeyNotFoundException($"Set with ID {setId} not found.");
            }

            await _setRepository.Delete(setId);
        }


    }
}

