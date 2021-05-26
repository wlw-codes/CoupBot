using System;
using System.Threading.Tasks;
using CoupBot.Common.Extensions.Models;
using Discord.Commands;

namespace CoupBot.Common.Preconditions.Command
{
    public class NotOwner : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext commandContext,
            CommandInfo command, IServiceProvider services)
        {
            return commandContext is not Context context // if we cannot convert commandContext to Context for some reason
                ? Task.FromResult(PreconditionResult.FromError("failed to create Context."))
                : Task.FromResult(context.Message.Author.Id == context.Guild.OwnerId // if the user is the owner of the guild
                    ? PreconditionResult.FromError("you cannot use this command as you are the owner of the server!")
                    : PreconditionResult.FromSuccess());
        }
    }
}