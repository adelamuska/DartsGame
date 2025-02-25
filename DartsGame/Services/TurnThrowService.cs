using AutoMapper;
using DartsGame.Data;
using DartsGame.DTO;
using DartsGame.Entities;
using DartsGame.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Services
{
    public class TurnThrowService
    {
        public readonly TurnThrowRepository _turnThrowRepository;
        public readonly IMapper _mapper;
        public TurnThrowService(TurnThrowRepository turnThrowRepository, IMapper mapper)
        {
            _turnThrowRepository = turnThrowRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TurnThrowDTO>> GetAll()
        {
            var turnsThrow = await _turnThrowRepository.GetAll();
            if (turnsThrow == null)
            {
                throw new ArgumentNullException(nameof(turnsThrow));

            }
            return _mapper.Map<IEnumerable<TurnThrowDTO>>(turnsThrow);
        }

        public async Task<TurnThrowDTO> GetById(Guid turnThrowId)
        {
            var turnThrow = await _turnThrowRepository.GetById(turnThrowId);
            if (turnThrow == null)
            {
                throw new KeyNotFoundException($"Turn throw with ID {turnThrowId} not found.");
            }
            return _mapper.Map<TurnThrowDTO>(turnThrow);
        }

        public async Task<TurnThrowDTO> AddTurnThrow(TurnThrowDTO turnThrowDTO)
        {
            if (turnThrowDTO == null)
            {
                throw new ArgumentNullException(nameof(turnThrowDTO), ("Turn throw cannot be null"));
            }

            var turnThrowEntity = _mapper.Map<TurnThrow>(turnThrowDTO);
            var addedTurnThrow = await _turnThrowRepository.Create(turnThrowEntity);

            if (addedTurnThrow == null)
            {
                throw new InvalidOperationException("Failed to add turn throw.");
            }

            return _mapper.Map<TurnThrowDTO>(addedTurnThrow);
        }

        public async Task<TurnThrowDTO> UpdateTurnThrow(Guid turnThrowId, TurnThrowDTO turnThrowDTO)
        {
            {
                if (turnThrowDTO == null)
                {
                    throw new ArgumentNullException(nameof(turnThrowDTO), "Turn throw cannot be null.");
                }

                var turnThrowById = await _turnThrowRepository.GetById(turnThrowId);

                if (turnThrowById == null)
                {
                    throw new KeyNotFoundException($"Turn throw with ID {turnThrowId} not found.");
                }

                var turnThrowEntity = _mapper.Map(turnThrowDTO, turnThrowById);
                turnThrowEntity.TurnThrowId = turnThrowId;

                var updatedTurnThrow = await _turnThrowRepository.Update(turnThrowEntity);

                if (updatedTurnThrow == null)
                {
                    throw new KeyNotFoundException($"Turn throw with ID {turnThrowDTO.TurnThrowId} not found for update.");
                }

                return _mapper.Map<TurnThrowDTO>(updatedTurnThrow);
            }

        }

        public async Task DeleteTurnThrow(Guid turnThrowId)
        {

            var turnThrow = await _turnThrowRepository.GetById(turnThrowId);

            if (turnThrow == null)
            {
                throw new KeyNotFoundException($"Turn throw with ID {turnThrowId} not found.");
            }

            await _turnThrowRepository.Delete(turnThrowId);
        }
    }
}
