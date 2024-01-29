using Discord;

namespace Manul;

public class Config
{
    public const string ConfigFilename = "Config.json";
    public const string VotingDataFilename = "Voting.json";
    public const int MessageCacheSize = 500;
    public static readonly Color EmbedColor = 0xFF9900;
    public const string Prefix = "!";
    public const string GameActivityName = "!help";
    public string Token { get; set; } = string.Empty;
}