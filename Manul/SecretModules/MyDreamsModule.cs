namespace Manul.SecretModules;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

public class MyDreamsModule : SecretModule
{
    private readonly Random _random = new ();
    private static readonly Dictionary<string, List<string>> VipUsers = new ()
    {
        { "null_me", new List<string>
            {
                "**Весьма занятный бред))**",
                "**Секрет вкусных пельменей от Шлёпы... Хорошие пельмени — это очень, очень вкусно! Вроде нужно много мяса...**",
                "**Снилось... Что?! МНЕ НЕ ПОКАЗАЛОСЬ?!! ДААААА, ТАААНКИИИИИИИ!**",
                "**Снились прикольные разговоры манулов в голосовом канале связи, Лисичка. Ну, и как в меня было заложено — танки. ДААААА, ТАНКИ! Начни играть прямо сейчас...**",
                "**Да сплошной шоколад) 🍫🍫🍫**",
                "**Хотел поесть своего любимого супа из опилок, но проснулся и пошёл в шахту ⛏⛏⛏**"
            }
        }
    };

    public MyDreamsModule() : base(
            keywords: new[]
            {
                "что снилос", "че снилос", "сон", "чё снилос"
            },
            answers: new []
            {
                "**1001111001111010111101010101010000011110101010100101010101010**",
                "**0110001101111000101010101010111010000111111111001110101001001**",
                "**0000000000111110110101010000111101010101011110000011101010101**",
                "**1101011111011110000010011010101011110000111000000111110011010**",
                "**Напишите программу, переводящую N чисел в формат single. Пользователь вводит с клавиатуры количество чисел N, а также сами числа, разделяя их пробелами**",
                "**Поздравляем! Мы готовы предложить Вам неоплачиваемую бесконечную стажировку в компании \"Miodenus Project MLG Team\" без последующего трудоустройства!!!**"
            }) {}

    public override async Task SendReplyAsync(SocketCommandContext context)
    {
        var builder = new EmbedBuilder { Color = Config.EmbedColor };

        if (VipUsers.ContainsKey(context.User.Username))
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