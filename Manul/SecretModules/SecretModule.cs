namespace Manul.SecretModules;

using System;
using System.Linq;
using Discord;
using System.Threading.Tasks;
using Discord.Commands;

public abstract class SecretModule
{
    protected readonly string[] Keywords;
    protected readonly string[] Answers;

    protected SecretModule(string[] keywords, string[] answers)
    {
        Keywords = keywords;
        Answers = answers;
    }

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