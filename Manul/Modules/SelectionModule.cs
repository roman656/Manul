using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Manul.Modules
{
    public class SelectionModule : ModuleBase<SocketCommandContext>
    {
        private readonly Random _random = new ();
        private readonly string[] _separators = { "[", ";", ",", ".", "]", "или", "либо", "ИЛИ", "ЛИБО", "Или", "Либо" };
        
        [Command("select"), Alias("choose", "sel", "ch", "выбери", "выбор", "выбирай", "выбрать", "реши")]
        [Summary("принимаю ответственные решения за тебя)")]
        public async Task SelectAsync([Summary("варианты (из чего выбирать)")][RemainderAttribute] string input = "")
        {
            var builder = new EmbedBuilder { Color = Config.EmbedColor };

            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                builder.Description = "**Выбирать не приходится)**";
            }
            else
            {
                var answers = input.Trim().Split(_separators, StringSplitOptions.RemoveEmptyEntries);

                if (answers.Length == 0)
                {
                    builder.Description = "**А я кто, чтоб такие ребусы обдумывать?))**";
                }
                else if (answers.Length == 1)
                {
                    builder.Description = "**Мне кажется ответ очевиден...**";
                }
                else
                {
                    builder.Description = $"**{(Context.User.Username == "PoorMercymain" ? "Саныч, я" : Context.User.Username == "null me" ? "Лисичка, я" : "Я")} думаю {answers[_random.Next(0, answers.Length)].Trim()}**";
                }
            }
            
            await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
        }
    }
}