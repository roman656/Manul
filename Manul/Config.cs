using Discord;

namespace Manul
{
    public class Config
    {
        public const string ConfigFilename = "Config.json";
        public const string VotingDataFilename = "Voting.json";
        public const int MessageCacheSize = 500;
        public static readonly Color EmbedColor = 0xff9900;
        public static readonly string[] Prefixes = { "!", "кот ", "манул " };
        public string Token = string.Empty;
    }
}