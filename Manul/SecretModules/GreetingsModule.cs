using System.Collections.Generic;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord;

namespace Manul.SecretModules;

public class GreetingsModule() : SecretModule(
    keywords:
    [
        "greet", "hello", "hi", "манул", "монул", "минул", "pallas", "кот", "кит", "manul",
        "прив", "дратут", "ку", "здаров", "даров", "хай", "салют", "здра"
    ],
    answers:
    [
        "**Здравствуй! Зачем зовёшь?) По рофлу или дело есть?))**", "**Здарова! Что снилось?))**",
        "**Ку!**", "**Привет!**", "**Ну здарова!**", "**Миу-миу-миу**", "***Приветствует по-манульи***",
        "**МЯЯЯЯЯЯУ**", "**Здарова!**", "**О, привет!**", "**Здравствуй!**", "**Привет)**", "**Даров)**",
        "**Да, привет)**"
    ])
{
    private const int PersonalizedResponseRate = 45;
    private static readonly Dictionary<string, List<string>> VipUsers = new ()
    {
        { "null_me", [ ":cat2:", "**Привет, Лисичка!**", "**Ну привет, Мысленная 🦊))**" ] },
        { "poormercymain", [ "**Я Вас категорически приветствую, Капитан Флексер!**", "**Клим Саныч, здравствуйте!**" ] }
    };

    public override async Task SendReplyAsync(SocketCommandContext context)
    {
        var random = new Random();
        var builder = new EmbedBuilder { Color = Config.EmbedColor };

        if (VipUsers.TryGetValue(context.User.Username, out var answersList) && random.Next(100) < PersonalizedResponseRate)
        {
            builder.Description = answersList[random.Next(answersList.Count)];
        }
        else
        {
            builder.Description = Answers[random.Next(Answers.Length)];
        }

        await context.Message.ReplyAsync(string.Empty, false, builder.Build());
    }
}