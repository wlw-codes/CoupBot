using System;

namespace CoupBot.Common.Structures
{
    public class Coup
    {
        public ulong ChallengerId { get; set; }

        public DateTime TimeInitiated { get; set; }

        public int TotalPossibleVotes { get; set; }
        public int VotesForChallenger { get; set; } = 0;
        public int VotesForRuler { get; set; } = 0;
    }
}