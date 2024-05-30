namespace Manul.Modules;

using System.Diagnostics;
using Discord.Addons.Music.Source;
using Discord.Audio;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Music.Common;
using Discord.Addons.Music.Player;
using Discord.Commands;
using Discord.WebSocket;

public class MusicModule : ModuleBase<SocketCommandContext>
{
    private readonly Random _random = new();
    private readonly string[] _commentAnswers =
    {
        "А чё, звучит чотыре)", "Вот это запрос...", "Мда...", "Ну, понеслась!", ")))", "(((", "))",
        "((", ")", "(", "Дементий, блин, я сколько раз уже говорил такие запросы не делать?!",
        "Я, кажется, оглох...", "Интересный выбор)"
    };
    private readonly Dictionary<string, string> _manulSongs = new()
    {
        { "радио бандитов 📻", "https://youtu.be/Nhrhb9QPCjE" },
        { "Барановичи 🐏", "https://youtu.be/BO1nxYNgg7M" },
        { "шрексофон 🎷", "https://youtu.be/6u28g47nlPQ" },
        { "Dr. Livesey walking 🚶🚶🚶", "https://youtu.be/tt8Vy42WHVY" },
        { "лалахей 🎤", "https://youtu.be/mBQGYdDitgc" },
        { "спидран 🏎", "https://youtu.be/JseIaLyGNuc" },
        { "я не поеду в Китай 🐼", "https://youtu.be/RdbWairj0no" },
        { "зато я спас кота 😸", "https://youtu.be/2KDZg1-7KsE" },
        { "ШИРОКАЯ музыка 🎹", "https://youtu.be/M2aQFtWNXRA" }
    };
    private readonly Dictionary<ulong, (AudioPlayer player, Queue<AudioTrack> queue)> _serverAudioPlayers = new();
    
    private void AddNewAudioPlayer(ulong serverId)
    {
        if (!_serverAudioPlayers.ContainsKey(serverId))
        {
            var player = new AudioPlayer();
            var queue = new Queue<AudioTrack>();

            player.OnTrackStartAsync += OnTrackStartAsync;
            player.OnTrackEndAsync += OnTrackEndAsync;

            _serverAudioPlayers.Add(serverId, (player, queue));
        }
    }

    private static Task OnTrackStartAsync(IAudioClient audioClient, IAudioSource track)
    {
        Console.WriteLine("Track start! " + track.Info.Title);
        return Task.CompletedTask;
    }

    private static async Task OnTrackEndAsync(IAudioClient audioClient, IAudioSource track)
    {
        Console.WriteLine("Track end! " + track.Info.Title);
        await audioClient.StopAsync();
    }

    [Command("music"), Alias("play", "музыка", "песня", "аудио", "audio", "song", "трек", "песню",
            "шарманка", "поставь", "запусти", "играй", "бард", "мелодия", "понеслась", "сыграй")]
    [Summary("не стреляйте в пианиста – он играет, как умеет)")]
    public async Task MusicAsync([Summary("ссылка на песню или запрос словами")][Remainder] string query = "")
    {
        var builder = new EmbedBuilder { Color = Config.EmbedColor };
        var isUserQueryEmpty = false;

        if (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query))
        {
            (var songName, query) = _manulSongs.ElementAt(_random.Next(0, _manulSongs.Count));
            builder.Description = $"**Ну раз предложений по музыке не поступило... Выбор сделаю я)\nРешено — {songName}**";
            isUserQueryEmpty = true;
        }

        var guild = (Context.User as SocketGuildUser)?.Guild;

        if (guild == null)
        {
            builder.Description = "**Не осмысляю...**";
            await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
            return;
        }

        AddNewAudioPlayer(guild.Id);

        var (player, _) = _serverAudioPlayers[guild.Id];
        var voiceChannel = (Context.User as SocketGuildUser)?.VoiceChannel;

        if (voiceChannel == null)
        {
            builder.Description = "**А ты где? В куда играть?))**";
            await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
            return;
        }

