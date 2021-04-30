using System;
using System.Collections.Generic;
using CoupBot.Common;
using CoupBot.Common.Structures;

namespace CoupBot.Database.Models
{
    public class Guild : Entity
    {
        // Text
        public string Prefix { get; set; } = Configuration.Prefix;

        // Timespan
        public TimeSpan CoupCampaignTime { get; set; } =
            TimeSpan.FromHours(Configuration.DefaultCoupCampaignTimeInHours);

        public TimeSpan CoupCooldown { get; set; } = TimeSpan.FromHours(Configuration.DefaultCoupCooldownInHours);

        // List
        public List<Coup> Coups { get; set; } = new List<Coup>();
    }
}