using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Manul.Modules
{
    public class SelectionModule : ModuleBase<SocketCommandContext>
    {
        private readonly Random _random = new ();
        private readonly string[] _separators = { "[", ";", ",", ".", "]", "или", "либо" };
        
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
                var answers = input.Trim().ToLower().Split(_separators, StringSplitOptions.RemoveEmptyEntries);
                builder.Description = $"**{(Context.User.Username == "PoorMercymain" ? "Саныч, я" : "Я")} думаю {answers[_random.Next(0, answers.Length)]}**";
            }
            
            await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
        }
    }
}