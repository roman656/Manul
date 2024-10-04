using System;
using System.IO;
using Discord;
using Newtonsoft.Json;

namespace Manul;

public class Config
{
    private const string ConfigFileDefaultPath = "Config.json";
    public const string VotingDataFilename = "Voting.json";
    public const int MessageCacheSize = 500;
    public static readonly Color EmbedColor = 0xFF9900;
    
    [JsonProperty(nameof(Prefix))]
    public static string Prefix { get; private set; } = "!";
    
    [JsonProperty(nameof(GameActivityName))]
    public static string GameActivityName { get; private set; } = "!help";
    
    [JsonProperty(nameof(Token))]
    public static string Token { get; private set; } = string.Empty;
    
    [JsonProperty(nameof(LogToConsole))]
    public static bool LogToConsole { get; private set; } = true;
    
    [JsonProperty(nameof(LogToFile))]
    public static bool LogToFile { get; private set; } = false;

    public static void Upload(string configFilePath)
    {
        configFilePath ??= ConfigFileDefaultPath;
        JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFilePath));

        if (string.IsNullOrEmpty(Token))
        {
            throw new ArgumentException($"There is no token specified in the configuration file: {configFilePath}");
        }
    }
}