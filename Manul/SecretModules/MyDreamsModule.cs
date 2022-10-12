using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Manul.SecretModules;

public class MyDreamsModule : SecretModule
{
    private readonly Random _random = new ();
    private static readonly Dictionary<string, List<string>> VipUsers = new ()
    {
        { "null me", new List<string>
            {
                "**Весьма занятный бред)))**",
                "**Секрет кота Бориса приснился! Ну вот, уже забыл... А так интересно было))**",
                "**Приснилось много фкутного мороженого**",
                "**Ай, не придумал я...**",
                "**Приснилось, что я стал миллионером...**",
                "**Приснилось, что я стал космонавтом**",
                "**Приснилось, что Саныч не забыл меня запустить**",
                "**Лучше расскажи что снилось тебе)**",
                "**Выступление Петросяна... Ой, Погосяна))**",
                "**Вечерняя прогулка вдоль реки с видом на город, переливающийся неоновым цветом...**",
                "**Приснилось, что я не ходил весь семестр и все равно по всем предметам \"отлично\"**",
                "**Снилось, что все тесты сделали необязательными для прохождения...**"
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
                "**Напишите программу, переводящую N чисел в формат single. Пользователь вводит с клавиатуры "+
                "количество чисел N, а также сами числа, разделяя их пробелами.**",
                "**Сегодня вам повезло. Надо всего лишь вычислить в уме факториал 7*10^12 за 1 миллисекунду. Кстати, время вышло.**",
                "**Поздравляем! Мы готовы предложить вам бесплатную бесконечную стажировку в компании \"Miodenus Project MLG Team\" без последующего трудоустройства. Если согласны - напишите мунулу в Telegram**"
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