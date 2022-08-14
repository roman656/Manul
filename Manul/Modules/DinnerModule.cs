namespace Manul.Modules;

using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

public class DinnerModule : ModuleBase<SocketCommandContext>
{
    private readonly Random _random = new ();
    private readonly string[] _drinks =
    {
        "квас", "морс", "березовый сок", "питень", "тархун", "водичка колодезная если повезет"
    };
    
    private readonly string[] _blins =
    {
        "блинчик с клубничным вареньем", "блин фермер", "блин цезарь", "блин морской богатырь", 
        "блин с яблоками и карамелью", "блин мясной богатырь", "блин двойной с икрой", "блин с икрой"
    };
    
    private readonly string[] _salats =
    {
        "салат витаминчик", "винегрет", "не помню я"
    };
    
    private readonly string[] _soups =
    {
        "суп гороховый", "борщ", "уха (вроде)", "лень вспоминать"
    };
    
    private readonly string[] _mainCourses =
    {
        "картофель по-фермерски", "картофель постный", "гречка постная", "гречка", "пюре", "пельмени"
    };

    [Command("dinner"), Alias("теремок", "ням", "обед", "меню", "кушать", "еда", "eat",
            "teremok", "есть", "покушать", "хрум", "food", "жрать")]
    [Summary("выберу за тебя еду из теремка)")]
    public async Task DinnerAsync()
    {
        var builder = new EmbedBuilder { Color = Config.EmbedColor };
        
        builder.Description = $"**Твое сегодняшнее меню: {_mainCourses[_random.Next(_mainCourses.Length)]}, {_soups[_random.Next(_soups.Length)]}, {_salats[_random.Next(_salats.Length)]}, {_blins[_random.Next(_blins.Length)]}, {_drinks[_random.Next(_drinks.Length)]}**";

        await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
    }
}