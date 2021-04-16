using CoupBot.Common;

namespace CoupBot.Database.Models
{
    public partial class Guild : Model
    {
        public Guild(ulong guildId)
        {
            GuildId = guildId;
        }

        // Numerical
        public ulong GuildId { get; set; }

        // Text
        public string Prefix { get; set; } = Configuration.Prefix;

        // Bool
        public bool CoupActive { get; set; } = false;
    }
}
