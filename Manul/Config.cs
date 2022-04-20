using Discord;

namespace Manul
{
    public class Config
    {
        public const string Filename = "Config.json";
        public const string Prefix = "!";
        public const int MessageCacheSize = 500;
        public static Color EmbedColor = 0xff9900;
        public string Token { get; set; } = string.Empty;
    }
}