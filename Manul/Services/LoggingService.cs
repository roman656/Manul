using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Manul.Services;

public class LoggingService
{
    public LoggingService(DiscordSocketClient client, CommandService commandService)
    {
        client.Log += LogAsync;
        commandService.Log += LogAsync;
    }

    private static LogEventLevel GetSerilogSeverity(LogSeverity discordSeverity) => discordSeverity switch
    {
        LogSeverity.Critical => LogEventLevel.Fatal,
        LogSeverity.Error => LogEventLevel.Error,
        LogSeverity.Warning => LogEventLevel.Warning,
        LogSeverity.Info => LogEventLevel.Information,
        LogSeverity.Verbose => LogEventLevel.Debug,
        LogSeverity.Debug => LogEventLevel.Verbose,
        _ => LogEventLevel.Information
    };

    private static async Task LogAsync(LogMessage message)
    {
        var severity = GetSerilogSeverity(message.Severity);

        Log.Write(severity, message.Exception, "{Source} -> {Message}", message.Source, message.Message);
        await Task.CompletedTask;
    }

    public static void PrepareLogger()
    {
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss.fff}] ({Level:u3}) {Message:lj}{NewLine}{Exception}",
                        theme: AnsiConsoleTheme.Sixteen)
                .WriteTo.File(
                        path: "logs/log.txt",
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] ({Level:u3}) {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
    }
}