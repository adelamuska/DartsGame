﻿namespace DartsGame.RequestDTOs
{
    public class ChangeTurnRequest
    {
        public Guid MatchId { get; set; }
        public string Throw1 { get; set; }
        public string Throw2 { get; set; }
        public string Throw3 { get; set; }
    }
}
