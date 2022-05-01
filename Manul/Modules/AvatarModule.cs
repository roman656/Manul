using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Manul.Modules
{
    public class AvatarModule : ModuleBase<SocketCommandContext>
    {
        private const int FailureRate = 90;
        private readonly Random _random = new ();
        
        [Command("avatar"), Alias("a", "ava", "а", "ава", "аватар", "аватарка", "аватарочка")]
        [Summary("подгоню тебе аватарку (либо твою, либо чужую)))")]
        public async Task GetAvatarAsync([Summary("чью аватарку тебе принести")][Remainder] IGuildUser user = null)
        {
            var builder = new EmbedBuilder { Color = Config.EmbedColor, Title = "На держи, только тихо..." };

            user ??= (IGuildUser)Context.User;

            if (user.Username == "pomaxpen" && Context.User.Username != "pomaxpen")
            {
                builder.Title = "";
                builder.Description = "**Отказано в доступе.**";
            }
            else if (user.Username == "Манул" && _random.Next(100) + 1 <= FailureRate)
            {
                builder.Title = "";
                builder.Description = "**Не покажу. Я стесняюсь...**";
            }
            else
            {
                var avatarUrl = user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl();
                builder.Description = $"**Если что - это аватарка** ***{user.Username}***";
                builder.WithImageUrl(avatarUrl);
            }

            await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
        }
    }
}