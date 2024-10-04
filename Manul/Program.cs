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

    public static int Main(string[] args)
    {
        if ((args.Length > 1) || ((args.Length == 1) && args[0].Equals("--help")))
        {
            Console.WriteLine("Usage: Manul [configFilePath]");
            return (args.Length > 1) ? 1 : 0;
        }
        
        var configFilePath = (args.Length == 1) ? args[0] : null;

        if (!SetupConfigAndLogger(configFilePath)) { return 2; }
        
        try
        {
            MainAsync().GetAwaiter().GetResult();
        }
        catch (Exception exception)
        {
            Log.Fatal("{Message}", exception.Message);
            return 3;
        }
        finally
        {
            Log.CloseAndFlush();
        }

        return 0;
    }

    private static bool SetupConfigAndLogger(string configFilePath)
    {
        try
        {
            Config.Upload(configFilePath);
            LoggingService.PrepareLogger(Config.LogToConsole, Config.LogToFile);
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return false;
        }

        return true;
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