        var audioClient = await voiceChannel.ConnectAsync();
        var wellFormedUri = Uri.IsWellFormedUriString(query, UriKind.Absolute);
        var isYouTubeUri = (query?.Contains("youtube")  ?? false) || (query?.Contains("youtu.be")  ?? false);
        
        if (!wellFormedUri)
        {
            query = Translit(query);    // костыль для +/- нормального поиска на русском.
        }
        else if (!isYouTubeUri)
        {
            var sunoSongId = ExtractSunoSongId(query);

            if (!string.IsNullOrEmpty(sunoSongId))
            {
                query = $"https://cdn1.suno.ai/{sunoSongId}.mp3";
            }

            var ffmpegCommand = $"-c \"youtube-dl -o - {query} | ffmpeg -loglevel panic -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1\"";
            var ffmpegProcess = Process.Start(new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = ffmpegCommand,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });

            if (ffmpegProcess == null)
            {
                builder.Description = $"**{Context.User.Mention} поставил песню, но я помер...**";
                await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
                
                return;
            }
            
            builder.Description = $"**{Context.User.Mention} поставил:\n*{query}*\n{_commentAnswers[_random.Next(_commentAnswers.Length)]}**";
            await Context.Message.ReplyAsync(string.Empty, false, builder.Build());

            using (var output = ffmpegProcess.StandardOutput.BaseStream)
            using (var discord = audioClient.CreatePCMStream(AudioApplication.Music))
            {
                try
                {
                    await output.CopyToAsync(discord);
                }
                finally
                {
                    await discord.FlushAsync();
                }
            }
            
            return;
        }

        var tracks = await TrackLoader.LoadAudioTrack(query, fromUrl: wellFormedUri);
        var firstTrack = tracks[0];

        if (!isUserQueryEmpty)
        {
            builder.ThumbnailUrl = firstTrack.Info.ThumbnailUrl;
            builder.Description = $"**{Context.User.Mention} поставил:\n*{firstTrack.Info.Title}*\n{_commentAnswers[_random.Next(_commentAnswers.Length)]}**";
        }

        await Context.Message.ReplyAsync(string.Empty, false, builder.Build());

        player.SetAudioClient(audioClient);

        await player.StartTrackAsync(firstTrack);
    }
    
    private static string ExtractSunoSongId(string shareLink)
    {
        const string songPrefix = "https://suno.com/song/";

        return shareLink.StartsWith(songPrefix) ? shareLink.Substring(songPrefix.Length) : string.Empty;
    }

    [Command("stop"), Alias("выключай", "вырубай", "стоп", "довольно", "хватит")]
    [Summary("выключаю проигрыватель...")]
    public async Task StopPlayingAsync()
    {
        var builder = new EmbedBuilder { Color = Config.EmbedColor };
        var guild = (Context.User as SocketGuildUser)?.Guild;

        if (guild == null)
        {
            builder.Description = "**Не осмысляю...**";
            await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
            return;
        }

        AddNewAudioPlayer(guild.Id);

        var (player, _) = _serverAudioPlayers[guild.Id];
        var voiceChannel = (Context.User as SocketGuildUser)?.VoiceChannel;

        if (voiceChannel == null)
        {
            builder.Description = "**Хорошая попытка))**";
            await Context.Message.ReplyAsync(string.Empty, false, builder.Build());
            return;
        }

        var audioClient = await voiceChannel.ConnectAsync();

        player.SetAudioClient(audioClient);

        await audioClient.StopAsync();
    }

    private static string Translit(string str)
    {
        string[] latUp = { "A", "B", "V", "G", "D", "E", "Yo", "Zh", "Z", "I", "Y", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "Kh", "Ts", "Ch", "Sh", "Shch", "\"", "Y", "'", "E", "Yu", "Ya" };
        string[] latLow = { "a", "b", "v", "g", "d", "e", "yo", "zh", "z", "i", "y", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "kh", "ts", "ch", "sh", "shch", "\"", "y", "'", "e", "yu", "ya" };
        string[] rusUp = { "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };
        string[] rusLow = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };

        for (var i = 0; i <= 32; i++)
        {
            str = str.Replace(rusUp[i], latUp[i]);
            str = str.Replace(rusLow[i], latLow[i]);
        }

        return str;
    }
}