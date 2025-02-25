﻿namespace DartsGame.Entities
{
    public class Set : ISoftDeletable
    {
        public Guid SetId { get; set; }
        public Guid MatchId { get; set; }
        public int BestOfLegs { get; set; }
        public int SetNumber { get; set; }
        public bool IsFinished {  get; set; }
        public bool IsDeleted { get; set; }
        public Guid? WinnerPlayerId { get; set; }

        public Match Match { get; set; }
        public ICollection<Leg> Legs { get; set; }
        public ICollection<SetResult> SetResults { get; set; }

        public Set(Guid setId)
        {
            SetId = setId;
        }
    }
}
