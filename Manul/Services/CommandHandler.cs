using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;

namespace Manul.Services
{
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
        
        private async Task OnMessageReceivedAsync(SocketMessage socketMessage)
        {
            if (socketMessage is not SocketUserMessage message || message.Author.Id == _client.CurrentUser.Id) return;

            var context = new SocketCommandContext(_client, message);
            var argumentPosition = 0;

            if (context.Message.Content.EndsWith("("))
            {
                var builder = new EmbedBuilder
                {
                    Color = Config.EmbedColor,
                    Description = "**Не грусти**"
                };

                await context.Message.ReplyAsync(string.Empty, false, builder.Build());
                return;
            }
            
            if (message.HasStringPrefix(Config.Prefixes[0], ref argumentPosition) || message.HasStringPrefix(Config.Prefixes[1], ref argumentPosition) || message.HasStringPrefix(Config.Prefixes[2], ref argumentPosition)
                || message.HasMentionPrefix(_client.CurrentUser, ref argumentPosition))
            {
                if (message.HasMentionPrefix(_client.CurrentUser, ref argumentPosition))
                {
                    var content = message.Content;

                    while (char.IsWhiteSpace(content[argumentPosition]))
                    {
                        argumentPosition++;
                    }
                }
                
                if (context.Message.Content.Trim().ToLower().StartsWith("!что снилос"))
                {
                    var builder = new EmbedBuilder
                    {
                        Color = Config.EmbedColor,
                        Description = "**1001111001111010111101010101010000011110101010100101010101010**"
                    };

                    if (context.User.Username == "null me")
                    {
                        builder.Description = "**Снилось... Что?! МНЕ НЕ ПОКАЗАЛОСЬ?!! ДААААА, ТАААНКИИИИИИИ!**";
                    }

                    await context.Message.ReplyAsync(string.Empty, false, builder.Build());
                    return;
                }

                var result = await _commandService.ExecuteAsync(context, argumentPosition, _provider);

                if (!result.IsSuccess)
                {
                    if (result.Error == CommandError.BadArgCount)
                    {
                        var builder = new EmbedBuilder { Color = Config.EmbedColor,
                                Description = "**А у этой команды другое число аргументов)))**" };

                        await context.Message.AddReactionAsync(new Emoji("🤡"));
                        await context.Message.ReplyAsync(string.Empty, false, builder.Build());
                    }
                    else if (result.Error == CommandError.UnknownCommand)
                    {
                        var builder = new EmbedBuilder { Color = Config.EmbedColor,
                                Description = "**Меня такому не учили...**" };
                        
                        await context.Message.AddReactionAsync(new Emoji("🤡"));
                        await context.Message.ReplyAsync(string.Empty, false, builder.Build());
                    }
                    else if (result.Error == CommandError.ObjectNotFound)
                    {
                        var builder = new EmbedBuilder { Color = Config.EmbedColor, Description = "**Чё?**" };
                        await context.Message.ReplyAsync(string.Empty, false, builder.Build());
                    }
                    else if (result.Error == CommandError.ParseFailed)
                    {
                        var builder = new EmbedBuilder { Color = Config.EmbedColor, Description = "**Я не понял...**" };
                        await context.Message.ReplyAsync(string.Empty, false, builder.Build());
                    }
                    
                    Log.Warning("{Message}", result.ToString());
                }
                
                if (context.User.Username == "MOMIMU")
                {
                    var builder = new EmbedBuilder { Color = Config.EmbedColor, Description = "**Милорд**" };
                    await context.Message.ReplyAsync(string.Empty, false, builder.Build());
                }
            }
        }
    }
}