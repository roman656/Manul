namespace Manul.Modules;

using System.IO;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

public class AvatarModule : ModuleBase<SocketCommandContext>
{
    private const int FailureRate = 70;
    private const int AvatarSize = 1024;
    private readonly Random _random = new ();
    private static readonly Dictionary<string, List<string>> VipUsers = new ()
    {
        { "pomaxpen", ["**А может тебе ещё спину вареньем намазать?))**", "**Отказано в доступе**"] },
        { "kirchq", ["**1113**"] }
    };

    [Command("avatar")]
    [Alias("a", "ava", "userpic", "юзерпик", "а", "ава", "авка", "аватар", "аватарка", "аватарочка")]
    [Summary("подгоню тебе аватарку (либо твою, либо чужую)))")]
    public async Task GetAvatarAsync([Summary("чью аватарку тебе принести")][Remainder] IGuildUser user = null)
    {
        var builder = new EmbedBuilder { Color = Config.EmbedColor };

        user ??= (IGuildUser)Context.User;

        if (VipUsers.ContainsKey(user.Username) && Context.User.Username != user.Username)
        {
            var answersList = VipUsers[user.Username];

            builder.Description = answersList[_random.Next(answersList.Count)];
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

    [Command("triggered")]
    [Alias("триггер", "триггеред", "trigger", "тригер", "тригеред", "triger", "trigered", "триггернуло",
            "тригернуло")]
    [Summary("фотошоплю аватарку типа триггеред")]
    public async Task GetTriggeredAvatarAsync([Summary("кто триггерится")][Remainder] IGuildUser user = null)
    {
        var builder = new EmbedBuilder { Color = Config.EmbedColor };

        user ??= (IGuildUser)Context.User;

        if (VipUsers.ContainsKey(user.Username) && Context.User.Username != user.Username)
        {
            var answersList = VipUsers[user.Username];

            builder.Description = answersList[_random.Next(answersList.Count)];
        }
        else if (user.Username == "Манул" && _random.Next(100) + 1 <= FailureRate)
        {
            builder.Description = _random.Next(2) == 1 ? "**Не покажу. Я стесняюсь...**" : "**Неа))**";
        }
        else
        {
            var avatarUrl = user.GetAvatarUrl(size: AvatarSize) ?? user.GetDefaultAvatarUrl();
            var uri = new Uri($"https://some-random-api.ml/canvas/triggered?avatar={avatarUrl}");
            using var httpClient = new HttpClient();
            var fileBytes = await httpClient.GetByteArrayAsync(uri);
            Stream stream = new MemoryStream(fileBytes);

            await Context.Channel.SendFileAsync(stream, "triggered.gif");
            return;
        }

        await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
    }
}