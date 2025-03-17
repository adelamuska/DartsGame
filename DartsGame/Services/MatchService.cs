using AutoMapper;
using DartsGame.Data;
using DartsGame.DTO;
using DartsGame.DTOs;
using DartsGame.Entities;
using DartsGame.Enums;
using DartsGame.Interfaces.ServiceInterfaces;
using DartsGame.Repositories;
using DartsGame.Services.Statistics;

namespace DartsGame.Services
{
    public class MatchService : IMatchService
    {
        private readonly AppDbContext _context;
        public readonly MatchRepository _matchRepository;
        public readonly IMapper _mapper;
        private readonly MatchStatsService _matchStatsService;

        public MatchService(AppDbContext context, MatchRepository matchRepository, IMapper mapper, MatchStatsService matchStatsService)
        {
            _context = context;
            _matchRepository = matchRepository;
            _mapper = mapper;
            _matchStatsService = matchStatsService;
        }

        //public async Task<IEnumerable<MatchDTO>> GetAll()
        //{
        //    var matches = await _matchRepository.GetAll();
        //    return _mapper.Map<IEnumerable<MatchDTO>>(matches);
        //}

        public async Task<IEnumerable<MatchWithStatsDTO>> GetAllWithStats()
        {
            var matches = await _matchRepository.GetAll(); 
            var result = new List<MatchWithStatsDTO>();

            foreach (var match in matches)
            {
                var matchDto = _mapper.Map<MatchWithStatsDTO>(match);

                var playerIds = await _matchRepository.GetMatchPlayerIds(match.MatchId);

                matchDto.Players = new List<PlayerMatchStatsDTO>();

                foreach (var playerId in playerIds)
                {
                    var player = await _context.Players.FindAsync(playerId);

                    if (player == null) continue;

                    var setsWon = await _matchStatsService.CalculateSetsWon(match.MatchId, playerId);

                    matchDto.Players.Add(new PlayerMatchStatsDTO
                    {
                        PlayerId = playerId,
                        PlayerName = player.Name,
                        SetsWon = setsWon
                    });
                }

                result.Add(matchDto);
            }

            return result;
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

        //public async Task<MatchDTO> AddMatch(MatchDTO matchDTO)
        //{
        //    if (matchDTO == null)
        //    {
        //        throw new ArgumentNullException("Match cannot be null.");
        //    }

        //    var matchEntity = _mapper.Map<Match>(matchDTO);

        //    var addedMatch = await _matchRepository.Create(matchEntity);

        //    if (addedMatch == null)
        //    {
        //        throw new InvalidOperationException("Failed to add match.");
        //    }

        //    return _mapper.Map<MatchDTO>(addedMatch);
        //}

       // public async Task<MatchDTO> UpdateMatch(Guid matchId, MatchDTO matchDTO)
        //{
        //    if (matchDTO == null)
        //    {
        //        throw new ArgumentNullException("Match cannot be null.");
        //    }

        //    var matchById = await _matchRepository.GetById(matchId);
        //    if (matchById == null)
        //    {
        //        throw new KeyNotFoundException($"Match with ID {matchId} not found.");
        //    }

        //    var matchEntity = _mapper.Map(matchDTO, matchById);

        //    var updatedMatch = await _matchRepository.Update(matchEntity);

        //    return _mapper.Map<MatchDTO>(updatedMatch);
        //}

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
            ValidateInputs(score, sets, legs, numberOfPlayers);

            StartingScore startingScore = (StartingScore)score;
            BestOfSets numberOfSets = (BestOfSets)sets;
            BestOfLegs numberOfLegs = (BestOfLegs)legs;

            var matchId = Guid.NewGuid();
            var matchPlayers = await AddPlayers(playerNames);

            var match = await CreateMatch(matchId, startingScore, numberOfSets);

            await AddPlayerMatches(matchId, matchPlayers);

            var newSet = await CreateSet(matchId, numberOfLegs);

            var firstLeg = await CreateLeg(newSet.SetId);

            await AddLegScores(firstLeg, matchPlayers, (int)startingScore);
            await AddSetResults(firstLeg.SetId, matchPlayers);

            var initialTurn = await CreateInitialTurn(firstLeg, matchPlayers);

            return _mapper.Map<Match>(match);
        }

        private void ValidateInputs(int score, int sets, int legs, int numberOfPlayers)
        {
            if (!Enum.IsDefined(typeof(StartingScore), score))
                throw new ArgumentException($"Invalid starting score {score}");

            if (!Enum.IsDefined(typeof(BestOfSets), sets))
                throw new ArgumentException($"Invalid number of sets {sets}");

            if (!Enum.IsDefined(typeof(BestOfLegs), legs))
                throw new ArgumentException($"Invalid number of sets {legs}");

            if (numberOfPlayers > 6 || numberOfPlayers < 1)
                throw new ArgumentException("Number of players should be between 1 and 6.");
        }

        private async Task<Dictionary<Guid, string>> AddPlayers(List<string> playerNames)
        {
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

            await _context.SaveChangesAsync();
            return matchPlayers;
        }

        private async Task<Match> CreateMatch(Guid matchId, StartingScore startingScore, BestOfSets numberOfSets)
        {
            var match = new Match(
                matchId,
                DateTime.UtcNow,
                null,
                numberOfSets,
                startingScore,
                false
            );

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
            return match;
        }

        private async Task AddPlayerMatches(Guid matchId, Dictionary<Guid, string> matchPlayers)
        {
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
        }

        private async Task<Set> CreateSet(Guid matchId, BestOfLegs numberOfLegs)
        {
            var newSet = new Set()
            {
                SetId = Guid.NewGuid(),
                MatchId = matchId,
                SetNumber = 1,
                BestOfLegs = numberOfLegs,
                IsFinished = false
            };

            _context.Sets.Add(newSet);
            await _context.SaveChangesAsync();

            return newSet;
        }

        private async Task<Leg> CreateLeg(Guid setId)
        {
            var firstLeg = new Leg(Guid.NewGuid(), setId, 1, false);
            _context.Legs.Add(firstLeg);
            await _context.SaveChangesAsync();

            return firstLeg;
        }

        private async Task AddLegScores(Leg firstLeg, Dictionary<Guid, string> matchPlayers, int startingScore)
        {
            foreach (var playerId in matchPlayers.Keys)
            {
                var legScore = new LegScore
                {
                    LegId = firstLeg.LegId,
                    PlayerId = playerId,
                    RemainingScore = startingScore
                };

                _context.LegScores.Add(legScore);
            }

            await _context.SaveChangesAsync();
        }

        private async Task AddSetResults(Guid setId, Dictionary<Guid, string> matchPlayers)
        {
            foreach (var playerId in matchPlayers.Keys)
            {
                var setResult = new SetResult
                {
                    SetId = setId,
                    PlayerId = playerId,
                    LegsWon = 0
                };

                _context.SetResults.Add(setResult);
            }

            await _context.SaveChangesAsync();
        }

        private async Task<Turn> CreateInitialTurn(Leg firstLeg, Dictionary<Guid, string> matchPlayers)
        {
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

            return initialTurn;
        }
    }
}
