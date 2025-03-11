using DartsGame.Entities;
using DartsGame.RequestDTOs;

namespace DartsGame.Interfaces
{
    public interface ITurnService
    {
        Task ProcessTurn(Match match, TurnThrowRequestDTO turnThrows);
    }
}
