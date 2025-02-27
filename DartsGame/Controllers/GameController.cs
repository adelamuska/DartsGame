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
    public class GameController : ControllerBase
    {

        private readonly MatchService _matchService;
        private readonly TurnService _turnService;
        private readonly MatchRepository _matchRepository;

        public GameController(MatchService matchService,TurnService turnService, MatchRepository matchRepository)
        {
            _matchService = matchService;
            _turnService = turnService;
            _matchRepository = matchRepository;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartMatch([FromBody] StartMatchRequest request)
        {
            try
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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("match/{matchId}")]
        public async Task<IActionResult> GetMatch(Guid matchId)
        {
            try
            {
                var match = await _matchService.GetById(matchId);
                return Ok(match);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("throw")]
        public async Task<IActionResult> ProcessTurn([FromBody] ProcessTurnRequest request)
        {
            try
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
           

                await _turnService.ChangeTurn(match, request.TurnThrows);

                return Ok(new { Message = "Turn processed successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("currentstate/{matchId}")]
        public async Task<IActionResult> GetGameState(Guid matchId)
        {
            try
            {
                var match = await _matchService.GetById(matchId);
                if (match == null)
                {
                    return NotFound($"Match with ID {matchId} not found.");
                }

                
                var currentSet = match.Sets?.OrderByDescending(s => s.SetNumber)
                    .FirstOrDefault(s => !s.IsFinished);

                
                var currentLeg = currentSet?.Legs?.OrderByDescending(l => l.LegNumber)
                    .FirstOrDefault(l => !l.IsFinished);

                if (currentLeg == null)
                {
                    return Ok(new
                    {
                         match.MatchId,
                         match.IsFinished,
                         match.WinnerPlayerId,
                        Status = "Match completed"
                    });
                }

                var currentTurn = currentLeg.Turns?.OrderByDescending(t => t.TimeStamp)
                    .FirstOrDefault(t => !t.IsDeleted);

                var playerScores = currentLeg.LegScores;

                return Ok(new
                {
                    match.MatchId,
                    CurrentSetNumber = currentSet.SetNumber,
                    CurrentLegNumber = currentLeg.LegNumber,
                    CurrentPlayerId = currentTurn?.PlayerId,
                    PlayerScores = playerScores,
                    SetScores = match.Sets.SelectMany(s => s.SetResults)
                        .GroupBy(sr => sr.PlayerId)
                        .Select(g => new {
                            PlayerId = g.Key,
                            SetsWon = g.Count(sr => sr.SetId == sr.Set.SetId && sr.Set.WinnerPlayerId == g.Key)
                        })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}

