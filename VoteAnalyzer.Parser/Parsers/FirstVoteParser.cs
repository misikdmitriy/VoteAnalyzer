using System;
using System.Collections.Generic;
using System.Linq;

using VoteAnalyzer.Common.Constants;
using VoteAnalyzer.Common.Extensions;
using VoteAnalyzer.Parser.Models;

namespace VoteAnalyzer.Parser.Parsers
{
    /// <summary>
    /// Search first known vote
    /// </summary>
    public class FirstVoteParser : AbstractParser<string[], FirstVoteParserModel>
    {
        public override FirstVoteParserModel Parse(string[] argument)
        {
            var taken = FindVote(argument);

            if (taken == -1)
            {
                throw new ArgumentException("cannot find vote");
            }

            return new FirstVoteParserModel
            {
                Vote = Constants.ExistingVotes[taken].Aggregate((current, next) => $"{current} {next}")
            };
        }

        private static int FindVote(IEnumerable<string> argument)
        {
            var minIndex = int.MaxValue;
            var taken = -1;

            foreach (var existingVote in Constants.ExistingVotes)
            {
                var index = argument.IndexOfSubsequence(existingVote);

                if (index != -1 && minIndex > index)
                {
                    taken = Array.IndexOf(Constants.ExistingVotes, existingVote);
                    minIndex = index;
                }
            }

            return taken;
        }
    }
}
