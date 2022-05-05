using System;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Manul.Modules
{
    public class RandomModule : ModuleBase<SocketCommandContext>
    {
        private const int DefaultMaxValue = 6;
        private const int DefaultMinValue = 1;
        private const int DefaultAmount = 1;
        private const int FailureRate = 5;
        private const int MaxAmount = 20;
        private readonly Random _random = new ();

        [Command("rand"), Alias("r", "random", "р", "рандом", "ранд", "кубики", "кубик", "кости")]
        [Summary("кидаю за тебя кубики и смотрю, что выпадет)")]
        public async Task RandAsync([Summary("нижняя граница диапазона")] string minValue = "", 
                [Summary("верхняя граница диапазона")] string maxValue = "",
                [Summary("количество чисел")] string amount = "")
        {
            var result = new StringBuilder();
            var builder = new EmbedBuilder { Color = Config.EmbedColor };
            var intMinValue = DefaultMinValue;
            var intMaxValue = DefaultMaxValue;
            var intAmount = DefaultAmount;

            if (minValue != string.Empty && !int.TryParse(minValue, out intMinValue))
            {
                builder.Description = "**Вместо минимального значения я вижу какой-то мусор.**";
            }
            else if (maxValue != string.Empty && !int.TryParse(maxValue, out intMaxValue))
            {
                builder.Description = "**Вместо максимального значения я вижу какой-то мусор.**";
            }
            else if (amount != string.Empty && !int.TryParse(amount, out intAmount))
            {
                builder.Description = "**Вместо количества чисел я вижу некий HOPEWELL, что переводится как помойка.**";
            }
            else if (intMinValue == intMaxValue)
            {
                builder.Description = "**Минимальное значение равно макcимальному... На самом деле мне всё с тобой ясно, в том числе и результат, который ты ожидаешь увидеть... Но не увидишь)))**";
            }
            else if (intMinValue > intMaxValue)
            {
                builder.Description = "**Минимальное значение не может быть больше макcимального... Я запрещаю тебе такое запрашивать!**";
            }
            else if (intAmount > MaxAmount)
            {
                builder.Description = $"**Слишком много чисел ты просишь... Максимум {MaxAmount} я готов дать.**";
            }
            else if (intAmount <= 0)
            {
                builder.Description = $"**Сколько чисел ты хочешь? {intAmount}? Ты в порядке {Context.User.Mention}?)))**";
            }
            else if (_random.Next(100) + 1 <= FailureRate)
            {
                builder.Description = "**Тебе сегодня повезло... Ведь я не буду отвечать)))**";
            }
            else
            {
                intMaxValue++;    // Чтоб было включительно.
                
                for (var i = 0; i < intAmount; i++)
                {
                    result.Append(i == intAmount - 1 ? $"{_random.Next(intMinValue, intMaxValue)}"
                            : $"{_random.Next(intMinValue, intMaxValue)}, ");
                }

                if (intMinValue == DefaultMinValue && intMaxValue == DefaultMaxValue && intAmount == DefaultAmount
                        && Context.User.Username == "submarinecap" && _random.Next(100) + 1 <= 50)
                {
                    builder.Description = "**Тебе выпало: 1113**";
                }
                else
                {
                    builder.Description = $"**Тебе выпало: {result}**";
                }
            }
            
            await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
        }
    }
}