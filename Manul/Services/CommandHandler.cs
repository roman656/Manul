using System;
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
        private readonly Random _random = new ();
        private static readonly string[] Answers =
        {
            "**Здравствуй! Зачем зовёшь?) По рофлу или дело есть?))**", "**Здарова! Что снилось?))**", "**Привет)**",
            "**Ку!**", "**Привет!**", "**Я Вас категорически приветствую!**", "**Ну здарова!**", "**Миу-миу-миу**",
            "***Приветствует по-манульи.***", "**МЯЯЯЯЯЯУ!**", "**Здарова!**", "**О, привет!**"
        };
        
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

            if (message.HasStringPrefix(Config.Prefix, ref argumentPosition)
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

                if (context.Message.Content.Trim().ToLower().StartsWith("!манул")
                        || context.Message.Content.Trim().ToLower().StartsWith("!монул")
                        || context.Message.Content.Trim().ToLower().StartsWith("!минул")
                        || context.Message.Content.Trim().ToLower().StartsWith("!pallascat")
                        || context.Message.Content.Trim().ToLower().StartsWith("!pallas cat")
                        || context.Message.Content.Trim().ToLower().StartsWith("!кот")
                        || context.Message.Content.Trim().ToLower().StartsWith("!привет"))
                {
                    var builder = new EmbedBuilder
                    {
                        Color = Config.EmbedColor,
                        Description = Answers[_random.Next(Answers.Length)]
                    };

                    if (context.User.Username == "null me" && _random.Next(100) + 1 <= 35)
                    {
                        builder.Description = "Привет Лисичка!!!";
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
                    
                    Log.Warning("{Message}", result.ToString());
                }
            }
        }
    }
}