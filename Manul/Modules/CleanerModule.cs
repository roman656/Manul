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
        private const int DelayBeforeGettingMessages = 50;
        private const int ReplyMessageDeletionDelay = 3000;
        
        [Command("clean")]
        [Alias("napalm", "fire", "clear", "зачистка", "очистка", "чистка", "огонь", "напалм", "напалмовый",
                "залп", "напалмовый залп", "резня", "уничтожить", "устранить", "нейтрализовать", "артподготовка")]
        [Summary("Обожаю запах напалма по утрам...")]
        public async Task CleanAsync([Summary("сколько сообщений уничтожить")] int amount = DefaultMessagesAmount,
                [Summary("по кому открыть огонь")][Remainder] IGuildUser user = null)
        {
            var builder = new EmbedBuilder { Color = Config.EmbedColor, Title = "🔥🔥🔥 Напалмовый залп! 🔥🔥🔥" };

            await Context.Message.DeleteAsync();

            if (!UsersWithAccess.Contains(Context.User.Username) && !ChannelsWithAccess.Contains(Context.Channel.Name))
            {
                builder.Title = string.Empty;
                builder.Description = "**Никак нет! Только по приказу начальства.**";

                var replyMessage = await Context.Channel.SendMessageAsync(string.Empty, false, builder.Build());
                
                await Task.Delay(ReplyMessageDeletionDelay);
                await replyMessage.DeleteAsync();
                
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
            
            await Task.Delay(DelayBeforeGettingMessages);

            var messages = await Context.Channel.GetMessagesAsync(amount).FlattenAsync();

            if (user != null)
            {
                messages = messages.Where(message => message.Author.Id == user.Id);
            }

            if (!messages.Any())
            {
                builder.Title = string.Empty;
                builder.Description = "**Противник не обнаружен!**";

                var replyMessage = await Context.Channel.SendMessageAsync(string.Empty, false, builder.Build());
                
                await Task.Delay(ReplyMessageDeletionDelay);
                await replyMessage.DeleteAsync();
                
                return;
            }

            var startMessage = await Context.Channel.SendMessageAsync(string.Empty, false, builder.Build());

            foreach (var message in messages)
            {
                await message.DeleteAsync();
                await Task.Delay(DeletionDelay);
            }
            
            await startMessage.DeleteAsync();
        }
    }
}