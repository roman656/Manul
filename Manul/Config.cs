namespace Manul;

using Discord;

public class Config
{
    public const string ConfigFilename = "Config.json";
    public const string VotingDataFilename = "Voting.json";
    public const int MessageCacheSize = 500;
    public static readonly Color EmbedColor = 0xff9900;
    public const string Prefix = "!";
    public string Token { get; set; } = string.Empty;
}