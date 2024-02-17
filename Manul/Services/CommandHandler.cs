using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;

namespace Manul.Services;

public class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commandService;
    private readonly IServiceProvider _provider;

    public CommandHandler(DiscordSocketClient client, CommandService commandService, IServiceProvider provider)
    {
        _client = client;
        _commandService = commandService;
        _provider = provider;

        _client.MessageReceived += OnMessageReceivedAsync;
    }

    private bool HasMessageBotPrefixes(SocketUserMessage message, ref int argumentPosition)
    {
        if (message.Content.Length > Config.Prefix.Length
                && message.HasStringPrefix(Config.Prefix, ref argumentPosition)
                && !char.IsWhiteSpace(message.Content[argumentPosition]))
        {
            return true;
        }
        else if (message.HasMentionPrefix(_client.CurrentUser, ref argumentPosition)
                && message.Content.Length > argumentPosition)
        {
            while (message.Content.Length > argumentPosition && char.IsWhiteSpace(message.Content[argumentPosition]))
            {
                argumentPosition++;
            }

            return true;
        }

        return false;
    }

    private async Task OnMessageReceivedAsync(SocketMessage socketMessage)
    {
        if (socketMessage is not SocketUserMessage message || message.Author.Id == _client.CurrentUser.Id) return;

        var context = new SocketCommandContext(_client, message);
        var argumentPosition = 0;

        if (HasMessageBotPrefixes(message, ref argumentPosition))
        {
            if (context.User.Username == "_momimu_")
            {
                var builder = new EmbedBuilder { Color = Config.EmbedColor, Description = "**Милорд**" };
                await context.Message.ReplyAsync(string.Empty, false, builder.Build());
            }

            var result = await _commandService.ExecuteAsync(context, argumentPosition, _provider);

            if (!result.IsSuccess)
            {
                var builder = new EmbedBuilder { Color = Config.EmbedColor };

                if (await SearchForSecretKeywordsAsync(context, argumentPosition)) return;

                switch (result.Error)
                {
                    case CommandError.BadArgCount:
                        {
                            builder.Description = "**А у этой команды другое число аргументов)))**";

                            await context.Message.AddReactionAsync(new Emoji("🤡"));
                            await context.Message.ReplyAsync(string.Empty, false, builder.Build());
                            break;
                        }
                    case CommandError.UnknownCommand:
                        {
                            builder.Description = "**Меня такому не учили...**";

                            await context.Message.AddReactionAsync(new Emoji("🤡"));
                            await context.Message.ReplyAsync(string.Empty, false, builder.Build());
                            break;
                        }
                    case CommandError.ObjectNotFound:
                        {
                            builder.Description = "**Чё?**";

                            await context.Message.ReplyAsync(string.Empty, false, builder.Build());
                            break;
                        }
                    case CommandError.ParseFailed:
                        {
                            builder.Description = "**Я не понял...**";

                            await context.Message.ReplyAsync(string.Empty, false, builder.Build());
                            break;
                        }
                    case CommandError.MultipleMatches:
                        {
                            builder.Description = "**Неоднозначненько выходит)))**";

                            await context.Message.ReplyAsync(string.Empty, false, builder.Build());
                            break;
                        }
                }

                Log.Warning("{Message}", result.ToString());
            }
        }
    }

    private static async Task<bool> SearchForSecretKeywordsAsync(SocketCommandContext context, int argumentPosition)
    {
        var wasFound = false;
        var message = context.Message.Content[argumentPosition..].Trim().ToLower();

        foreach (var module in Program.SecretModules)
        {
            if (module.WasCalled(message))
            {
                await module.SendReplyAsync(context);
                wasFound = true;
                break;
            }
        }

        return wasFound;
    }
}