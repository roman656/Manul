namespace Manul.Services;

using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

public class StartupService
{
    private readonly IServiceProvider _provider;
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commandService;

    public StartupService(IServiceProvider provider, DiscordSocketClient client, CommandService commandService)
    {
        _provider = provider;
        _client = client;
        _commandService = commandService;
    }

    public async Task StartAsync()
    {
        await _client.LoginAsync(TokenType.Bot, Program.Config.Token);
        await _client.SetGameAsync(Config.GameActivityName);
        await _client.StartAsync();
        await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
    }
}