using System;

namespace CoupBot.Common
{
    public static class Configuration
    {
        public const string Prefix = "$",
            Version = "0.6.0",
            InviteLink =
                "https://discord.com/oauth2/authorize?client_id=832358656569638997&scope=bot&permissions=268520528",
            RepositoryUrl = "https://github.com/wlw-codes/CoupBot",
            SupportServerUrl = "https://discord.gg/qN4ZYd6AYZ",
            OwnerTag = "Lumite_#0187";

        public const int CoupAttemptCancelTime = 10,
            DefaultCoupCampaignTimeInHours = 48,
            DefaultCoupCooldownInHours = 72;

        public static readonly string Game = $"{Prefix}help | v{Version}",
            HelpMessage = $"To view all available commands, please use `{Prefix}commands`.\n" +
                          $"To join the Discord server, please use `{Prefix}discord` and ping **{OwnerTag}**.\n" +
                          $"To get technical assistance, please use `{Prefix}git` and fill out an issue form.\n\n" +
                          $"To get some useful information about what this bot does, please use `{Prefix}readme`.";
    }
}
