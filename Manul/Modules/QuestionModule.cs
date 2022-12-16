using System.Collections.Generic;
using System.Linq;
using Discord.Addons.Music.Common;
using Discord.Addons.Music.Player;
using Discord.Addons.Music.Source;
using Discord.Audio;
using Discord.WebSocket;

namespace Manul.Modules;

using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

public class QuestionModule : ModuleBase<SocketCommandContext>
{
    private readonly Random _random = new ();
    private readonly string[] _questionAnswers =
    {
        "Да", "Нет", "Скорее да", "Скорее нет", "Крутой вопрос! Отвечать на него я, конечно, не буду...", "Неа)",
        "Не знаю)", "Возможно", "Может быть...", "ОФКОРС!", "Нетъ", "Нинаю)", "Не, ну ты даёшь... Конечно же нет!",
        "Почему бы и нет)", "А вот и нет!", "Не угадал!", "Угадал, верно!", "Да нет", "Угадай)", "Да нет, наверное",
        "Разве?", "Ну и бредятина", "Кто-нибудь держите меня, а то я втащу ему!", "Да-да", "Ещё чего спросишь?",
        "Ты и сам знаешь верный ответ...", "На этот вопрос я буду отвечать только в присутствии адвоката!", "Дыа)",
        "Не-не-не", "Да-с", "Нет-с", "Мда...", "Nope", "No", "Yep!", "Yes", "NOOOOOOOOOOOO!", "Мдя", "Угу", "Ага)",
        "YEEEEEEEEEEES!", "За тобой выехали...", "Ты как из палаты выбрался?", ")))", "(((", "))", "((", ")", "(",
        "Я уже передал распечатку с твоими координатами в ближайшее расположение ракетных войск...", "Это неважно)",
        "Думал я тебе отвечу? Но ты спросил без уважения...", "Однозначно нет!", "Однозначно да!", "Ну конечно!",
        "Я считаю оскорбительным для своего интеллекта отвечать на такое...", "Похоже, что так)", "Ни в кoeм paзe!",
        "Может и да... А может и нет)", "Кто куда, а ты в бан", "Сам подумай)", "А я то тут при чём?!", "Неясно",
        "Я тебе запрещаю задавать такие вопросы!", "Это не имеет значения... Для тебя)))", "Непонятно", "Так точно!",
        "Дементий, блин, я сколько раз уже говорил такие вопросы не задавать?! Меня от них пучит!",
        "Однажды мне приснилось, что ты передумал такое спрашивать. И быстро!", "Никак нет!",
        "Однажды я сидел... и вдруг я понял - НЕТ!", "Шо ты там сказал, а? Я не слушал просто)))",
        "Я не знаю, что ответить, поэтому воспользуюсь помощью друга. Шлёпа, вопрос Вам)"
    };

    [Command("question"), Alias("вопрос", "ответь", "ответ", "спросить", "слушай", "ask", "answer",
            "допрос", "отвечай", "атвичай", "вопросик")]
    [Summary("отвечу на любой твой вопрос... подразумевающий ответ да/нет))")]
    public async Task QuestionAsync([Summary("твой вопросик")][RemainderAttribute] string input = "")
    {
        var builder = new EmbedBuilder { Color = Config.EmbedColor };

        if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
        {
            builder.Description = "**Я чёт вопрос не понял. Я молодец!**";
        }
        else
        {
            builder.Description = $"**{_questionAnswers[_random.Next(_questionAnswers.Length)]}**";
        }
            
        await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
        
        // Initialize AudioPlayer
        AudioPlayer audioPlayer = new AudioPlayer();

// Set player's audio client
// This is required for AudioPlayer to create an audio stream to Discord
        SocketVoiceChannel voiceChannel = (Context.User as SocketGuildUser)?.VoiceChannel;
        var audioClient = await voiceChannel.ConnectAsync();
        audioPlayer.SetAudioClient(audioClient);
        
        string query = input;
        bool wellFormedUri = Uri.IsWellFormedUriString(query, UriKind.Absolute);
        List<AudioTrack> tracks = await TrackLoader.LoadAudioTrack(query, fromUrl: wellFormedUri);

// Pick the first entry and use AudioPlayer.StartTrack to play it on Thread Pool
        AudioTrack firstTrack = tracks.ElementAt(0);
        

// OR
// await track to finish playing
        await audioPlayer.StartTrackAsync(firstTrack);
    }
    
    public class TrackScheduler
    {
        public Queue<AudioTrack> SongQueue { get; set; }
        private AudioPlayer player;

        public TrackScheduler(AudioPlayer player)
        {
            SongQueue = new Queue<AudioTrack>();
            this.player = player;
            this.player.OnTrackStartAsync += OnTrackStartAsync;
            this.player.OnTrackEndAsync += OnTrackEndAsync;
        }

        public Task Enqueue(AudioTrack track)
        {
            if (player.PlayingTrack != null)
            {
                SongQueue.Enqueue(track);
            }
            else
            {
                // fire and forget
                player.StartTrackAsync(track).ConfigureAwait(false);
            }
            return Task.CompletedTask;
        }

        public async Task NextTrack()
        {
            AudioTrack nextTrack;
            if (SongQueue.TryDequeue(out nextTrack))
                await player.StartTrackAsync(nextTrack);
            else
                player.Stop();
        }

        private Task OnTrackStartAsync(IAudioClient audioClient, IAudioSource track)
        {
            Console.WriteLine("Track start! " + track.Info.Title);
            return Task.CompletedTask;
        }

        private async Task OnTrackEndAsync(IAudioClient audioClient, IAudioSource track)
        {
            Console.WriteLine("Track end! " + track.Info.Title);

            await NextTrack();
        }
    }
}