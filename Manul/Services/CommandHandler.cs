using System;
using System.Threading.Tasks;
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
            
            if (message.HasStringPrefix(Config.Prefix, ref argumentPosition)
                    || message.HasMentionPrefix(_client.CurrentUser, ref argumentPosition))
            {
                var result = await _commandService.ExecuteAsync(context, argumentPosition, _provider);

                if (!result.IsSuccess)
                {
                    Log.Warning("{Message}", result.ToString());
                }
            }
        }
    }
}