using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Manul.Exceptions;
using Newtonsoft.Json;

namespace Manul
{
    public class Program
    {
        private static Config _config;
        private DiscordSocketClient _client;

        public static void Main(string[] args)
        {
            try
            {
                CheckFile(Config.Filename);
                _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Config.Filename));
                new Program().MainAsync().GetAwaiter().GetResult();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error: " + exception.Message);
            }
        }

        private async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;
            
            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }
        
        private static Task Log(LogMessage message)
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
        
        private static void CheckFile(in string filename)
        {
            var fileInfo = new FileInfo(filename);
            
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException($"File {filename} not found.");
            }

            if (fileInfo.Length <= 0)
            {
                throw new EmptyFileException($"File {filename} is empty.");
            }
        }
    }
}