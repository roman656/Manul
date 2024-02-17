namespace Manul.SecretModules;

using System.Collections.Generic;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord;

public class GreetingsModule : SecretModule
{
    private const int PersonalizedResponseRate = 45;
    private readonly Random _random = new ();
    private static readonly Dictionary<string, List<string>> VipUsers = new ()
    {
        { "null_me", [":cat2:", "**Привет, Лисичка!**", "**Ну привет, Мысленная 🦊))**"] },
        { "poormercymain", ["**Я Вас категорически приветствую, Капитан Флексер!**", "**Клим Саныч, здравствуйте!**"] }
    };

    public GreetingsModule() : base(
            keywords: new[]
            {
                "greet", "hello", "hi", "манул", "монул", "минул", "pallas", "кот", "кит", "manul",
                "прив", "дратут", "ку", "здаров", "даров", "хай", "салют", "здра"
            },
            answers: new []
            {
                "**Здравствуй! Зачем зовёшь?) По рофлу или дело есть?))**", "**Здарова! Что снилось?))**",
                "**Ку!**", "**Привет!**", "**Ну здарова!**", "**Миу-миу-миу**", "***Приветствует по-манульи***",
                "**МЯЯЯЯЯЯУ**", "**Здарова!**", "**О, привет!**", "**Здравствуй!**", "**Привет)**", "**Даров)**",
                "**Да, привет)**"
            }) {}

    public override async Task SendReplyAsync(SocketCommandContext context)
    {
        var builder = new EmbedBuilder { Color = Config.EmbedColor };

        if (VipUsers.ContainsKey(context.User.Username) && _random.Next(100) + 1 <= PersonalizedResponseRate)
        {
            var answersList = VipUsers[context.User.Username];

            builder.Description = answersList[_random.Next(answersList.Count)];
        }
        else
        {
            builder.Description = Answers[_random.Next(Answers.Length)];
        }

        await context.Message.ReplyAsync(string.Empty, false, builder.Build());
    }
}