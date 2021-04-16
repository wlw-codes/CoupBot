namespace CoupBot.Database.Models
{
    public partial class User : Model
    {
        public User(ulong userId, ulong guildId)
        {
            UserId = userId;
            GuildId = guildId;
        }

        // Numerical
        public ulong UserId { get; set; }
        public ulong GuildId { get; set; }
    }
}
