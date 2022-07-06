namespace Manul.Modules;

using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

public class HelpModule : ModuleBase<SocketCommandContext>
{
    private readonly CommandService _service;

    public HelpModule(CommandService service) => _service = service;

    [Command("help"), Alias("h", "?", "справка", "хелп")]
    [Summary("показываю справку")]
    public async Task HelpAsync()
    {
        var builder = new EmbedBuilder { Color = Config.EmbedColor, Title = "Справка :cat::books:" };
            
        foreach (var module in _service.Modules)
        {
            var description = string.Empty;
                
            foreach (var commandInfo in module.Commands)
            {
                var result = await commandInfo.CheckPreconditionsAsync(Context);
                    
                if (result.IsSuccess)
                {
                    description += $"*{Config.Prefix}{commandInfo.Aliases[0]}{(commandInfo.Parameters.Count > 0 ? " (c аргументами)" : "")}*\n";
                }
            }
                
            if (!string.IsNullOrWhiteSpace(description))
            {
                builder.AddField(x =>
                {
                    x.Name = "Команды модуля " + module.Name.Replace("Module", "");
                    x.Value = description;
                    x.IsInline = false;
                });
            }
        }

        await ReplyAsync(string.Empty, false, builder.Build());
    }

    [Command("help"), Alias("h", "?", "справка")]
    [Summary("показываю справку по конкретной команде")]
    public async Task HelpAsync([Summary("название команды")] string command)
    {
        var result = _service.Search(Context, command);
        var builder = new EmbedBuilder { Color = Config.EmbedColor, Title = "Справка по команде :cat::books:" };

        if (!result.IsSuccess)
        {
            builder.Description = $"**А я не знаю команд, похожих на** ***{(string.IsNullOrEmpty(command) || string.IsNullOrWhiteSpace(command) ? "ты :clown:" : command)}***";
            await ReplyAsync(string.Empty, false, builder.Build());
            return;
        }

        builder.Description = $"**Вот команды, похожие на** ***{command}***";
            
        foreach (var match in result.Commands)
        {
            var commandInfo = match.Command;
                
            builder.AddField(x =>
            {
                x.Name = string.Join(", ", commandInfo.Aliases);
                x.Value = (commandInfo.Parameters.Count > 0 ? $"Аргумент{(commandInfo.Parameters.Count > 1 ? "ы" : "")}: *{string.Join(", ", commandInfo.Parameters.Select(p => p.Summary))}*\n" : "")
                        + $"Подсказка: *{commandInfo.Summary}*";
                x.IsInline = false;
            });
        }

        await ReplyAsync(string.Empty, false, builder.Build());
    }
}