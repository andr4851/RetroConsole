using System.Text.RegularExpressions;

namespace RetroConsole.Utility
{
    public static class Tokenizator
    {
        public static readonly Regex TokenRx = new Regex(
        @"""(?<val>(?:\\""|[^""])*)""" +
        @"|(?<val>(?:\\\s|[^\s])+)",
        RegexOptions.Compiled
        );

        public static string Unescape(string s) =>
            Regex.Replace(s, @"\\([\s""])", "$1");
    }
}
