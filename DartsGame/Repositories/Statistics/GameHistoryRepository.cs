using DartsGame.Data;
using DartsGame.DTO;
using DartsGame.DTOs;
using DartsGame.Interfaces.RepositoryInterfaces.Statistics;
using Microsoft.EntityFrameworkCore;


namespace DartsGame.Repositories.Statistics
{
    public class GameHistoryRepository : IGameHistoryRepository
    {
        private readonly AppDbContext _context;

        public GameHistoryRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<MatchStatsDTO>> GetMatchStatsByMatchId(Guid matchId)
        {
            var matchStats = await _context.MatchStats
                .Where(ms => ms.MatchId == matchId)
                .Include(ms => ms.Match)
                .Include(ms => ms.Player)  
                .Include(ms => ms.Match.Sets)
                     .ThenInclude(s => s.Legs)
                .ToListAsync();

            var matchStatsDTO = matchStats.Select(ms => new MatchStatsDTO
            {
                MatchId = ms.MatchId,
                PlayerName = ms.Player.Name,
                StartTime = ms.Match.StartTime,
                EndTime = ms.Match.EndTime,
                SetsWon = ms.SetsWin,
                LegsWon = ms.LegsWin,
                PPD = ms.PPD,
                First9PPD = ms.First9PPD,
                CheckoutPercentage = ms.CheckoutPercentage,
                BestCheckoutPoints = ms.BestCheckoutPoints,
                Count60Plus = ms.Count60Plus,
                Count100Plus = ms.Count100Plus,
                Count140Plus = ms.Count140Plus,
                Count180s = ms.Count180s,
                Sets = _context.SetStats
                    .Where(ss => ss.PlayerId == ms.PlayerId && ss.Set.MatchId == ms.MatchId)
                    .Select(ss => new SetStatsDTO
                    {
                        SetId = ss.SetId,
                        LegsWon = ss.LegsWin,
                        PPD = ss.PPD,
                        First9PPD = ss.First9PPD,
                        CheckoutPercentage = ss.CheckoutPercentage,
                        BestCheckoutPoints = ss.BestCheckoutPoints,
                        Count60Plus = ss.Count60Plus,
                        Count100Plus = ss.Count100Plus,
                        Count140Plus = ss.Count140Plus,
                        Count180s = ss.Count180s,
                        Legs = _context.LegStats
                            .Where(ls => ls.PlayerId == ss.PlayerId && ls.Leg.SetId == ss.SetId)
                            .Select(ls => new LegStatsDTO
                            {
                                LegId = ls.LegId,
                                DartsThrown = ls.DartsThrown,
                                PPD = ls.PPD,
                                First9PPD = ls.First9PPD,
                                CheckoutPoints = ls.CheckoutPoints,
                                CheckoutPercentage = ls.CheckoutPercentage,
                                Count60Plus = ls.Count60Plus,
                                Count100Plus = ls.Count100Plus,
                                Count140Plus = ls.Count140Plus,
                                Count180s = ls.Count180s,
                                TurnThrows = _context.TurnThrows
                                    .Where(tt => tt.Turn.LegId == ls.LegId && tt.Turn.PlayerId == ls.PlayerId)
                                    .Select(tt => new TurnThrowDTO
                                    {
                                        TurnThrowId = tt.TurnThrowId,
                                        TurnId = tt.TurnId,
                                        Throw1 = tt.Throw1.ToString(),
                                        Throw2 = tt.Throw2.ToString(),
                                        Throw3 = tt.Throw3.ToString(),
                                        Score = tt.Score
                                    }).ToList()
                           } ).ToList()
                    }).ToList()
            }).ToList();

            return matchStatsDTO;
        }
    }
}
