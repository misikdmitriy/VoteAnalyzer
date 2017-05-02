using System;

namespace VoteAnalyzer.Parser
{
    public class WordParser : IParser<string, string[]>
    {
        private static readonly char[] Delimeters = { ' ', '.', ',' };

        public string[] Parse(string argument)
        {
            return string.IsNullOrEmpty(argument) 
                ? new string[0] 
                : argument.Split(Delimeters, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
