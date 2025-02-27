using AutoMapper;
using DartsGame.Data;
using DartsGame.DTO;
using DartsGame.Entities;
using DartsGame.Enums;
using DartsGame.Interfaces;
using DartsGame.Repositories;

namespace DartsGame.Services
{
    public class MatchService : IMatchService
    {
        private readonly AppDbContext _context;
        public readonly MatchRepository _matchRepository;
        public readonly IMapper _mapper;

        public MatchService(AppDbContext context, MatchRepository matchRepository, IMapper mapper)
        {
            _context = context;
            _matchRepository = matchRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MatchDTO>> GetAll()
        {
            var matches = await _matchRepository.GetAll();
            if (matches == null)
            {
                throw new ArgumentNullException(nameof(matches));

            }
            return _mapper.Map<IEnumerable<MatchDTO>>(matches);
        }

        public async Task<MatchDTO> GetById(Guid matchId)
        {
            var match = await _matchRepository.GetById(matchId);
            if (match == null)
            {
                throw new KeyNotFoundException($"Match with ID {matchId} not found.");
            }
            return _mapper.Map<MatchDTO>(match);
        }

        public async Task<MatchDTO> AddMatch(MatchDTO matchDTO)
        {
            if (matchDTO == null)
            {
                throw new ArgumentNullException(nameof(matchDTO), ("Match cannot be null"));
            }

            var matchEntity = _mapper.Map<Match>(matchDTO);
            var addedMatch = await _matchRepository.Create(matchEntity);

            if (addedMatch == null)
            {
                throw new InvalidOperationException("Failed to add match.");
            }

            return _mapper.Map<MatchDTO>(addedMatch);
        }

        public async Task<MatchDTO> UpdateMatch(Guid matchId, MatchDTO matchDTO)
        {
            {
                if (matchDTO == null)
                {
                    throw new ArgumentNullException(nameof(matchDTO), "Match cannot be null.");
                }

                var matchById = await _matchRepository.GetById(matchId);

                if (matchById == null)
                {
                    throw new KeyNotFoundException($"Match with ID {matchId} not found.");
                }

                var matchEntity = _mapper.Map(matchDTO, matchById);
                matchEntity.MatchId = matchId;

                var updatedMatch = await _matchRepository.Update(matchEntity);

                if (updatedMatch == null)
                {
                    throw new KeyNotFoundException($"Match with ID {matchDTO.MatchId} not found for update.");
                }

                return _mapper.Map<MatchDTO>(updatedMatch);
            }

        }

        public async Task DeleteMatch(Guid matchId)
        {

            var match = await _matchRepository.GetById(matchId);

            if (match == null)
            {
                throw new KeyNotFoundException($"Match with ID {matchId} not found.");
            }

            await _matchRepository.Delete(matchId);
        }



        public async Task<Match> StartMatch(int score, int sets, int legs, int numberOfPlayers, List<string> playerNames)
        {
            if (!Enum.IsDefined(typeof(StartingScore), score))
            {
                throw new ArgumentException($"Invalid starting score {score}");
            }
            var startingScore = (StartingScore)score;

            if (!Enum.IsDefined(typeof(BestOfSets), sets))
            {
                throw new ArgumentException($"Invalid number of sets {sets}");
            }
            var numberOfSets = (BestOfSets)sets;

            if(!Enum.IsDefined(typeof(BestOfLegs), legs))
            {
                throw new ArgumentException($"Invalid number of sets {legs}");
            }
            var numberOfLegs = (BestOfLegs)legs;

            if (numberOfPlayers > 6 || numberOfPlayers < 1)
            {
                throw new ArgumentException("Number of players should be between 1 and 6.");
            }

            var matchId = Guid.NewGuid();
            var matchPlayers = new Dictionary<Guid, string>();

            foreach (var playerName in playerNames)
            {
                var existingPlayer = _context.Players.FirstOrDefault(p => p.Name == playerName);
                Guid playerId;

                if (existingPlayer == null)
                {
                    playerId = Guid.NewGuid();
                    var newPlayer = new Player(playerId, playerName);
                    _context.Players.Add(newPlayer);
                }
                else
                {
                    playerId = existingPlayer.PlayerId;
                }

                matchPlayers[playerId] = playerName;
            }

            var matchDTO = new MatchDTO(
                   matchId,
                   DateTime.UtcNow,
                   null,
                   numberOfSets,
                   startingScore,
                   false
               );

            var match = await AddMatch(matchDTO);

            
            foreach (var playerId in matchPlayers.Keys)
            {
                var playerMatch = new PlayerMatch()
                {
                    MatchId = matchId,
                    PlayerId = playerId,
                };

                _context.PlayerMatches.Add(playerMatch);
            }

            await _context.SaveChangesAsync();

            
            var newSet = new Set(Guid.NewGuid())
            {
                MatchId = matchId,
                SetNumber = 1,
                BestOfLegs = numberOfLegs, 
                IsFinished = false
            };

            _context.Sets.Add(newSet);
            await _context.SaveChangesAsync();

            var firstLeg = new Leg(
                Guid.NewGuid(),
                newSet.SetId,
                1,
                false
            );

            _context.Legs.Add(firstLeg);

            foreach (var playerId in matchPlayers.Keys)
            {
                var legScore = new LegScore
                {
                    LegId = firstLeg.LegId,
                    PlayerId = playerId,
                    RemainingScore = (int)startingScore
                };

                _context.LegScores.Add(legScore);
            }

            var initialTurn = new Turn(
                Guid.NewGuid(),
                matchPlayers.Keys.First(),
                firstLeg.LegId,
                DateTime.UtcNow,
                false,
                false,
                false
            );

            _context.Turns.Add(initialTurn);
            await _context.SaveChangesAsync();

            return _mapper.Map<Match>(match);
        }
    }
}
