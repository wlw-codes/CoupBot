namespace CoupBot.Common
{
    public static class Configuration
    {
        public static readonly string Prefix = "$",
            Version = "0.1.0",
            Game = $"{Prefix}help | v{Version}",
            InviteLink =
                "https://discord.com/oauth2/authorize?client_id=832358656569638997&scope=bot&permissions=268520528",
            HelpMessage = $"Here's some help... {InviteLink}";
    }
}
