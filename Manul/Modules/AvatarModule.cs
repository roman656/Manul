using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Manul.Modules
{
    public class AvatarModule : ModuleBase<SocketCommandContext>
    {
        [Command("avatar"), Alias("a", "ava", "а", "ава", "аватар", "аватарка", "аватарочка")]
        [Summary("подгоню тебе аватарку (либо твою, либо чужую)))")]
        public async Task GetAvatarAsync([Summary("чью аватарку тебе принести")] SocketUser user = null)
        {
            var builder = new EmbedBuilder { Color = Config.EmbedColor, Title = "На держи, только тихо..." };

            user ??= Context.User;
            var avatarUrl = user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl();
            builder.Description = $"**Если что - это аватарка** ***{user.Username}***";
            builder.WithImageUrl(avatarUrl);

            await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
        }
    }
}