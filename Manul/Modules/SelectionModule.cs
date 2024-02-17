using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Manul.Modules;

public class SelectionModule : ModuleBase<SocketCommandContext>
{
    private readonly Random _random = new ();
    private readonly Dictionary<string, string> _specialUsersAnswers = new()
    {
        { "null_me", "Лисичка, я" },
        { "poormercymain", "Саныч, я" }
    };

    [Command("select"), Alias("choose", "sel", "ch", "выбери", "выбор", "выбирай", "выбрать", "реши")]
    [Summary("принимаю ответственные решения за тебя)")]
    public async Task SelectAsync([Summary("варианты (из чего выбирать)")][Remainder] string input = "")
    {
        var builder = new EmbedBuilder { Color = Config.EmbedColor };

        if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
        {
            builder.Description = "**Выбирать не приходится)**";
        }
        else
        {
            var answers = GetPossibleAnswers(input);

            if (answers.Count == 0)
            {
                builder.Description = "**А я кто, чтоб такие ребусы обдумывать?))**";
            }
            else if (answers.Count == 1)
            {
                builder.Description = "**Ответ настолько очевиден, что я даже не ответ...**";
            }
            else
            {
                var selectedAnswer = answers[_random.Next(0, answers.Count)];
                
                if (_specialUsersAnswers.TryGetValue(Context.User.Username, out var specialUserAnswer))
                {
                    builder.Description = $"**{specialUserAnswer} думаю {selectedAnswer}**";
                }
                else
                {
                    builder.Description = $"**Я думаю {selectedAnswer}**";
                }
            }
        }

        await Context.Message.ReplyAsync(embed: builder.Build());
    }
    
    private static List<string> GetPossibleAnswers(string input)
    {
        var regex = new Regex(
                """^(?:или|либо|or)|(?:или|либо|or)$|[^\wА-Яа-яЁё\"\'\`?!.:]+(?:или|либо|or)[^\wА-Яа-яЁё\"\'\`?!.:]+(?:(?:или|либо|or)[^\wА-Яа-яЁё\"\'\`?!.:])*|\s*[,;\\\/|]\s*""",
                RegexOptions.IgnoreCase);
    
        input = input.Trim();
    
        var answers = regex
                .Split(input)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

        return answers;
    }
}