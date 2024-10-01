using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Manul.SecretModules;

public class MyDreamsModule() : SecretModule(
    keywords:
    [
        "что снилос", "че снилос", "сон", "чё снилос", "шо снилос",
    ],
    answers:
    [
        "**010010010010000001101000011000010110010000100000011000010010000001100100011100100110010101100001011011010010000001100001011000100110111101110101011101000010000001110111011011110111001001101011**",
        "**010011110111011001100101011100100110000101101100011011000010000001010011010101010010110100110001001100000011000000100000011010010111001100100000011000010010000001110000011011000110010101100001011100110110000101101110011101000010000001101101011000010110001101101000011010010110111001100101**",
        "**Напишите программу, переводящую N чисел в формат single. Пользователь вводит с клавиатуры количество чисел N, а также сами числа, разделяя их пробелами**",
        "**Поздравляем! Мы готовы предложить Вам неоплачиваемую бесконечную стажировку в компании \"Miodenus Project MLG Team\" без последующего трудоустройства!**",
        "**Не помню что-то)**",
        "**В последнее время что-то странное снится. Сложно описать, а когда сложно я сдаюсь...**",
    ])
{
    private static readonly Dictionary<string, List<string>> VipUsers = new ()
    {
        {
            "null_me",
            [
                "**Весьма занятный бред)**",
                "**Секрет вкусных пельменей от Шлёпы... Хорошие пельмени — это очень, очень вкусно! Вроде нужно много мяса...**",
                "**Что бы там мне ни приснилось, там точно были танки. Много танков!**",
                "**Снились прикольные разговоры манулов в голосовом канале связи, Лисичка. Ну, и как в меня было заложено — танки. ДААААА, ТАНКИ! Начни сливать прямо сейчас...**",
                "**Да сплошной 🍫шоколад🍫)**",
                "**Хотел поесть своего любимого супа из опилок, но проснулся и пошёл в шахту ⛏⛏⛏**",
            ]
        }
    };

    public override async Task SendReplyAsync(SocketCommandContext context)
    {
        var random = new Random();
        var builder = new EmbedBuilder { Color = Config.EmbedColor };

        if (VipUsers.TryGetValue(context.User.Username, out var answersList))
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