using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Manul.Modules
{
    public class CleanerModule : ModuleBase<SocketCommandContext>
    {
        private static readonly string[] UsersWithAccess = { "pomaxpen", "null me", "Mercer" };
        private static readonly string[] ChannelsWithAccess = { "алое-озеро" };
        private const int DefaultMessagesAmount = 15;
        private const int МахMessagesAmount = 30;
        private const int DeletionDelay = 1000;
        
        [Command("clean")]
        [Alias("napalm", "зачистка", "очистка", "чистка", "огонь", "напалм", "напалмовый", "залп",
                "напалмовый залп", "резня", "уничтожить", "устранить", "артподготовка")]
        [Summary("Обожаю запах напалма по утрам...")]
        public async Task CleanAsync([Summary("сколько сообщений уничтожить")] int amount = DefaultMessagesAmount,
                [Summary("по кому открыть огонь")][Remainder] IGuildUser user = null)
        {
            var builder = new EmbedBuilder { Color = Config.EmbedColor, Title = "🔥🔥🔥 Напалмовый залп! 🔥🔥🔥" };
            
            if (!UsersWithAccess.Contains(Context.User.Username) && !ChannelsWithAccess.Contains(Context.Channel.Name))
            {
                builder.Title = "";
                builder.Description = "Никак нет! Только по приказу начальства.";
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

            if (user != null)
            {
                messages = messages.Where(message => message.Author.Id == user.Id);
            }

            var startMessage = await Context.Message.ReplyAsync(string.Empty, false, builder.Build());

            foreach (var message in messages)
            {
                await message.DeleteAsync();
                await Task.Delay(DeletionDelay);
            }
            
            await startMessage.DeleteAsync();
        }
    }
}