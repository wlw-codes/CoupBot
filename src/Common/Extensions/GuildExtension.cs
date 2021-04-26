using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace CoupBot.Common.Extensions
{
    public static class GuildExtension
    {
        public static async Task<int> GetMessageCount(this IGuild guild)
        {
            var count = 0;
            
            foreach (var textChannel in await guild.GetTextChannelsAsync()) // for every text channel in the guild
            {
                var totalMessages = await textChannel.GetMessagesAsync(int.MaxValue).FlattenAsync(); // get all messages in that channel
                
                count += totalMessages.Count(); // add that number of messages to the total
            } 

            return count;
        }
    }
}