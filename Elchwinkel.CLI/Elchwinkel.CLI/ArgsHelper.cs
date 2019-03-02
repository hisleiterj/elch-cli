using System;
using System.Collections.Generic;
using System.Linq;

namespace Elchwinkel.CLI
{
    internal static class ArgsHelper
    {
        private static IEnumerable<string> SplitCommandLine(string commandLine)
        {
            var inQuotes = false;

            return commandLine.Split(c =>
                {
                    if (c == '\"')
                        inQuotes = !inQuotes;

                    return !inQuotes && c == ' ';
                })
                .Select(arg => TrimMatchingQuotes(arg.Trim(), '\"'))
                .Where(arg => !string.IsNullOrEmpty(arg));
        }
        private static string TrimMatchingQuotes(this string input, char quote)
        {
            if ((input.Length >= 2) &&
                (input[0] == quote) && (input[input.Length - 1] == quote))
                return input.Substring(1, input.Length - 2);

            return input;
        }
        private static IEnumerable<string> Split(this string str, Func<char, bool> controller)
        {
            var nextPiece = 0;
            for (var c = 0; c < str.Length; c++)
            {
                if (!controller(str[c])) continue;
                yield return str.Substring(nextPiece, c - nextPiece);
                nextPiece = c + 1;
            }
            yield return str.Substring(nextPiece);
        }
        public static string[] Parse(string input) => SplitCommandLine(input).ToArray();

        public static string GetCommandPart(string input) => Parse(input).FirstOrDefault();

        public static string[] GetArgsPart(string input) => Parse(input).Skip(1).ToArray();
    }
}