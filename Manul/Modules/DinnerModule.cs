using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Manul.Modules;

public class DinnerModule : ModuleBase<SocketCommandContext>
{
    private readonly Random _random = new ();
    private readonly Dictionary<string, string[]> _menu = new()
    {
        {
            "Фирменные напитки",
            ["Квас", "Морс", "Сок берёзовый", "Питень", "Тархун", "Узвар боярышник", "Узвар груша-дичка", "Узвар шиповник", "Квас Белый Смородина и Мята", "Квас Бородинский"]
        },
        {
            "Блины сладкие",
            ["Блин с шоколадным кремом", "Блин Ватрушка", "Блин с шоколадным кремом и бананом", "Блин Сгущёнка", "Блин яблочно-карамельный", "Блинчик с вишнёвым вареньем", "Блинчик с клубничным вареньем"]
        },
        {
            "Салаты",
            ["Салат \"Оливье\"", "Салат \"Оливье\" постный", "Салат \"Русский Цезарь\"", "Салат Винегрет постный", "Салат Винегрет с сёмгой", "Салат Витаминчик с орешками", "Сельдь под шубой"]
        },
        {
            "Супы",
            ["Борщ \"Теремковский\"", "Борщ постный с булочкой", "Лапша куриная с фрикадельками", "Суп гороховый постный с булочкой", "Суп гороховый с копчёностями", "Сырный крем-суп", "Уха из форели по-фински"]
        },
        {
            "Пельмени",
            ["Пельмени", "Пельмени двойная порция"]
        },
        {
            "Вареники",
            ["Вареники с картофелем и грибами"]
        },
        {
            "Хлеб",
            ["Булочка пшеничная", "Булочка ржаная", "Чипы блинные в уголке"]
        }
    };

    [Command("dinner"), Alias("теремок", "обед", "меню", "кушать", "еда", "eat", "teremok", "есть",
         "трапеза", "food", "ужин", "menu")]
    [Summary("что будете заказывать?")]
    public async Task DinnerAsync()
    {
        var builder = new EmbedBuilder { Color = Config.EmbedColor };
        var brandedDrinksChoice = _menu["Фирменные напитки"][_random.Next(_menu.ElementAt(0).Value.Length)];
        var sweetPancakesChoice = _menu.ElementAt(1).Value[_random.Next(_menu.ElementAt(1).Value.Length)];
        var saladsChoice = _menu.ElementAt(2).Value[_random.Next(_menu.ElementAt(2).Value.Length)];

        builder.Description = $"**Мои рекомендации таковы:\n *— {saladsChoice}\n — {sweetPancakesChoice}\n — {brandedDrinksChoice}***";

        await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
    }
}