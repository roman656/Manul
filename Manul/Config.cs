using Discord;

namespace Manul
{
    public class Config
    {
        public const string Filename = "Config.json";
        public static Color EmbedColor = 0xff9900;
        public string Token { get; set; } = string.Empty;
        public string Prefix { get; set; } = "!";
    }
}