using System;
using System.Linq;
using Discord;
using System.Threading.Tasks;
using Discord.Commands;

namespace Manul.SecretModules;

public abstract class SecretModule(string[] keywords, string[] answers)
{
    protected readonly string[] Keywords = keywords;
    protected readonly string[] Answers = answers;

    public virtual bool WasCalled(string message) => Keywords.Any(message.StartsWith);

    public virtual async Task SendReplyAsync(SocketCommandContext context)
    {
        var random = new Random();
        var builder = new EmbedBuilder
        {
            Color = Config.EmbedColor,
            Description = Answers[random.Next(Answers.Length)]
        };

        await context.Message.ReplyAsync(string.Empty, false, builder.Build());
    }
}