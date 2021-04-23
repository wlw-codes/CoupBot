using System;
using CoupBot.Common;
using CoupBot.Common.Structures;

namespace CoupBot.Database.Models
{
    public class Guild : Model
    {
        public Guild(ulong guildId)
        {
            GuildId = guildId;
        }

        // Numerical
        public ulong GuildId { get; set; }

        // Text
        public string Prefix { get; set; } = Configuration.Prefix;

        // Coup
        public Coup CurrentCoup { get; set; } = null;
        
        // Timespan
        public TimeSpan CoupCampaignTime { get; set; } =
            TimeSpan.FromHours(Configuration.DefaultCoupCampaignTimeInHours);
    }
}
