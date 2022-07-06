namespace Manul.Modules;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

public class CleanerModule : ModuleBase<SocketCommandContext>
{
    private static readonly string[] UsersWithAccess = { "pomaxpen", "null me", "Mercer" };
    private static readonly string[] ChannelsWithAccess = { "алое-озеро" };
    private const int DefaultMessagesAmount = 15;
    private const int МахMessagesAmount = 30;
    private const int ScanningMessagesAmount = 200;
    private const int DeletionDelay = 1000;
    private const int DelayBeforeGettingMessages = 100;
    private const int ReplyMessageDeletionDelay = 3000;

    [Command("clean")]
    [Alias("napalm", "fire", "clear", "зачистка", "очистка", "чистка", "огонь", "напалм", "напалмовый",
            "залп", "напалмовый залп", "резня", "уничтожить", "устранить", "нейтрализовать", "артподготовка")]
    [Summary("обожаю запах напалма по утрам...")]
    public async Task CleanAsync([Summary("сколько сообщений уничтожить")] int amount = DefaultMessagesAmount,
            [Summary("по кому открыть огонь")][Remainder] IGuildUser user = null)
    {
        await Context.Message.DeleteAsync();
        ConstrainAmount(ref amount);

        if (!UsersWithAccess.Contains(Context.User.Username) && !ChannelsWithAccess.Contains(Context.Channel.Name))
        {
            await SendReplyAsync("**Никак нет! Только по приказу начальства.**");
            return;
        }
            
        IEnumerable<IMessage> messages;

        await Task.Delay(DelayBeforeGettingMessages);

        if (user != null)
        {
            messages = await Context.Channel.GetMessagesAsync(ScanningMessagesAmount).FlattenAsync();
            messages = messages.Where(message => message.Author.Id == user.Id);
        }
        else
        {
            messages = await Context.Channel.GetMessagesAsync(amount).FlattenAsync();
        }

        var messagesList = messages.ToList();
            
        if (messagesList.Count == 0)
        {
            await SendReplyAsync("**Противник не обнаружен!**");
            return;
        }

        if (messagesList.Count > amount)
        {
            messagesList = messagesList.GetRange(0, amount);
        }

        var builder = new EmbedBuilder { Color = Config.EmbedColor, Title = "🔥🔥🔥 Напалмовый залп! 🔥🔥🔥" };
        var startMessage = await Context.Channel.SendMessageAsync(string.Empty, false, builder.Build());

        foreach (var message in messagesList)
        {
            await message.DeleteAsync();
            await Task.Delay(DeletionDelay);
        }

        await startMessage.DeleteAsync();
    }

    private async Task SendReplyAsync(string description)
    {
        var builder = new EmbedBuilder { Color = Config.EmbedColor, Description = description };
        var replyMessage = await Context.Channel.SendMessageAsync(string.Empty, false, builder.Build());

        await Task.Delay(ReplyMessageDeletionDelay);
        await replyMessage.DeleteAsync();
    }
        
    private static void ConstrainAmount(ref int amount)
    {
        if (amount > МахMessagesAmount)
        {
            amount = МахMessagesAmount;
        }
        else if (amount < 1)
        {
            amount = 1;
        }
    }
}