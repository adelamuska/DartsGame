using AutoMapper;
using DartsGame.Data;
using DartsGame.DTO;
using DartsGame.Entities;
using DartsGame.Helpers;
using DartsGame.Interfaces;
using DartsGame.Repositories;
using DartsGame.RequestDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DartsGame.Services
{
    public class TurnService : ITurnService
    {
        public readonly AppDbContext _context;
        private readonly GameFlowService _gameService;
        public readonly TurnRepository _turnRepository;
        public readonly PlayerRepository _playerRepository;
        public readonly LegRepository _legRepository;


        public readonly IMapper _mapper;
        public TurnService(AppDbContext context, GameFlowService gameService, TurnRepository turnRepository, PlayerRepository playerRepository, LegRepository legRepository, IMapper mapper)
        {
            _context = context;
            _gameService = gameService;
            _turnRepository = turnRepository;
            _playerRepository = playerRepository;
            _legRepository = legRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TurnDTO>> GetAll()
        {
            var turns = await _turnRepository.GetAll();
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
                throw new ArgumentNullException("Turn cannot be null.");
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
            if (turnDTO == null)
            {
                throw new ArgumentNullException("Turn cannot be null.");
            }

            var turnById = await _turnRepository.GetById(turnId);
            if (turnById == null)
            {
                throw new KeyNotFoundException($"Turn with ID {turnId} not found.");
            }

            var turnEntity = _mapper.Map(turnDTO, turnById);

            var updatedTurn = await _turnRepository.Update(turnEntity);

            return _mapper.Map<TurnDTO>(updatedTurn);
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

        public async Task ProcessTurn(Match match, TurnThrowRequestDTO turnThrows)
        {
            var activePlayersIds =await _playerRepository.GetActivePlayerIds(match.MatchId) ;

            var currentLeg = await _legRepository.GetCurrentLeg(match.Sets.Last().SetId);


            if (currentLeg == null)
            {
                throw new InvalidOperationException("No active leg found.");
            }
            if (currentLeg.IsFinished)
            {
                throw new InvalidOperationException("The current leg is already finished.");
            }

            // Nuk eshte se prish ndonje pune nqs rrijne null, se gjithsesi null do bohen
            // null ne logjiken e lojes dmth muri, toka, ose s'ka pas nevoje me e gjujt
            //if (turnThrows.Throw1 == null || turnThrows.Throw2 == null || turnThrows.Throw3 == null)
            //{
            //    throw new ArgumentException("All three throws are required to complete the turn.");
            //}


            var firstPlayerId = activePlayersIds.First();
            var currentTurn = await _turnRepository.GetCurrentTurn(currentLeg.LegId);


            if (currentTurn == null)
            {
                throw new InvalidOperationException("No active turn found.");
            }

            int? throw1Score = ScoreTableHelper.GetScore(turnThrows.Throw1);
            int? throw2Score = ScoreTableHelper.GetScore(turnThrows.Throw2);
            int? throw3Score = ScoreTableHelper.GetScore(turnThrows.Throw3);

            int totalScore = throw1Score.GetValueOrDefault() + throw2Score.GetValueOrDefault() + throw3Score.GetValueOrDefault();


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

            var lastThrow = _context.TurnThrows.OrderBy(t => t.TurnThrowId).Last();
            await _gameService.ProcessGameStateAfterTurn(currentLeg.LegId, totalScore, lastThrow.ToString());

            await _context.Entry(currentLeg).ReloadAsync();


            if (!currentLeg.IsFinished)
            {
                var currentPlayerId = currentTurn.PlayerId;
                var currentPlayerIndex = activePlayersIds.IndexOf(currentPlayerId);
                var nextPlayerId = activePlayersIds[(currentPlayerIndex + 1) % activePlayersIds.Count];

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