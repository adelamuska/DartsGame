using DartsGame.Entities;
using DartsGame.Repositories;
using DartsGame.RequestDTOs;
using DartsGame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DartsGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameFlowController : ControllerBase
    {

        private readonly MatchService _matchService;
        private readonly TurnService _turnService;
        private readonly MatchRepository _matchRepository;

        public GameFlowController(MatchService matchService, TurnService turnService, MatchRepository matchRepository)
        {
            _matchService = matchService;
            _turnService = turnService;
            _matchRepository = matchRepository;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartMatch([FromBody] StartMatchRequest request)
        {

            if (request == null || request.PlayerNames == null || request.PlayerNames.Count == 0)
            {
                return BadRequest("Player names are required.");
            }

            var match = await _matchService.StartMatch(
                request.StartingScore,
                request.BestOfSets,
                request.BestOfLegs,
                request.PlayerNames.Count,
                request.PlayerNames);

            return Ok(new
            {
                match.MatchId,
                Message = "Game started successfully."
            });

        }

        //[HttpGet("match/{matchId}")]
        //public async Task<IActionResult> GetMatch(Guid matchId)
        //{

        //    var match = await _matchService.GetById(matchId);
        //    return Ok(match);

        //}

        [HttpPost("throw")]
        public async Task<IActionResult> ProcessTurn([FromBody] ProcessTurnRequest request)
        {

            var match = await _matchRepository.GetById(request.MatchId);
            if (match == null)
            {
                return NotFound($"Match with ID {request.MatchId} not found.");
            }

            if (match.IsFinished)
            {
                return BadRequest("This match is already finished.");
            }


            await _turnService.ProcessTurn(match, request.TurnThrows);

            var currentSet = match.Sets?.OrderByDescending(s => s.SetNumber)
       .FirstOrDefault(s => !s.IsFinished);


            var currentLeg = currentSet?.Legs?.OrderByDescending(l => l.LegNumber)
       .FirstOrDefault(l => !l.IsFinished);

            var currentTurn = currentLeg.Turns?.OrderByDescending(t => t.TimeStamp)
        .FirstOrDefault(t => !t.IsDeleted);
  

            var playerScore = currentLeg.LegScores
        .FirstOrDefault(ls => ls.PlayerId == currentTurn.PlayerId);

            return Ok(new
            {
                Message = "Turn processed successfully.",
                RemainingScore = playerScore?.RemainingScore ?? 0
            });
        }

        //[HttpGet("currentstate/{matchId}")]
        //public async Task<IActionResult> GetGameState(Guid matchId)
        //{

        //    var match = await _matchService.GetById(matchId);
        //    if (match == null)
        //    {
        //        return NotFound($"Match with ID {matchId} not found.");
        //    }


        //    var currentSet = match.Sets?.OrderByDescending(s => s.SetNumber)
        //        .FirstOrDefault(s => !s.IsFinished);


        //    var currentLeg = currentSet?.Legs?.OrderByDescending(l => l.LegNumber)
        //        .FirstOrDefault(l => !l.IsFinished);

        //    if (currentLeg == null)
        //    {
        //        return Ok(new
        //        {
        //            match.MatchId,
        //            match.IsFinished,
        //            match.WinnerPlayerId,
        //            Status = "Match completed"
        //        });
        //    }

        //    var currentTurn = currentLeg.Turns?.OrderByDescending(t => t.TimeStamp)
        //        .FirstOrDefault(t => !t.IsDeleted);

        //    var playerScores = currentLeg.LegScores;

        //    return Ok(new
        //    {
        //        match.MatchId,
        //        CurrentSetNumber = currentSet.SetNumber,
        //        CurrentLegNumber = currentLeg.LegNumber,
        //        CurrentPlayerId = currentTurn?.PlayerId,
        //        PlayerScores = playerScores,
        //        SetScores = match.Sets.SelectMany(s => s.SetResults)
        //            .GroupBy(sr => sr.PlayerId)
        //            .Select(g => new
        //            {
        //                PlayerId = g.Key,
        //                SetsWon = g.Count(sr => sr.SetId == sr.Set.SetId && sr.Set.WinnerPlayerId == g.Key)
        //            })
        //    });

        //}

    }
}

