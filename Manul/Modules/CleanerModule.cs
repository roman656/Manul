using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Manul.Modules
{
    public class CleanerModule : ModuleBase<SocketCommandContext>
    {
        private const int DefaultMessagesAmount = 10;
        private const int МахMessagesAmount = 20;
        
        [Command("clean"), Alias("napalm", "зачистка", "очистка", "чистка", "огонь", "напалм", "напалмовый", "залп", "напалмовый залп")]
        [Summary("Обожаю запах напалма по утрам...")]
        public async Task CleanAsync([Summary("сколько сообщений уничтожить")] int amount = DefaultMessagesAmount)
        {
            var builder = new EmbedBuilder {Color = Config.EmbedColor, Title = "🔥🔥🔥 Напалмовый залп! 🔥🔥🔥"};

            if (Context.User.Username != "pomaxpen")
            {
                builder.Title = "";
                builder.Description = "Никак нет! Только по приказу командира.";
                await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
                return;
            }

            if (amount > МахMessagesAmount)
            {
                amount = МахMessagesAmount;
            }
            else if (amount < 1)
            {
                amount = 1;
            }
            
            var messages = await Context.Channel.GetMessagesAsync(amount).FlattenAsync();
            var startMessage = await Context.Message.ReplyAsync(string.Empty, false, builder.Build());

            foreach (var message in messages)
            {
                await message.DeleteAsync();
                await Task.Delay(1000);
            }
            
            await startMessage.DeleteAsync();
        }
    }
}