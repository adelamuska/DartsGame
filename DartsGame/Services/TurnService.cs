using AutoMapper;
using DartsGame.Data;
using DartsGame.DTO;
using DartsGame.Entities;
using DartsGame.Helpers;
using DartsGame.Interfaces;
using DartsGame.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Services
{
    public class TurnService : ITurnService
    {
        public readonly AppDbContext _context;
        private readonly GameService _gameService;
        public readonly TurnRepository _turnRepository;
        public readonly IMapper _mapper;
        public TurnService(AppDbContext context, GameService gameService, TurnRepository turnRepository, IMapper mapper)
        {
            _context = context;
            _gameService = gameService;
            _turnRepository = turnRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TurnDTO>> GetAll()
        {
            var turns = await _turnRepository.GetAll();
            if (turns == null)
            {
                throw new ArgumentNullException(nameof(turns));

            }
            return _mapper.Map<IEnumerable<TurnDTO>>(turns);
        }

        public async Task<TurnDTO> GetById(Guid turnId)
        {
            var turn = await _turnRepository.GetById(turnId);
            if (turn == null)
            {
                throw new KeyNotFoundException($"Turn with ID {turnId} not found.");
            }
            return _mapper.Map<TurnDTO>(turn);
        }

        public async Task<TurnDTO> AddTurn(TurnDTO turnDTO)
        {
            if (turnDTO == null)
            {
                throw new ArgumentNullException(nameof(turnDTO), ("Turn cannot be null"));
            }

            var turnEntity = _mapper.Map<Turn>(turnDTO);
            var addedTurn = await _turnRepository.Create(turnEntity);

            if (addedTurn == null)
            {
                throw new InvalidOperationException("Failed to add turn.");
            }

            return _mapper.Map<TurnDTO>(addedTurn);
        }

        public async Task<TurnDTO> UpdateTurn(Guid turnId, TurnDTO turnDTO)
        {
            {
                if (turnDTO == null)
                {
                    throw new ArgumentNullException(nameof(turnDTO), "Turn cannot be null.");
                }

                var turnById = await _turnRepository.GetById(turnId);

                if (turnById == null)
                {
                    throw new KeyNotFoundException($"Turn with ID {turnById} not found.");
                }

                var turnEntity = _mapper.Map(turnDTO, turnById);
                turnEntity.TurnId = turnId;

                var updatedTurn = await _turnRepository.Update(turnEntity);

                if (updatedTurn == null)
                {
                    throw new KeyNotFoundException($"Turn with ID {turnDTO.TurnId} not found for update.");
                }

                return _mapper.Map<TurnDTO>(updatedTurn);
            }

        }

        public async Task DeleteTurn(Guid turnId)
        {

            var turn = await _turnRepository.GetById(turnId);

            if (turn == null)
            {
                throw new KeyNotFoundException($"Turn with ID {turnId} not found.");
            }

            await _turnRepository.Delete(turnId);
        }




        public async Task ChangeTurn(Match match, TurnThrowRequestDTO turnThrows)
        {
            var activePlayers = await _context.PlayerMatches
                .Where(pm => pm.MatchId == match.MatchId)
                .Select(pm => pm.PlayerId)
                .ToListAsync();

            var currentLeg = await _context.Legs
                .Where(l => l.SetId == match.Sets.Last().SetId && !l.IsFinished)
                .OrderByDescending(l => l.LegNumber)
                .FirstOrDefaultAsync();


            if (currentLeg == null)
            {
                throw new InvalidOperationException("No active leg found.");
            }
            if (currentLeg.IsFinished)
            {
                throw new InvalidOperationException("The current leg is already finished.");
            }

            if (turnThrows.Throw1 == null || turnThrows.Throw2 == null || turnThrows.Throw3 == null)
            {
                throw new ArgumentException("All three throws are required to complete the turn.");
            }

            var firstTurn = activePlayers.First(); 
            var currentTurn = await _context.Turns
                .Where(t => t.LegId == currentLeg.LegId && !t.IsDeleted)
                .OrderByDescending(t => t.TimeStamp)
                .FirstOrDefaultAsync();

            if (currentTurn == null)
            {
                throw new InvalidOperationException("No active turn found.");
            }

            int throw1Score = ScoreTableHelper.GetScore(turnThrows.Throw1);
            int throw2Score = ScoreTableHelper.GetScore(turnThrows.Throw2);
            int throw3Score = ScoreTableHelper.GetScore(turnThrows.Throw3);
            int totalScore = throw1Score + throw2Score + throw3Score;

            var turnThrow = new TurnThrow
            {
                TurnThrowId = Guid.NewGuid(),
                TurnId = currentTurn.TurnId,  
                Throw1 = throw1Score,
                Throw2 = throw2Score,
                Throw3 = throw3Score,
                Score = totalScore,
                IsDeleted = false
            };

            _context.TurnThrows.Add(turnThrow);
            await _context.SaveChangesAsync();

            await _gameService.ProcessGameStateAfterTurn(currentLeg.LegId, totalScore, turnThrows.Throw3.ToString());

            await _context.Entry(currentLeg).ReloadAsync();


            if (!currentLeg.IsFinished)
            {
                var currentPlayerId = currentTurn.PlayerId;
                var currentPlayerIndex = activePlayers.IndexOf(currentPlayerId);
                var nextPlayerId = activePlayers[(currentPlayerIndex + 1) % activePlayers.Count];

                var newTurn = new Turn(
                    Guid.NewGuid(),
                    nextPlayerId,
                    currentLeg.LegId,
                    DateTime.UtcNow,
                    false,
                    false,
                    false
                );

                _context.Turns.Add(newTurn);
                await _context.SaveChangesAsync();
            }
        }


    }
}
