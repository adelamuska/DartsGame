using DartsGame.DTO;
using DartsGame.Entities;

namespace DartsGame.Interfaces
{
    public interface ITurnService
    {
        Task ChangeTurn(Match match, TurnThrowRequestDTO turnThrows);
    }
}
