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
            var answers = GetPossibleAnswers(input.Trim());

            if (answers.Count == 0)
            {
                builder.Description = "**А я кто, чтоб такие ребусы обдумывать?))**";
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
        const string separatorWordsPattern = "[^\\wА-Яа-яЁё\"\'](или|либо)[^\\wА-Яа-яЁё\"\']";
        var punctuation = input
                .Where(symbol => char.IsPunctuation(symbol) && symbol != '"' && symbol != '\'')
                .Distinct()
                .ToArray();
        var temp = input
                .Split(punctuation,StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Distinct()
                .ToList();
        var answers = new List<string>();

        foreach (var tempAnswer in temp)
        {
            answers.AddRange(Regex.Split(tempAnswer, separatorWordsPattern, RegexOptions.IgnoreCase).Distinct());
        }

        answers.RemoveAll(x => x.Equals("или", StringComparison.CurrentCultureIgnoreCase) ||
                               x.Equals("либо", StringComparison.CurrentCultureIgnoreCase));

        return answers;
    }
}