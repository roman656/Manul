namespace Manul.Services;

using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

public class LoggingService
{
    public LoggingService(DiscordSocketClient client, CommandService commandService)
    {
        client.Log += LogAsync;
        commandService.Log += LogAsync;
    }
        
    private static async Task LogAsync(LogMessage message)
    {
        var severity = message.Severity switch
        {
            LogSeverity.Critical => LogEventLevel.Fatal,
            LogSeverity.Error => LogEventLevel.Error,
            LogSeverity.Warning => LogEventLevel.Warning,
            LogSeverity.Info => LogEventLevel.Information,
            LogSeverity.Verbose => LogEventLevel.Debug,
            LogSeverity.Debug => LogEventLevel.Verbose,
            _ => LogEventLevel.Information
        };
            
        Log.Write(severity, message.Exception, "{Source} -> {Message}", message.Source, message.Message);
        await Task.CompletedTask;
    }
        
    public static void PrepareLogger()
    {
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen,
                        outputTemplate: "[{Timestamp:HH:mm:ss}] ({Level:u3}) {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
    }
}