using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Manul.SecretModules;
using Manul.Services;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Manul;

public static class Program
{
    public static readonly List<VotingData> VotingData = [];
    public static readonly List<SecretModule> SecretModules =
    [
        new GreetingsModule(),
        new WolfModule(),
        new MyDreamsModule()
    ];

    public static void Main()
    {
        try
        {
            LoggingService.PrepareLogger();
            Config.Upload();
            MainAsync().GetAwaiter().GetResult();
        }
        catch (Exception exception)
        {
            Log.Fatal("{Message}", exception.Message);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static async Task MainAsync()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);

        var provider = services.BuildServiceProvider();
        provider.GetRequiredService<LoggingService>();
        provider.GetRequiredService<CommandHandler>();

        await provider.GetRequiredService<StartupService>().StartAsync();
        await Task.Delay(Timeout.Infinite);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = Config.MessageCacheSize,
                AlwaysDownloadUsers = true,
                GatewayIntents = GatewayIntents.MessageContent |
                                 GatewayIntents.Guilds |
                                 GatewayIntents.GuildMembers |
                                 GatewayIntents.GuildMessageReactions |
                                 GatewayIntents.GuildMessages |
                                 GatewayIntents.GuildVoiceStates
            } ))
            .AddSingleton(new CommandService(new CommandServiceConfig
                { LogLevel = LogSeverity.Verbose, DefaultRunMode = RunMode.Async } ))
            .AddSingleton<CommandHandler>()
            .AddSingleton<StartupService>()
            .AddSingleton<LoggingService>();
    }
}
