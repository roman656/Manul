using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Manul.Modules
{
    public class AvatarModule : ModuleBase<SocketCommandContext>
    {
        private const int FailureRate = 70;
        private const int AvatarSize = 1024;
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
                builder.Description = _random.Next(2) == 1 ? "**А может тебе ещё спину вареньем намазать?))**"
                        : "**Отказано в доступе.**";
            }
            else if (user.Username == "submarinecap" && Context.User.Username != "submarinecap")
            {
                builder.Description = "**1113**";
            }
            else if (user.Username == "Манул" && _random.Next(100) + 1 <= FailureRate)
            {
                builder.Description = _random.Next(2) == 1 ? "**Не покажу. Я стесняюсь...**" : "**Неа))**";
            }
            else
            {
                var avatarUrl = user.GetAvatarUrl(size: AvatarSize) ?? user.GetDefaultAvatarUrl();

                builder.Title = "На держи, только тихо...";
                builder.Description = $"**Если что - это аватарка {user.Mention}**";
                builder.WithImageUrl(avatarUrl);
            }

            await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
        }
    }
}