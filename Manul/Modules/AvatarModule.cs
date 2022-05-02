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
        
        [Command("avatar")]
        [Alias("a", "ava", "userpic", "юзерпик", "а", "ава", "аватар", "аватарка", "аватарочка")]
        [Summary("подгоню тебе аватарку (либо твою, либо чужую)))")]
        public async Task GetAvatarAsync([Summary("чью аватарку тебе принести")][Remainder] IGuildUser user = null)
        {
            var builder = new EmbedBuilder { Color = Config.EmbedColor };

            user ??= (IGuildUser)Context.User;

            if (user.Username == "pomaxpen" && Context.User.Username != "pomaxpen")
            {
                builder.Description = "**А может тебе ещё спину вареньем намазать?))**";
            }
            else if (user.Username == "Манул" && _random.Next(100) + 1 <= FailureRate)
            {
                builder.Description = "**Не покажу. Я стесняюсь...**";
            }
            else
            {
                var avatarUrl = user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl();

                builder.Title = "На держи, только тихо...";
                builder.Description = $"**Если что - это аватарка** ***{user.Nickname ?? user.Username}***";
                builder.WithImageUrl(avatarUrl);
            }

            await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
        }
    }
}