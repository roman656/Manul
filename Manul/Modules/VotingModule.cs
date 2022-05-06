using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Manul.Modules
{
    public class VotingModule : ModuleBase<SocketCommandContext>
    {
        private readonly string[] _separators = { "[", ";", ",", ".", "]", "или", "либо", "ИЛИ", "ЛИБО", "Или", "Либо" };

        public VotingModule(DiscordSocketClient client) => client.SelectMenuExecuted += SelectMenuHandler;

        [Command("vote"), Alias("v", "p", "о", "г", "poll", "опрос", "голосование")]
        [Summary("обеспечиваю качественную организацию твоих опросов")]
        public async Task VoteAsync([Summary("уникальное (среди активных) название опроса")] string name,
                [Summary("минимальное количество выбранных ответов")] int minAnswersAmount,
                [Summary("максимальное количество выбранных ответов")] int maxAnswersAmount,
                [Summary("тема опроса")] string theme,
                [Summary("варианты ответа")][RemainderAttribute] string answers)
        {
            var builder = new EmbedBuilder { Color = Config.EmbedColor };
            
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                builder.Description = "**Название опроса необходимо... Лично for me...**";
                await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
                return;
            }
            
            name = name.Trim();

            foreach (var votingData in Program.VotingData)
            {
                if (votingData.Name == name)
                {
                    builder.Description = "**А опрос с таким названием уже есть)**";
                    await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
                    return;
                }
            }

            if (string.IsNullOrEmpty(theme) || string.IsNullOrWhiteSpace(theme))
            {
                builder.Description = "**На какую тему опрос?**";
                await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
                return;
            }
            
            theme = theme.Trim();
            
            if (string.IsNullOrEmpty(answers) || string.IsNullOrWhiteSpace(answers))
            {
                builder.Description = "**А варианты ответов мне самому придумать? Я конечно могу, но...**";
                await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
                return;
            }

            builder.Title = $"Автор опроса - *{Context.User.Username}*";
            builder.Description = $"**Тема:** ***{theme}***";
                
            var buttonMessages = answers.Trim().Split(_separators, StringSplitOptions.RemoveEmptyEntries);
            var answersDictionary = new Dictionary<int, string>();

            for (var i = 0; i < buttonMessages.Length; i++)
            {
                buttonMessages[i] = buttonMessages[i].Trim();
                answersDictionary[i] = buttonMessages[i];
            }

            if (minAnswersAmount < 1)
            {
                minAnswersAmount = 1;
            }
            
            if (maxAnswersAmount < 1)
            {
                maxAnswersAmount = 1;
            }
            
            if (minAnswersAmount > buttonMessages.Length)
            {
                minAnswersAmount = buttonMessages.Length;
            }
            
            if (maxAnswersAmount > buttonMessages.Length)
            {
                maxAnswersAmount = buttonMessages.Length;
            }
            
            if (minAnswersAmount > maxAnswersAmount)
            {
                minAnswersAmount = maxAnswersAmount;
            }
            
            var menuBuilder = new SelectMenuBuilder().WithPlaceholder("Давай выбирай").WithCustomId(name)
                    .WithMinValues(minAnswersAmount).WithMaxValues(maxAnswersAmount);

            for (var index = 0; index < buttonMessages.Length; index++)
            {
                menuBuilder.AddOption(buttonMessages[index], index.ToString());
            }

            Program.VotingData.Add(new VotingData(name, theme, answersDictionary, Context.User.Username));

            await Context.Message.ReplyAsync(string.Empty, false, builder.Build(),
                    components: new ComponentBuilder().WithSelectMenu(menuBuilder).Build());
        }

        [Command("voting_results"), Alias("vr", "pr")]
        [Summary("подведение итогов опроса")]
        public async Task ShowVotingResultsAsync([Summary("название опроса")] string name)
        {
            var builder = new EmbedBuilder { Color = Config.EmbedColor };
            
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                builder.Description = "**А название опроса где?**";
                await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
                return;
            }

            name = name.Trim();

            foreach (var votingData in Program.VotingData)
            {
                if (votingData.Name == name)
                {
                    if (votingData.Author != Context.User.Username)
                    {
                        builder.Description = $"**А это не твой опрос. Пусть {votingData.Author} его завершает)**";
                        await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
                        return;
                    }
                    
                    builder.Title = $"Результаты опроса от *{votingData.Author}*";
                    builder.Description = $"**Тема:** ***{votingData.Theme}***";

                    var results = new Dictionary<int, int>();

                    foreach (var answer in votingData.Answers)
                    {
                        results[answer.Key] = 0;
                    }

                    foreach (var userAnswer in votingData.UserAnswers)
                    {
                        foreach (var value in userAnswer.Value)
                        {
                            results[value] += 1;
                        }
                    }

                    for (var i = 0; i < results.Count; i++)
                    {
                        builder.AddField(x =>
                        {
                            x.Name = $"{results[i]} манул{(results[i] == 0 || results[i] >= 5 ? "ов" : results[i] == 1 ? "" : "а")} выбрал{(results[i] == 0 || results[i] >= 2 ? "о" : "")}:";
                            x.Value = $"{votingData.Answers[i]}";
                            x.IsInline = false;
                        });
                    }

                    for (var j = 0; j < Program.VotingData.Count; j++)
                    {
                        if (Program.VotingData[j].Name == name)
                        {
                            Program.VotingData.RemoveAt(j);
                            break;
                        }
                    }

                    await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
                    return;
                }
            }
            
            builder.Description = "**Я чёт такого опроса не помню((**";
            await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
        }

        private async Task SelectMenuHandler(SocketMessageComponent argument)
        {
            foreach (var votingData in Program.VotingData)
            {
                if (votingData.Name == argument.Data.CustomId)
                {
                    votingData.UserAnswers[argument.User.Username] = new List<int>();
                    
                    foreach (var value in argument.Data.Values)
                    {
                        votingData.UserAnswers[argument.User.Username].Add(int.Parse(value));
                    }
                    
                    await argument.RespondAsync("Дописал в книжечку мнение.");
                    await Task.Delay(500);
                    await argument.DeleteOriginalResponseAsync();
                    break;
                }
            }
        }
    }
}