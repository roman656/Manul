using Discord;
using Newtonsoft.Json;

namespace Manul;

public class Config
{
    public const string ConfigFilename = "Config.json";
    public const string VotingDataFilename = "Voting.json";
    public const int MessageCacheSize = 500;
    public static readonly Color EmbedColor = 0xFF9900;
    
    [JsonProperty(nameof(Prefix))]
    public static string Prefix { get; set; } = "!";
    
    [JsonProperty(nameof(GameActivityName))]
    public static string GameActivityName { get; set; } = "!help";
    
    [JsonProperty(nameof(Token))]
    public static string Token { get; set; } = string.Empty;
}