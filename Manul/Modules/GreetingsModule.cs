using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Manul.Modules
{
    public class GreetingsModule : ModuleBase<SocketCommandContext>
    {
        private const int PersonalizedResponseRate = 35;
        private readonly Random _random = new ();
        private static readonly string[] GreetingsAnswers =
        {
            "**Здравствуй! Зачем зовёшь?) По рофлу или дело есть?))**", "**Здарова! Что снилось?))**", "**Привет)**",
            "**Ку!**", "**Привет!**", "**Ну здарова!**", "**Миу-миу-миу**", "***Приветствует по-манульи.***",
            "**МЯЯЯЯЯЯУ!**", "**Здарова!**", "**О, привет!**", "**Здравствуй!**"
        };
        
        [Command("greet")]
        [Alias("hello", "hi", "hello there", "манул", "монул", "минул", "pallascat", "pallas cat", "кот",
                "прив", "привет", "приветик", "дратути", "ку", "здаров", "здарова", "даров", "ку-ку")]
        [Summary("Привет тебе!")]
        public async Task GreetAsync()
        {
            var builder = new EmbedBuilder
            {
                Color = Config.EmbedColor,
                Description = GreetingsAnswers[_random.Next(GreetingsAnswers.Length)]
            };

            if (Context.User.Username == "null me" && _random.Next(100) + 1 <= PersonalizedResponseRate)
            {
                builder.Description = _random.Next(2) == 1
                        ? "**Привет, Лисичка!!!**"
                        : ":cat2:";
            }
            else if (Context.User.Username == "PoorMercymain" && _random.Next(100) + 1 <= PersonalizedResponseRate)
            {
                builder.Description = _random.Next(2) == 1
                        ? "**Я Вас категорически приветствую, Капитан Флексер!**"
                        : "**Клим Саныч, здравствуйте!**";
            }
            
            await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
        }
    }
}