using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace CoupBot.Common.Extensions
{
    public static class GuildUserExtension
    {
        public static async Task<int> GetMessageCount(this IGuildUser guildUser)
        {
            var count = 0;
            
            foreach (var textChannel in await guildUser.Guild.GetTextChannelsAsync()) // for every text channel in the guild
            {
                var totalMessages = await textChannel.GetMessagesAsync(int.MaxValue).FlattenAsync(); // get all messages in that channel
                var messageCountFromUser = totalMessages.Count(x => x.Author.Id == guildUser.Id); // get number of messages from user

                count += messageCountFromUser; // add that number of messages to the total
            }

            return count;
        }
    }
}