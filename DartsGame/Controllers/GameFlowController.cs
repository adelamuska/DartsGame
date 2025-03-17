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

       
    }
}

