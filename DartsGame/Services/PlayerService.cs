using AutoMapper;
using DartsGame.DTO;
using DartsGame.Entities;
using DartsGame.Repositories;

namespace DartsGame.Services
{
    public class PlayerService
    {
        public readonly PlayerRepository _playerRepository;
        public readonly IMapper _mapper;

        public PlayerService(PlayerRepository playerRepository, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlayerDTO>> GetAll()
        {
            var players = await _playerRepository.GetAll();
            if (players == null)
            {
                throw new ArgumentNullException(nameof(players));

            }
            return _mapper.Map<IEnumerable<PlayerDTO>>(players);
        }

        public async Task<PlayerDTO> GetById(Guid playerId)
        {
            var player = await _playerRepository.GetById(playerId);
            if (player == null)
            {
                throw new KeyNotFoundException($"Player with ID {playerId} not found.");
            }
            return _mapper.Map<PlayerDTO>(player);
        }

        public async Task<PlayerDTO> AddPlayer(PlayerDTO playerDTO)
        {
            if (playerDTO == null)
            {
                throw new ArgumentNullException(nameof(playerDTO), ("Player cannot be null"));
            }

            var playerEntity = _mapper.Map<Player>(playerDTO);
            var addedPlayer = await _playerRepository.Create(playerEntity);

            if (addedPlayer == null)
            {
                throw new InvalidOperationException("Failed to add player.");
            }

            return _mapper.Map<PlayerDTO>(addedPlayer);
        }

        public async Task<PlayerDTO> UpdatePlayer(Guid playerId, PlayerDTO playerDTO)
        {
            {
                if (playerDTO == null)
                {
                    throw new ArgumentNullException(nameof(playerDTO), "Player cannot be null.");
                }

                var playerById = await _playerRepository.GetById(playerId);

                if (playerById == null)
                {
                    throw new KeyNotFoundException($"Player with ID {playerById} not found.");
                }

                var playerEntity = _mapper.Map(playerDTO, playerById);
                playerEntity.PlayerId = playerId;

                var updatedPlayer = await _playerRepository.Update(playerEntity);

                if (updatedPlayer == null)
                {
                    throw new KeyNotFoundException($"Player with ID {playerDTO.PlayerId} not found for update.");
                }

                return _mapper.Map<PlayerDTO>(updatedPlayer);
            }

        }

        public async Task DeletePlayer(Guid playerId)
        {

            var player = await _playerRepository.GetById(playerId);

            if (player == null)
            {
                throw new KeyNotFoundException($"Player with ID {playerId} not found.");
            }

             await _playerRepository.Delete(playerId);
        }
    }
}

