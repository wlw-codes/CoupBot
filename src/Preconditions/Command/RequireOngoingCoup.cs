using System;
using System.Threading.Tasks;
using CoupBot.Common.Extensions.Models;
using Discord.Commands;

namespace CoupBot.Common.Preconditions.Command
{
    public class RequireOngoingCoup : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext commandContext,
            CommandInfo command, IServiceProvider services)
        {
            if (commandContext is not Context context) // if we cannot convert commandContext to Context for some reason
                return Task.FromResult(PreconditionResult.FromError("failed to create Context."));

            var coupSearch = context.DbGuild.GetCurrentCoup();

            if (coupSearch == null) // if the guild has no historic coups
            {
                return Task.FromResult(PreconditionResult.FromError(
                    $"there is no ongoing coup at this time. You can initiate one by using `{context.DbGuild.Prefix}callcoup`."));
            }

            // code below only executes if the guild has had at least one coup before

            var coupExpiresOn =
                coupSearch.TimeInitiated.Add(context.DbGuild.CoupCampaignTime); // the DateTime the coup expires on

            if (DateTime.Now <= coupExpiresOn)
                return Task.FromResult(PreconditionResult.FromSuccess()); // if the coup is still ongoing

            var cooldownOverOn =
                coupExpiresOn.Add(context.DbGuild.CoupCooldown); // the DateTime the coup and cooldown expires on

            return Task.FromResult(PreconditionResult.FromError(
                DateTime.Now < cooldownOverOn // if the cooldown is still in effect
                    ? $"there is no coup as this time, as the owner has enough strength to retain control until at least **{cooldownOverOn.ToShortDateString()}** at **{cooldownOverOn.ToShortTimeString()}**."
                    : $"there is no ongoing coup at this time. You can initiate one by using `{context.DbGuild.Prefix}callcoup`."));
        }
    }
}